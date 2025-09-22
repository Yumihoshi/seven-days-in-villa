using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;

public class ResourceLoader : cjr.Single.Singleton<ResourceLoader>
{
   
    public ResourceLoader()
    {
       
    }
    
    
    
    public void LoadMp3Clip(string localPath, System.Action<AudioClip> onLoaded)
    {
        // 判断是不是Resources路径
        if (localPath.Replace("\\", "/").StartsWith("Assets/Resources/"))
        {
            // 转为Resources.Load的相对路径（去掉Assets/Resources/和扩展名）
            string resourcePath = localPath.Replace("\\", "/");
            resourcePath = resourcePath.Substring("Assets/Resources/".Length);
            int dot = resourcePath.LastIndexOf('.');
            if (dot != -1) resourcePath = resourcePath.Substring(0, dot);

            AudioClip clip = Resources.Load<AudioClip>(resourcePath);
            if (clip == null)
            {
                Debug.LogError("Resources.Load 加载失败: " + resourcePath);
            }

            onLoaded?.Invoke(clip);
            return;
        }

        // 否则用协程加载本地文件
        CoroutineFactory.Instance.RunCoroutine(LoadMp3ClipCoroutine(localPath, onLoaded));
    }

    private IEnumerator LoadMp3ClipCoroutine(string localPath, System.Action<AudioClip> onLoaded)
    {
        string path = localPath.Replace("\\", "/");
        if (!path.StartsWith("file://"))
        {
            // 绝对路径加file:///
            if (System.IO.Path.IsPathRooted(path))
            {
                if (!path.StartsWith("/")) path = "/" + path; // 兼容windows
                path = "file://" + path;
            }
            else
            {
                // 相对路径转绝对路径
                path = "file:///" + System.IO.Path.GetFullPath(path).Replace("\\", "/");
            }
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
#else
        if (www.isNetworkError || www.isHttpError)
#endif
            {
                Debug.LogError(path);
                Debug.LogError("加载MP3失败: " + www.error);
                onLoaded?.Invoke(null);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                onLoaded?.Invoke(clip);
            }
        }
    }

    /// <summary>
    /// 传入 Resources 下相对路径，不带扩展名，如 "Config/MyData"
    /// </summary>
    public T LoadSO<T>(string resourcesPath) where T : ScriptableObject
    {
        T so = Resources.Load<T>(resourcesPath);
        if (so == null) Debug.LogError($"Resources.Load 找不到 SO: {resourcesPath}");
        return so;
    }


    public Sprite LoadSprite(string spritePath)
    {
        return Resources.Load<Sprite>(spritePath);
    }
    
    /// <summary>
    /// 已经创建新的了
    /// </summary>
    /// <param name="ObjectPath"></param>
    /// <returns></returns>
    public GameObject LoadObject(string ObjectPath)
    {
        
        return GameObjectFactory.Instance.Create(Resources.Load<GameObject>(ObjectPath));
    }
    
    public GameObject LoadObject(string ObjectPath,Transform parent)
    {
        
        return GameObject.Instantiate(Resources.Load<GameObject>(ObjectPath),parent);
    }
}


