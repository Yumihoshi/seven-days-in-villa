using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerInventory : cjr.Single.SingleMon<PlayerInventory>,RequireSavingItem
{
    
    
    
    
    [SerializeField] List<SaveItemStruct> playerInventory;
    public const string playerInventoryName = "PlayerInventory";
    public void Save()
    {
        
        ES3.Save(SaveSystemManager.Instance.SaveSlotName+playerInventoryName, playerInventory);
    }


    public void Load()
    {
        if (ES3.KeyExists(SaveSystemManager.Instance.SaveSlotName+playerInventoryName))
        {
            playerInventory = ES3.Load<List<SaveItemStruct>>(SaveSystemManager.Instance.SaveSlotName+playerInventoryName);
        }
        else
        {
            playerInventory = new List<SaveItemStruct>();
            playerInventory.Clear();
        }
    }

    public bool checkInventory(InventoryItemInWorld itemInWorld)
    {
        foreach (var it in playerInventory)
        {
            if(it.ItemID == itemInWorld.ID)
                return true;
        }
        return false;
    }

    public void AddItem(InventoryItemInWorld itemInWorld)
    {
        if (!checkInventory(itemInWorld))
        {
            SaveItemStruct saveItem = new SaveItemStruct(itemInWorld.ID,itemInWorld.Name,
                itemInWorld.transform.position,itemInWorld.NumType);
            playerInventory.Add(saveItem);
        }
       
        
        Destroy(itemInWorld.gameObject);
       
    }

    public bool checkInventory(ShopGoodForSaveItem item)
    {
        foreach (var single in playerInventory)
        {
            if(single.ItemID == item.ItemID)
                return true;
        }
        return false;
    }
    
    public void AddItem(ShopGoodForSaveItem item)
    {
        if (!checkInventory(item))
        {
            SaveItemStruct saveItem= new SaveItemStruct(item.ItemID, item.GoodName, Vector3.zero);
            playerInventory.Add(saveItem);
        }
    }
    
}
