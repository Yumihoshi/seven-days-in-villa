using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    private Collider2D _collider2D;
    [SerializeField] protected bool NeedTrigger = true;

    [SerializeField] private Vector3 offset;
    public Vector3 GetPosition()
    {
        return transform.position + offset;
    }

    public bool NeedHint
    {
        get
        {
            return needHint;
        }
    }
    
    
    [SerializeField] bool needHint = false;
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
        needHint = false;
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
