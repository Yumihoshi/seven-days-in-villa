using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShopGoodItem: MonoBehaviour
{
    ShopGoodItem _shopGoodItem;
    
    
}

[Serializable]
public struct ShopGoodForSaveItem
{
    public string GoodName;
    public string GoodDescription;
    public string GoodPrice;
    public string GoodSpitePath;
}