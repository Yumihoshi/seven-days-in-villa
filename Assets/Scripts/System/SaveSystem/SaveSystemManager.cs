using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{


    [SerializeField] private bool IsDebug = true;
    
    
    public string SaveSlotName;
    static SaveSystemManager instance;

    [SerializeField] GameData_So gameData_SO;
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
        SaveSlotName = gameData_SO.CurrentSaveSlotName;
        if (SaveSlotName == "")
        {
            SaveSlotName = gameData_SO.SaveSlotNames[0];
        }
    }

    public List<GameObject> rootObjects = new List<GameObject>();


    
    /// <summary>
    /// 保存当前
    /// </summary>
    public void SaveGame()
    {
        if(IsDebug)
            return;
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
        if(IsDebug)
            return;
        
        string currentSceneName =cjr.Scence.SceneManager.Instance.GetCurrentScene();
        string BaseKey=SaveSlotName+currentSceneName;
    }
}

public enum SlotName
{
    Default,
}