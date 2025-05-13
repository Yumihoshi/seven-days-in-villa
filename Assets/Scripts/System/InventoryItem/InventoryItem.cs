using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UseType
{
    Single,
    Additive,
}

public class InventoryItem : MonoBehaviour
{ 
    public int ID;
    public string Name;
    public UseType Type;
}
