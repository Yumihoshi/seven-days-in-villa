using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class InteractableItem : MonoBehaviour
{
    private Collider2D _collider2D;
    [SerializeField] protected bool NeedTrigger = true;
    protected virtual void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.isTrigger = NeedTrigger;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            PlayerAction.Instance.SetInteract(this);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player")) 
            PlayerAction.Instance.SetInteract(this);
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            PlayerAction.Instance.SetInteract(null);
            UiGameobject.Instance.SetInteractbleInfoClose();
        }
    }
    public virtual void Interact()
    {
      
    }


    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerAction.Instance.SetInteract(this);
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerAction.Instance.SetInteract(this);
    }


    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            PlayerAction.Instance.SetInteract(null);
            UiGameobject.Instance.SetInteractbleInfoClose();
        }
    }
}
