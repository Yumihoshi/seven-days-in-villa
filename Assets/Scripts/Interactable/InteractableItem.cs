using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class InteractableItem : MonoBehaviour
{
    private Collider2D _collider2D;
    protected virtual void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAction.Instance.SetInteract(this);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        PlayerAction.Instance.SetInteract(this);
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        PlayerAction.Instance.SetInteract(null);
    }
    public virtual void Interact()
    {
      
    } 
}
