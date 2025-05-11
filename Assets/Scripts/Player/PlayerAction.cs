using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb;
   [Header("Movement")]
   [SerializeField] private float speed = 0.5f;
   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
   }



   public void Move(InputAction.CallbackContext context)
   {
      Vector2 movement = context.ReadValue<Vector2>();
      if (context.performed)
      {
         movement *= speed*Time.deltaTime;
         rb.MovePosition(rb.position+movement);
      }
   }
   
}
