using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaveSceneStruct 
{
    public int ItemID;
    public string ItemName;
    public Vector3 ItemPosition;

    SaveSceneStruct(int itemID, string itemName, Vector3 itemPosition)
    {
        ItemID = itemID;
        ItemName = itemName;
        ItemPosition = itemPosition;
    }
}
