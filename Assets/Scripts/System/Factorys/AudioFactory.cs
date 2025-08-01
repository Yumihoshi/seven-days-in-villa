using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioFactory - 音频工厂类
/// 支持全局音量、静音、命名音源、对象池、淡入淡出等功能
/// </summary>
public class AudioFactory
{
    private static AudioFactory _instance;
    public static AudioFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AudioFactory();
            }
            return _instance;
        }
    }

    // 池设置
    private int _initialPoolSize = 10;
    private int _maxPoolSize = 50;

    // 数据结构
    private Queue<AudioSource> _audioSourcePool;
    private List<AudioSource> _activeAudioSources;
    private Dictionary<string, AudioSource> _namedAudioSources;
    private Transform _audioSourceParent;
    private MonoBehaviour _coroutineRunner;

    // 全局静音
    private bool _isMuted = false;

    // 全局音量
    private float _masterVolume = 1f;

    /// <summary>
    /// 私有构造函数
    /// </summary>
    private AudioFactory() { }

    /// <summary>
    /// 初始化音频工厂
    /// </summary>
    public void Initialize(MonoBehaviour coroutineRunner, int initialPoolSize = 10, int maxPoolSize = 50)
    {
        this._coroutineRunner = coroutineRunner;
        this._initialPoolSize = Mathf.Max(1, initialPoolSize);
        this._maxPoolSize = Mathf.Max(this._initialPoolSize, maxPoolSize);

        if (_audioSourceParent == null)
        {
          
            _audioSourceParent = coroutineRunner.transform;
        }

        _audioSourcePool = new Queue<AudioSource>();
        _activeAudioSources = new List<AudioSource>();
        _namedAudioSources = new Dictionary<string, AudioSource>();

        for (int i = 0; i < this._initialPoolSize; i++)
        {
            var src = CreateNewAudioSource();
            _audioSourcePool.Enqueue(src);
        }

        Debug.Log($"AudioFactory 初始化完成 - 初始池大小: {this._initialPoolSize}, 最大池大小: {this._maxPoolSize}");
    }

    /// <summary>
    /// 创建新的 AudioSource
    /// </summary>
    private AudioSource CreateNewAudioSource()
    {
        GameObject audioObj = new GameObject($"AudioSource_{_audioSourcePool?.Count + _activeAudioSources?.Count ?? 0}");
        audioObj.transform.SetParent(_audioSourceParent);
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = _masterVolume;
        audioSource.pitch = 1f;
        audioSource.spatialBlend = 0f;
        audioSource.mute = _isMuted;
        audioObj.SetActive(false);
        return audioSource;
    }

    /// <summary>
    /// 根据名字创建AudioSource（命名音源）
    /// </summary>
    public AudioSource Create(string audioSourceName)
    {
        if (_coroutineRunner == null)
        {
            Debug.LogError("AudioFactory 未初始化，请先调用 Initialize 方法");
            return null;
        }
        if (string.IsNullOrEmpty(audioSourceName))
        {
            Debug.LogError("AudioFactory: AudioSource名字不能为空");
            return null;
        }
        if (_namedAudioSources.ContainsKey(audioSourceName))
        {
            Debug.LogWarning($"AudioFactory: AudioSource '{audioSourceName}' 已存在，返回现有实例");
            return _namedAudioSources[audioSourceName];
        }

        AudioSource audioSource = GetAudioSource();
        if (audioSource == null) return null;

        audioSource.gameObject.name = audioSourceName;
        _namedAudioSources[audioSourceName] = audioSource;
        Debug.Log($"AudioFactory: 创建AudioSource '{audioSourceName}'");
        return audioSource;
    }

    /// <summary>
    /// 获取命名AudioSource
    /// </summary>
    public AudioSource Get(string audioSourceName)
    {
        if (_namedAudioSources.ContainsKey(audioSourceName))
            return _namedAudioSources[audioSourceName];
        Debug.LogWarning($"AudioFactory: 未找到名为 '{audioSourceName}' 的AudioSource");
        return null;
    }

    /// <summary>
    /// 销毁命名AudioSource
    /// </summary>
    public void Destroy(string audioSourceName)
    {
        if (_namedAudioSources.ContainsKey(audioSourceName))
        {
            AudioSource audioSource = _namedAudioSources[audioSourceName];
            DestroyAudioSource(audioSource);
            _namedAudioSources.Remove(audioSourceName);
            Debug.Log($"AudioFactory: 销毁AudioSource '{audioSourceName}'");
        }
        else
        {
            Debug.LogWarning($"AudioFactory: 未找到名为 '{audioSourceName}' 的AudioSource");
        }
    }

    /// <summary>
    /// 检查命名音源是否存在
    /// </summary>
    public bool Exists(string audioSourceName)
    {
        return _namedAudioSources.ContainsKey(audioSourceName);
    }

    /// <summary>
    /// 获取所有命名音源名字列表
    /// </summary>
    public List<string> GetNamedAudioSourceNames()
    {
        return new List<string>(_namedAudioSources.Keys);
    }

    /// <summary>
    /// 从对象池获取AudioSource
    /// </summary>
    public AudioSource GetAudioSource()
    {
        if (_coroutineRunner == null)
        {
            Debug.LogError("AudioFactory 未初始化，请先调用 Initialize 方法");
            return null;
        }
        AudioSource audioSource;
        if (_audioSourcePool.Count > 0)
        {
            audioSource = _audioSourcePool.Dequeue();
        }
        else if (_activeAudioSources.Count + _audioSourcePool.Count < _maxPoolSize)
        {
            audioSource = CreateNewAudioSource();
        }
        else
        {
            Debug.LogWarning("AudioFactory: AudioSource池已满，无法获取可用音源！");
            return null;
        }
        _activeAudioSources.Add(audioSource);
        audioSource.gameObject.SetActive(true);
        audioSource.mute = _isMuted;
        return audioSource;
    }

    /// <summary>
    /// 播放音效（返回AudioSource，可用于控制）
    /// </summary>
    public AudioSource PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioFactory: 尝试播放空的音频片段");
            return null;
        }
        AudioSource audioSource = GetAudioSource();
        if (audioSource == null) return null;

        audioSource.clip = clip;
        audioSource.volume = Mathf.Clamp01(volume) * _masterVolume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.mute = _isMuted;
        audioSource.Play();

        if (!loop)
        {
            _coroutineRunner.StartCoroutine(AutoRecycleAudioSource(audioSource, clip.length));
        }

        return audioSource;
    }

    /// <summary>
    /// 播放音效（通过资源路径）
    /// </summary>
    public AudioSource PlaySound(string resourcePath, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);
        if (clip == null)
        {
            Debug.LogError($"AudioFactory: 无法加载音频文件: {resourcePath}");
            return null;
        }
        return PlaySound(clip, volume, pitch, loop);
    }

    /// <summary>
    /// 播放OneShot音效（不占用AudioSource）
    /// </summary>
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioFactory: 尝试播放空的音频片段");
            return;
        }
        AudioSource audioSource = GetAudioSource();
        if (audioSource == null) return;
        audioSource.mute = _isMuted;
        audioSource.PlayOneShot(clip, Mathf.Clamp01(volume) * _masterVolume);
        _coroutineRunner.StartCoroutine(AutoRecycleAudioSource(audioSource, clip.length));
    }

    /// <summary>
    /// 播放OneShot音效（通过资源路径）
    /// </summary>
    public void PlayOneShot(string resourcePath, float volume = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>(resourcePath);
        if (clip == null)
        {
            Debug.LogError($"AudioFactory: 无法加载音频文件: {resourcePath}");
            return;
        }
        PlayOneShot(clip, volume);
    }

    /// <summary>
    /// 停止指定AudioSource
    /// </summary>
    public void StopAudioSource(AudioSource audioSource)
    {
        if (audioSource == null) return;
        audioSource.Stop();
        RecycleAudioSource(audioSource);
    }

    /// <summary>
    /// 暂停指定AudioSource
    /// </summary>
    public void PauseAudioSource(AudioSource audioSource)
    {
        if (audioSource == null) return;
        audioSource.Pause();
    }

    /// <summary>
    /// 恢复指定AudioSource
    /// </summary>
    public void ResumeAudioSource(AudioSource audioSource)
    {
        if (audioSource == null) return;
        audioSource.UnPause();
    }

    /// <summary>
    /// 销毁指定AudioSource
    /// </summary>
    public void DestroyAudioSource(AudioSource audioSource)
    {
        if (audioSource == null) return;
        _activeAudioSources.Remove(audioSource);

        string keyToRemove = null;
        foreach (var kvp in _namedAudioSources)
        {
            if (kvp.Value == audioSource)
            {
                keyToRemove = kvp.Key;
                break;
            }
        }
        if (keyToRemove != null)
        {
            _namedAudioSources.Remove(keyToRemove);
        }
        if (audioSource.gameObject != null)
        {
            Object.Destroy(audioSource.gameObject);
        }
    }

    /// <summary>
    /// 停止所有活跃的音频源
    /// </summary>
    public void StopAllAudioSources()
    {
        for (int i = _activeAudioSources.Count - 1; i >= 0; i--)
        {
            if (_activeAudioSources[i] != null)
            {
                _activeAudioSources[i].Stop();
                RecycleAudioSource(_activeAudioSources[i]);
            }
        }
    }

    /// <summary>
    /// 暂停所有活跃的音频源
    /// </summary>
    public void PauseAllAudioSources()
    {
        foreach (var audioSource in _activeAudioSources)
        {
            if (audioSource != null)
                audioSource.Pause();
        }
    }

    /// <summary>
    /// 恢复所有活跃的音频源
    /// </summary>
    public void ResumeAllAudioSources()
    {
        foreach (var audioSource in _activeAudioSources)
        {
            if (audioSource != null)
                audioSource.UnPause();
        }
    }

    /// <summary>
    /// 自动回收AudioSource
    /// </summary>
    private IEnumerator AutoRecycleAudioSource(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null && !audioSource.loop)
        {
            RecycleAudioSource(audioSource);
        }
    }

    /// <summary>
    /// 回收AudioSource到对象池
    /// </summary>
    private void RecycleAudioSource(AudioSource audioSource)
    {
        if (audioSource == null) return;
        _activeAudioSources.Remove(audioSource);

        string keyToRemove = null;
        foreach (var kvp in _namedAudioSources)
        {
            if (kvp.Value == audioSource)
            {
                keyToRemove = kvp.Key;
                break;
            }
        }
        if (keyToRemove != null)
        {
            _namedAudioSources.Remove(keyToRemove);
        }

        audioSource.Stop();
        audioSource.clip = null;
        audioSource.loop = false;
        audioSource.volume = _masterVolume;
        audioSource.pitch = 1f;
        audioSource.mute = _isMuted;
        audioSource.gameObject.SetActive(false);

        if (_audioSourcePool.Count < _maxPoolSize)
        {
            _audioSourcePool.Enqueue(audioSource);
        }
        else
        {
            Object.Destroy(audioSource.gameObject);
        }
    }

    /// <summary>
    /// 获取活跃的音频源数量
    /// </summary>
    public int GetActiveAudioSourceCount() => _activeAudioSources.Count;

    /// <summary>
    /// 获取池中可用的音频源数量
    /// </summary>
    public int GetPooledAudioSourceCount() => _audioSourcePool.Count;

    /// <summary>
    /// 获取命名音源数量
    /// </summary>
    public int GetNamedAudioSourceCount() => _namedAudioSources.Count;

    /// <summary>
    /// 清理所有音频源（用于场景切换等）
    /// </summary>
    public void ClearAllAudioSources()
    {
        // 停止并回收所有活跃的音频源
        for (int i = _activeAudioSources.Count - 1; i >= 0; i--)
        {
            if (_activeAudioSources[i] != null)
            {
                _activeAudioSources[i].Stop();
                RecycleAudioSource(_activeAudioSources[i]);
            }
        }

        // 清空对象池
        while (_audioSourcePool.Count > 0)
        {
            AudioSource audioSource = _audioSourcePool.Dequeue();
            if (audioSource != null)
            {
                Object.Destroy(audioSource.gameObject);
            }
        }

        _namedAudioSources.Clear();
    }

    /// <summary>
    /// 销毁音频工厂
    /// </summary>
    public void Destroy()
    {
        ClearAllAudioSources();
        if (_audioSourceParent != null)
        {
            Object.Destroy(_audioSourceParent.gameObject);
        }
        _instance = null;
    }

    /// <summary>
    /// 全局静音
    /// </summary>
    public void SetMute(bool mute)
    {
        _isMuted = mute;
        foreach (var src in _activeAudioSources)
        {
            if (src != null) src.mute = mute;
        }
        foreach (var src in _audioSourcePool)
        {
            if (src != null) src.mute = mute;
        }
    }
    public bool IsMuted() => _isMuted;

    /// <summary>
    /// 设置全局音量（0-1）
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        // 仅影响新播放的音效。若需要立即同步所有音源，可解开注释：
        // ApplyMasterVolume();
    }

    /// <summary>
    /// 获取全局音量
    /// </summary>
    public float GetMasterVolume() => _masterVolume;

    /// <summary>
    /// 可立即同步全部活跃音源音量（可选，通常建议只影响新播放音效）
    /// </summary>
    public void ApplyMasterVolume()
    {
        foreach (var src in _activeAudioSources)
        {
            if (src != null)
            {
                src.volume = _masterVolume;
            }
        }
        foreach (var src in _audioSourcePool)
        {
            if (src != null)
            {
                src.volume = _masterVolume;
            }
        }
    }

    /// <summary>
    /// 淡出音效
    /// </summary>
    public void FadeOut(AudioSource audioSource, float fadeDuration = 1f, System.Action onComplete = null)
    {
        if (audioSource == null || !audioSource.isPlaying)
        {
            onComplete?.Invoke();
            return;
        }
        _coroutineRunner.StartCoroutine(FadeOutRoutine(audioSource, fadeDuration, onComplete));
    }

    private IEnumerator FadeOutRoutine(AudioSource audioSource, float fadeDuration, System.Action onComplete)
    {
        float startVolume = audioSource.volume;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (audioSource == null) yield break;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        if (audioSource != null)
        {
            audioSource.Stop();
            RecycleAudioSource(audioSource);
        }
        onComplete?.Invoke();
    }
}