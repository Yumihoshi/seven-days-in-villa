using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class WalkedRoomCheck : MonoBehaviour
{
    Collider2D collider2D;
    
    [SerializeField] bool hasInit = false;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!hasInit&&other.CompareTag("Player"))
        {
            GameState.Instance.CurrentStateSlot?.Step();
            hasInit = true;
        }
    }
    
}
