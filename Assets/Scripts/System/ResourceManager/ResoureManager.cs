using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResoureManager 
{
    /// <summary>
    /// 从Resources文件夹加载并实例化预制体
    /// </summary>
    /// <param name="resourcePath">Resources文件夹下的相对路径（不包含Resources/前缀）</param>
    /// <returns>实例化的GameObject，如果加载失败返回null</returns>
    public static GameObject LoadGameobject(string resourcePath)
    {
        try
        {
            // 从Resources文件夹加载预制体
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
        
            if (prefab == null)
            {
                Debug.LogError($"无法从Resources文件夹加载预制体: {resourcePath}");
                return null;
            }
        
            // 实例化预制体
            GameObject instance = Object.Instantiate(prefab);
        
            Debug.Log($"成功实例化预制体: {resourcePath}");
            return instance;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"实例化预制体时出错: {e.Message}");
            return null;
        }
    }

    public static GameObject LoadGameobject(string resourcePath, Transform parent)
    {
        try
        {
            // 从Resources文件夹加载预制体
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
        
            if (prefab == null)
            {
                Debug.LogError($"无法从Resources文件夹加载预制体: {resourcePath}");
                return null;
            }
        
            // 实例化预制体
            GameObject instance = Object.Instantiate(prefab, parent);
            
            return instance;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"实例化预制体时出错: {e.Message}");
            return null;
        }
    }
}
