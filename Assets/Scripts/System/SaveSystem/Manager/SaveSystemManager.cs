using System;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;


public class SaveSystemManager : cjr.Single.SingleMon<SaveSystemManager>
{


    public bool IsDebug = true;
    
    public string SaveSlotName=>gameData_SO.CurrentSaveSlotName;

    [SerializeField] Dictionary<string,Dictionary<string,List<SaveItemStruct>>>SaveDatas = new Dictionary<string, Dictionary<string,List<SaveItemStruct>>>();

    public const string InventoryItemDic = "InventoryItemDic";

    #region ????

    
        [SerializeField] InventoryItemInWorld[] items;
        [SerializeField] List<SaveItemStruct> saved;

    #endregion



    public int GameState_Ondebug = 0;
    
    public void SaveGameState()
    {
        if(IsDebug)
            return;
        
        ES3.Save(SaveSlotName+"nowGameState",GameState.Instance.CurrentStateSlot.GetState());
    }

    public int LoadGameState()
    {
        if(IsDebug|| !ES3.KeyExists(SaveSlotName+"nowGameState"))
            return GameState_Ondebug;
        return (ES3.Load<int>(SaveSlotName+"nowGameState"));
    }
    

    [SerializeField] private CanvasGroup SavingMask;
    
    public  IReadOnlyList<SaveItemStruct> GetCurrentLists()
    {
        return   SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()];
    }

    
    [Button("SaveSceneItem")]
    public void SaveSceneItem()
    {
        
        ClearCurrentSceneData();
        
        items = FindObjectsOfType<InventoryItemInWorld>();
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            SaveItemStruct saveItemStruct = new SaveItemStruct(item.ID,item.Name,item.transform.position,item.NumType);
            AddItem(saveItemStruct);
        }
        ES3.Save(InventoryItemDic,SaveDatas);
    }

    [Button("LoadSceneItem")]
    public void LoadSceneItem()
    {
        items = FindObjectsOfType<InventoryItemInWorld>();
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
                    //
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

    public void AddItem(SaveItemStruct item)
    {
        if (!SaveDatas.ContainsKey(SaveSlotName))
        {
            SaveDatas[SaveSlotName] = new Dictionary<string, List<SaveItemStruct>>();
        }

        if (!SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()] = new List<SaveItemStruct>();
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
    
    #region ????

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
       
        // SaveSlotName = gameData_SO.CurrentSaveSlotName;
        // if (SaveSlotName == "")
        // {
        //     SaveSlotName = gameData_SO.SaveSlotNames[0];
        // }

        LoadGame();
        if (ES3.KeyExists(InventoryItemDic))
        {
            // var mid=new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>();
           SaveDatas = (Dictionary<string,Dictionary<string,List<SaveItemStruct>>>)ES3.Load(InventoryItemDic);
            // SaveDatas =new Dictionary<string, Dictionary<string,List<SaveSceneStruct>>>(mid);
        }
        
        if (!SaveDatas.ContainsKey(SaveSlotName))
        {
            SaveDatas[SaveSlotName] = new Dictionary<string, List<SaveItemStruct>>();
        }

        if (!SaveDatas[SaveSlotName].ContainsKey(cjr.Scence.SceneManager.Instance.GetCurrentScene()))
        {
            SaveDatas[SaveSlotName][cjr.Scence.SceneManager.Instance.GetCurrentScene()] = new List<SaveItemStruct>();
        }
    }

    List<GameObject> rootObjects = new List<GameObject>();



    public void SaveObject(string key, object entity)
    {
        
        if(IsDebug)
            return;
        // 任意 JSON 库都行，这里用 Newtonsoft.Json 举例
        string json = JsonConvert.SerializeObject(entity);
        var deepCopy = JsonConvert.DeserializeObject(json);
        ES3.Save(SaveSlotName + key, deepCopy, ES3Settings.defaultSettings);
    } 
    

    public T LoadObject<T>(string key)
    {
        if (IsDebug)
            return default(T);
        
        
        if (!ES3.KeyExists(SaveSlotName + key))
            return default(T);

        // ES3.Load 会 new 一份全新的对象，不会跟任何旧对象共享引用
        return ES3.Load<T>(SaveSlotName + key);
    }


    public bool IsContainKey(string key)
    {
        return ES3.KeyExists(SaveSlotName + key);
    }
    
    public void SaveGame()
    {
        StartCoroutine(SaveGameCoroutine());
    }

    IEnumerator SaveGameCoroutine()
    {
        if(IsDebug)
            yield break;
        foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        { 
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is RequireSavingItem savingItem)
                {
                    savingItem.Save();
                    yield return null;
                }
            }
        }
        SaveSceneItem();
    }
    
    
    //todo
    /// <summary>
    /// ?????????
    /// </summary>
    /// ???????????????????
    public void LoadGame()
    {
      StartCoroutine(LoadGameCoroutine());
    }


    IEnumerator LoadGameCoroutine()
    {
        if(IsDebug)
            yield break;
        SavingMask.alpha = 1;
        {
            foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (!go)
                {
                    yield return null;
                    continue;
                }
                Component[] components = go.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] is RequireSavingItem savingItem)
                    {
                        savingItem.Load();
                        yield return null;
                    }
                }
            }
        }
        LoadSceneItem();
        // PlayerSaving.Instance.Load();
        // yield return null;
        // PlayerInventory.Instance.Load();
        // yield return null;
        // PlayerHpSystem.Instance.Load();
        // yield return null;
        SavingMask.DOFade(0, 0.2f);
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