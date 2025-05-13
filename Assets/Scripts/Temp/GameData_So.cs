using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/GameData_So")]
public class GameData_So : ScriptableObject
{
    public List<string> SaveSlotNames;
    public string CurrentSaveSlotName;

    public List<InventoryItem>InventoryItems;
    
    private void Awake()
    {
        SaveSlotNames.Add(nameof(SlotName.Default));
    }
}
