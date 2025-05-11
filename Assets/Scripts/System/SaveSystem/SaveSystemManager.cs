using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public string SaveSlotName;
    static SaveSystemManager instance;

    public static SaveSystemManager Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType<SaveSystemManager>();
            return instance;
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SaveSlotName = nameof(SlotName.Default);
    }

    public List<GameObject> rootObjects = new List<GameObject>();


    
    /// <summary>
    /// 保存当前
    /// </summary>
    public void SaveGame()
    {
        foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
          Component[] components = go.GetComponents<Component>();
          for (int i = 0; i < components.Length; i++)
          {
              if (components[i] is RequireSavingItem savingItem)
              {
                  savingItem.Save();
              }
          }
        }
    }
    
    /// <summary>
    /// 加载下一个
    /// </summary>
    public void LoadGame()
    {
        string currentSceneName =cjr.Scence.SceneManager.Instance.GetCurrentScene();
        string BaseKey=SaveSlotName+currentSceneName;
    }
}

public enum SlotName
{
    Default,
}