using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShopGoodItem: MonoBehaviour
{
    ShopGoodForSaveItem _shopGoodItem;
    
    
}


[Serializable]
public enum ItemType
{
    Recovery,
    Aiming,
    Efficiency,
    Other,
    Error
}

[Serializable]
public struct ShopGoodForSaveItem
{
    public int ItemID;
    public string GoodName;
    public string GoodDescription;
    public int GoodPrice;
    public Sprite Icon;
    
    public ItemType itemType;
}






public static class EnumParser
{
    public static ItemType ParseItemType(string itemType)
    {
        Debug.LogWarning(itemType);
        
        switch (itemType)
        {
            case "恢复类":
                return ItemType.Recovery;
                break;
            case "洞察类":
                return ItemType.Aiming;
            break;
            case "效用类":
                return ItemType.Efficiency;
            break;
            case "杂物类":
                return ItemType.Other;
            break;
            default:
                return ItemType.Error;
        }
    }
}