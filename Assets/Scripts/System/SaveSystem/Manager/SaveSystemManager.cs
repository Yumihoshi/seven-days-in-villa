using System;
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



public class SaveSystemManager : cjr.Single.Singleton<SaveSystemManager>
{


    public bool IsDebug = true;
    
    public string SaveSlotName=>gameData_SO.CurrentSaveSlotName;

    [SerializeField] Dictionary<string,Dictionary<string,List<SaveSceneStruct>>>SaveDatas = new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>();

    public const string InventoryItemDic = "InventoryItemDic";

    #region 调试

    
        [SerializeField] InventoryItem[] items;
        [SerializeField] List<SaveSceneStruct> saved;

    #endregion
    
    public  IReadOnlyList<SaveSceneStruct> GetCurrentLists()
    {
        return   SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
    }

    
    [Button("SaveSceneItem")]
    public void SaveSceneItem()
    {
        
        ClearCurrentSceneData();
        
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
        SaveDatas = ES3.Load(InventoryItemDic,SaveDatas);
           
        if (SaveDatas.ContainsKey(SaveSlotName) &&
            SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            saved=SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
            for (int i = 0; i < saved.Count; i++)
            {
                int id = saved[i].ItemID;
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

            for (int i = 0; i < items.Length; i++)
            {
                bool need=true;
                for (int ii = 0; ii < saved.Count; ii++)
                {
                    if (saved[ii].ItemID == items[i].ID)
                    {
                        need=false;
                        break;
                    }
                }

                if (need)
                {
                    Destroy(items[i].gameObject);
                }
            }   
        }
    }

    public void ClearData()
    {
        ES3.DeleteFile();
    }

    public void ClearCurrentSceneData()
    {
        // ES3.DeleteFile();
        if (SaveDatas.ContainsKey(SaveSlotName) &&
            SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            
            SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()].Clear();
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
        if (!SaveDatas.ContainsKey(SaveSlotName))
        {
            SaveDatas[SaveSlotName] = new Dictionary<string, List<SaveSceneStruct>>();
        }

        if (!SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()] = new List<SaveSceneStruct>();
        }
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

    public GameData_So gameData_SO;
    // private void Awake()
    protected override void Awake()
    {
        base.Awake();
        if(IsDebug)
           ClearData();
        DontDestroyOnLoad(gameObject);
        // SaveSlotName = gameData_SO.CurrentSaveSlotName;
        // if (SaveSlotName == "")
        // {
        //     SaveSlotName = gameData_SO.SaveSlotNames[0];
        // }

        LoadGame();
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
        SaveSceneItem();
        PlayerSaving.Instance.Save();
        PlayerInventory.Instance.Save();
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
            foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            { 
                Component[] components = go.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] is RequireSavingItem savingItem)
                    {
                        savingItem.Load();
                    }
                }
            }
        }
        LoadSceneItem();
        PlayerSaving.Instance.Load();
        PlayerInventory.Instance.Load();
    }
    

    private void OnApplicationQuit()
    {
        SaveGame();
        gameData_SO.CurrentSaveSlotName = SlotName.Default.ToString();
    }
}

public enum SlotName
{
    Default,
    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
}