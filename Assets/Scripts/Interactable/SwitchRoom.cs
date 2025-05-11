using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoom : InteractableItem
{
    [SerializeField] private PolygonCollider2D NextConfiner;
    [SerializeField] Transform NextPoisition;
    public override void Interact()
    {
        base.Interact();
    }
}
