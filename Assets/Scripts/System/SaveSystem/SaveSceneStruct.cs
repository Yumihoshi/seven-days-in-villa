using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct SaveSceneStruct 
{
    public int ItemID;
    public string ItemName;
    public Vector3 ItemPosition;
    public NumType ItemType;
    public int ItemAmount;

    public SaveSceneStruct(int itemID, string itemName, Vector3 itemPosition, NumType itemType)
    {
        ItemAmount = 1;
        ItemID = itemID;
        ItemName = itemName;
        ItemPosition = itemPosition;
        ItemType = itemType;
    }
    public static bool operator ==(SaveSceneStruct a, SaveSceneStruct b) => a.Equals(b);
    public static bool operator !=(SaveSceneStruct a, SaveSceneStruct b) => !(a == b);

    public override bool Equals(object obj) => obj is SaveSceneStruct other && Equals(other);

    public bool Equals(SaveSceneStruct other)
    {
        return ItemID == other.ItemID;
    }
    

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + ItemID;
            hash = hash * 23 + (ItemName?.GetHashCode() ?? 0);
            hash = hash * 23 + ItemPosition.GetHashCode();
            hash = hash * 23 + ItemType.GetHashCode();
            return hash;
        }
    }
}
