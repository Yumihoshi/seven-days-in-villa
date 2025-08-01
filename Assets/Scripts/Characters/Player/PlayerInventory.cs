using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInventory : cjr.Single.SingleMon<PlayerInventory>,RequireSavingItem
{
    [SerializeField] List<SaveSceneStruct> playerInventory;
    public const string playerInventoryName = "PlayerInventory";
    public void Save()
    {
        
        ES3.Save(SaveSystemManager.Instance.SaveSlotName+playerInventoryName, playerInventory);
    }


    public void Load()
    {
        if (ES3.KeyExists(SaveSystemManager.Instance.SaveSlotName+playerInventoryName))
        {
            playerInventory = ES3.Load<List<SaveSceneStruct>>(SaveSystemManager.Instance.SaveSlotName+playerInventoryName);
        }
        else
        {
            playerInventory = new List<SaveSceneStruct>();
            playerInventory.Clear();
        }
    }

    public bool checkInventory(InventoryItem item)
    {
        foreach (var it in playerInventory)
        {
            if(it.ItemID == item.ID)
                return true;
        }
        return false;
    }

    public void AddItem(InventoryItem item)
    {
        if (!checkInventory(item))
        {
            SaveSceneStruct saveScene = new SaveSceneStruct(item.ID,item.Name,item.transform.position,item.NumType);
            playerInventory.Add(saveScene);
        }
        else
        {
            
        }
        
        Destroy(item.gameObject);
       
    }
    
}
