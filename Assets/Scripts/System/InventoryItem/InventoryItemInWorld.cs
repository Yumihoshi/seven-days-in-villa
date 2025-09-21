using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UseType
{
    Single,
    Additive,
}


public enum NumType
{
    Single,
    Multiple,
}

public class InventoryItemInWorld : InteractableItem
{ 
    public int ID;
    public string Name;
    public UseType Type;
    public NumType NumType;
    public int Amount;

    public override void Interact()
    {
        base.Interact();
        PlayerInventory.Instance.AddItem(this);
    }
}
