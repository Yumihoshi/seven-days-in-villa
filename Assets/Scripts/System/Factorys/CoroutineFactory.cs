using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协程工厂 - 统一管理所有协程
/// 单例模式，提供协程的启动、停止、暂停、恢复等功能
/// 注意：自定义 Start/Stop 方法避免与 Unity 原生方法冲突
/// </summary>
public class CoroutineFactory : cjr.Single.SingleMon<CoroutineFactory>
{
    public enum LogLevel { None, Info, Warning, Error }

    [Serializable]
    public class CoroutineInfo
    {
        public string id;
        public Coroutine coroutine;
        public IEnumerator enumerator;
        public bool isPaused;
        public bool isCompleted;
        public bool isCanceled;
        public DateTime startTime;
        public DateTime? endTime;
        public Action onComplete;
        public Action onCancel;

        public CoroutineInfo(string id, Coroutine coroutine, IEnumerator enumerator)
        {
            this.id = id;
            this.coroutine = coroutine;
            this.enumerator = enumerator;
            this.startTime = DateTime.Now;
            this.isPaused = false;
            this.isCompleted = false;
            this.isCanceled = false;
            this.endTime = null;
        }
    }

    private Dictionary<string, CoroutineInfo> _runningCoroutines = new Dictionary<string, CoroutineInfo>();
    private Dictionary<string, CoroutineInfo> _completedCoroutines = new Dictionary<string, CoroutineInfo>();
    private int _coroutineIdCounter = 0;

    [SerializeField] private LogLevel _logLevel = LogLevel.Info;

    // 全局事件
    public event Action<string> OnCoroutineStarted;
    public event Action<string> OnCoroutineCompleted;
    public event Action<string> OnCoroutineCancelled;
    public event Action<string> OnCoroutinePaused;
    public event Action<string> OnCoroutineResumed;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 启动协程并返回协程ID（自定义方法名，避免和Unity冲突）
    /// </summary>
    public string RunCoroutine(IEnumerator enumerator, Action onComplete = null, Action onCancel = null, bool freezeWait = false)
    {
        if (enumerator == null)
        {
            Log(LogLevel.Error, "CoroutineFactory: 协程枚举器不能为空");
            return null;
        }

        string coroutineId = GenerateCoroutineId();
        IEnumerator wrappedEnum = freezeWait ? FreezeWaitEnumerator(enumerator, coroutineId) : enumerator;
        Coroutine coroutine = StartCoroutine(InternalCoroutine(coroutineId, wrappedEnum, onComplete, onCancel));

        var coroutineInfo = new CoroutineInfo(coroutineId, coroutine, wrappedEnum)
        {
            onComplete = onComplete,
            onCancel = onCancel
        };

        _runningCoroutines[coroutineId] = coroutineInfo;
        coroutine = StartCoroutine(InternalCoroutine(coroutineId, wrappedEnum, onComplete, onCancel));
        coroutineInfo.coroutine = coroutine;
        Log(LogLevel.Info, $"CoroutineFactory: 启动协程 {coroutineId}");
        OnCoroutineStarted?.Invoke(coroutineId);

        return coroutineId;
    }

    /// <summary>
    /// 支持传入协程方法
    /// </summary>
    public string RunCoroutine(Func<IEnumerator> coroutineMethod, Action onComplete = null, Action onCancel = null, bool freezeWait = false)
    {
        if (coroutineMethod == null)
        {
            Log(LogLevel.Error, "CoroutineFactory: 协程方法不能为空");
            return null;
        }
        return RunCoroutine(coroutineMethod(), onComplete, onCancel, freezeWait);
    }

    /// <summary>
    /// 停止指定协程（自定义方法名，避免和Unity冲突）
    /// </summary>
    public bool HaltCoroutine(string coroutineId)
    {
        if (string.IsNullOrEmpty(coroutineId) || !_runningCoroutines.TryGetValue(coroutineId, out var coroutineInfo))
        {
            Log(LogLevel.Warning, $"CoroutineFactory: 协程 {coroutineId} 不存在或已停止");
            return false;
        }

        if (coroutineInfo.coroutine != null)
        {
            StopCoroutine(coroutineInfo.coroutine);
        }

        coroutineInfo.isCanceled = true; // 标记为取消，由内部协程处理移除和回调
        return true;
    }

