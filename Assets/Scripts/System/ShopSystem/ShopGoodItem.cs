using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShopGoodItem: MonoBehaviour
{
    ShopGoodForSaveItem _shopGoodItem;
    
    
}

[Serializable]
public struct ShopGoodForSaveItem
{
    public int ItemID;
    public string GoodName;
    public string GoodDescription;
    public int GoodPrice;
    public Sprite Icon;
}