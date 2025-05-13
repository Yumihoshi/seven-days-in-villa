using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
/*
 *保存可以消失或者获取的物体呢
 *我的想法是先将所有的物体都转为预制体
 *放入data_so中
 *这样我就只需要保存物体的id，位置就行
 *
 * 
 */



public class SaveSystemManager : MonoBehaviour
{


    [SerializeField] private bool IsDebug = true;
    
    [Header("每次游戏选择的存档名称")]
    public string SaveSlotName;

    [SerializeField] Dictionary<string,Dictionary<string,List<SaveSceneStruct>>>SaveDatas = new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>();

    public const string InventoryItemDic = "InventoryItemDic";

 
    [SerializeField] InventoryItem[] items;
    [SerializeField] List<SaveSceneStruct> saved;
    public  IReadOnlyList<SaveSceneStruct> GetCurrentLists()
    {
        return   SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
    }

    
    [Button("SaveSceneItem")]
    public void SaveSceneItem()
    {
        items = FindObjectsOfType<InventoryItem>();
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            SaveSceneStruct saveSceneStruct = new SaveSceneStruct(item.ID,item.Name,item.transform.position,item.NumType);
            AddItem(saveSceneStruct);
        }
        ES3.Save(InventoryItemDic,SaveDatas);
    }

    [Button("LoadSceneItem")]
    public void LoadSceneItem()
    {
        items = FindObjectsOfType<InventoryItem>();
        if(!ES3.KeyExists(InventoryItemDic))
            return;
        ES3.Load(InventoryItemDic);
        saved=SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
        for (int i = 0; i < saved.Count; i++)
        {
            int id = saved[i].ItemID;
            Debug.LogWarning(id);
            bool exi=false;
            for (int ii = 0; ii < items.Length; ii++)
            {
                if (items[ii].ID == id)
                {
                    items[ii].transform.position = saved[i].ItemPosition;
                    exi=true;
                    break;
                }
            }
            if (!exi)
            {
                int offset = id - 1000;
                var Games=Instantiate(gameData_SO.InventoryItems[offset],saved[i].ItemPosition,Quaternion.identity);
                Games.Name=saved[i].ItemName;
                //todo
                //具体一些变量的赋值
                Games.transform.position = saved[i].ItemPosition;
            }
        }
    }

    public void RemoveItems(int ID)
    {
        var saveData = SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
        for (int i = 0; i < saveData.Count; i++)
        {
            if (saveData[i].ItemID == ID)
            {
                saveData.RemoveAt(i);
                return;
            }
        }
        Debug.LogWarning("Can not remove item");
    }

    public void AddItem(SaveSceneStruct item)
    {
        var saveData = SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
        if (saveData.Contains(item))
        {
            if(item.ItemType==NumType.Single)
                return;
            saveData.Remove(item);
            item.ItemAmount++;
            saveData.Add(item);
        }
        else
        {
            saveData.Add(item);
        }
    }
    
    #region 单例

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
    

    #endregion

    [SerializeField] GameData_So gameData_SO;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SaveSlotName = gameData_SO.CurrentSaveSlotName;
        if (SaveSlotName == "")
        {
            SaveSlotName = gameData_SO.SaveSlotNames[0];
        }

        if (ES3.KeyExists(InventoryItemDic))
        {
            // var mid=new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>();
           SaveDatas = (Dictionary<string,Dictionary<string,List<SaveSceneStruct>>>)ES3.Load(InventoryItemDic);
            // SaveDatas =new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>(mid);
        }
        
        if (!SaveDatas.ContainsKey(SaveSlotName))
        {
            SaveDatas[SaveSlotName] = new Dictionary<string, List<SaveSceneStruct>>();
        }

        if (!SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()] = new List<SaveSceneStruct>();
        }
    }

    List<GameObject> rootObjects = new List<GameObject>();


    
    /// <summary>
    /// 保存当前场景的数据
    /// 而具体的键值需要具体的类进行实现
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
    //todo
    /// <summary>
    /// 加载下一个
    /// </summary>
    public void LoadGame()
    {
        if(IsDebug)
            return;
        {
            string currentSceneName =cjr.Scence.SceneManager.Instance.GetCurrentScene();
            string BaseKey=SaveSlotName+currentSceneName;
        }
    }
    
}

public enum SlotName
{
    Default,
}