    /// <summary>
    /// 暂停协程
    /// </summary>
    public bool PauseCoroutine(string coroutineId)
    {
        if (string.IsNullOrEmpty(coroutineId) || !_runningCoroutines.TryGetValue(coroutineId, out var coroutineInfo))
            return false;

        if (!coroutineInfo.isPaused && !coroutineInfo.isCompleted)
        {
            coroutineInfo.isPaused = true;
            Log(LogLevel.Info, $"CoroutineFactory: 暂停协程 {coroutineId}");
            OnCoroutinePaused?.Invoke(coroutineId);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 恢复协程
    /// </summary>
    public bool ResumeCoroutine(string coroutineId)
    {
        if (string.IsNullOrEmpty(coroutineId) || !_runningCoroutines.TryGetValue(coroutineId, out var coroutineInfo))
            return false;

        if (coroutineInfo.isPaused && !coroutineInfo.isCompleted)
        {
            coroutineInfo.isPaused = false;
            Log(LogLevel.Info, $"CoroutineFactory: 恢复协程 {coroutineId}");
            OnCoroutineResumed?.Invoke(coroutineId);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 停止所有协程
    /// </summary>
    public void HaltAllCoroutines()
    {
        var coroutineIds = new List<string>(_runningCoroutines.Keys);
        foreach (var coroutineId in coroutineIds)
        {
            HaltCoroutine(coroutineId);
        }
        Log(LogLevel.Info, "CoroutineFactory: 停止所有协程");
    }

    /// <summary>
    /// 获取协程信息
    /// </summary>
    public CoroutineInfo GetCoroutineInfo(string coroutineId)
    {
        if (_runningCoroutines.TryGetValue(coroutineId, out var info))
            return info;
        if (_completedCoroutines.TryGetValue(coroutineId, out info))
            return info;
        return null;
    }

    public List<string> GetAllRunningCoroutineIds() => new List<string>(_runningCoroutines.Keys);
    public List<string> GetAllCompletedCoroutineIds() => new List<string>(_completedCoroutines.Keys);

    public List<string> GetAllPausedCoroutineIds()
    {
        var list = new List<string>();
        foreach (var pair in _runningCoroutines)
            if (pair.Value.isPaused) list.Add(pair.Key);
        return list;
    }

    public int GetRunningCoroutineCount() => _runningCoroutines.Count;
    public int GetCompletedCoroutineCount() => _completedCoroutines.Count;

    public bool IsCoroutineRunning(string coroutineId)
    {
        return _runningCoroutines.TryGetValue(coroutineId, out var info) && !info.isCompleted && !info.isCanceled;
    }
    public bool IsCoroutinePaused(string coroutineId)
    {
        return _runningCoroutines.TryGetValue(coroutineId, out var info) && info.isPaused;
    }
    public bool IsCoroutineCompleted(string coroutineId)
    {
        return _completedCoroutines.ContainsKey(coroutineId);
    }

    /// <summary>
    /// 运行协程的内部方法
    /// </summary>
    private IEnumerator InternalCoroutine(string coroutineId, IEnumerator enumerator, Action onComplete, Action onCancel)
    {
        if (!_runningCoroutines.TryGetValue(coroutineId, out var coroutineInfo))
            yield break;

        while (true)
        {
            // 检查是否被取消
            if (coroutineInfo.isCanceled)
            {
                try { coroutineInfo.onCancel?.Invoke(); } catch (Exception ex) { Log(LogLevel.Error, $"CoroutineFactory: 取消回调异常: {ex}"); }
                coroutineInfo.isCompleted = true;
                coroutineInfo.endTime = DateTime.Now;
                _completedCoroutines[coroutineId] = coroutineInfo;
                _runningCoroutines.Remove(coroutineId);
                Log(LogLevel.Info, $"CoroutineFactory: 协程 {coroutineId} 取消并移除");
                OnCoroutineCancelled?.Invoke(coroutineId);
                yield break;
            }

            // 检查是否暂停
            while (coroutineInfo.isPaused)
                yield return null;

            bool moved;
            try
            {
                moved = enumerator.MoveNext();
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"CoroutineFactory: 协程 {coroutineId} 运行异常: {ex}");
                coroutineInfo.isCanceled = true;
                continue;
            }
            if (!moved)
                break;

            yield return enumerator.Current;
        }

        coroutineInfo.isCompleted = true;
        coroutineInfo.endTime = DateTime.Now;
        try { onComplete?.Invoke(); } catch (Exception ex) { Log(LogLevel.Error, $"CoroutineFactory: 完成回调异常: {ex}"); }
        _completedCoroutines[coroutineId] = coroutineInfo;
        _runningCoroutines.Remove(coroutineId);

        Log(LogLevel.Info, $"CoroutineFactory: 协程 {coroutineId} 完成");
        OnCoroutineCompleted?.Invoke(coroutineId);
    }

    /// <summary>
    /// 生成协程ID
    /// </summary>
    private string GenerateCoroutineId() => $"Coroutine_{_coroutineIdCounter++}_{DateTime.Now.Ticks}";

    protected override void OnDestroy()
    {
        HaltAllCoroutines();
        base.OnDestroy();
    }

    private void Log(LogLevel level, string msg)
    {
        if (level == LogLevel.None) return;
        if ((int)level >= (int)_logLevel)
        {
            switch (level)
            {
                case LogLevel.Info: Debug.Log(msg); break;
                case LogLevel.Warning: Debug.LogWarning(msg); break;
                case LogLevel.Error: Debug.LogError(msg); break;
            }
        }
    }

    /// <summary>
    /// 冻结等待的协程包装器
    /// </summary>
    private IEnumerator FreezeWaitEnumerator(IEnumerator enumerator, string coroutineId)
    {
        while (true)
        {
            object current;
            try
            {
                if (!enumerator.MoveNext()) yield break;
                current = enumerator.Current;
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, $"CoroutineFactory: 协程 {coroutineId} FreezeWait 运行异常: {ex}");
                yield break;
            }

            // 如果暂停则冻结
            if (_runningCoroutines.TryGetValue(coroutineId, out var coroutineInfo) && coroutineInfo.isPaused)
            {
                while (_runningCoroutines.TryGetValue(coroutineId, out coroutineInfo) && coroutineInfo.isPaused)
                    yield return null;
            }
            yield return current;
        }
    }
}