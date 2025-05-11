using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb;
   [Header("Movement")]
   [SerializeField] private float speed = 3f;
   [SerializeField] bool isMoving = false;
   private Vector2 movement;
   
   
   [SerializeField] Animator animator;
   
   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponentInChildren<Animator>();
   }


   private void FixedUpdate()
   {
      if (isMoving)
      {
         rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
      }
      else
      {
         rb.velocity = Vector2.zero;
      }
   }

   public void Move(InputAction.CallbackContext context)
   {
      movement = context.ReadValue<Vector2>();
      if (context.performed)
      {
         isMoving = true;
      }
      else if (context.canceled)
      {
         isMoving = false;
      }
      animator.SetBool("Ismoving", isMoving);
      movement.Normalize();
      animator.SetFloat("InputX", movement.x);
      animator.SetFloat("InputY", movement.y);
   }
   
}
