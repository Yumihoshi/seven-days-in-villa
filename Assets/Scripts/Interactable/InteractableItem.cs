using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    protected virtual void Awake()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAction.Instance.SetInteract(this);
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        PlayerAction.Instance.SetInteract(null);
    }
    public virtual void Interact()
    {
        Debug.Log("Interact");
    } 
}
