using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb;
   [Header("Movement")]
   [SerializeField] private float speed = 3f;
   [SerializeField] bool isMoving = false;
   [SerializeField] private float Speeding = 1.5f;
   public PlayerInput playerInput; 
   private Vector2 movement;
   
   
   [Header("Interact")]
   [SerializeField] PlayerInteract playerInteract;
   
   
   [SerializeField] Animator animator;
   
   static PlayerAction playerAction;

   public static PlayerAction Instance
   {
      get
      {
         if(playerAction == null)
            playerAction = FindObjectOfType<PlayerAction>();
         return playerAction;
      }
   }
   
   private void Awake()
   {
     
      rb = GetComponent<Rigidbody2D>();
      animator = GetComponentInChildren<Animator>();
      playerInteract = GetComponent<PlayerInteract>();
      playerInput = GetComponent<PlayerInput>();
   }


   private void FixedUpdate()
   {
      if (isMoving)
      {
         rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
         if (Input.GetKey(KeyCode.LeftShift))
         {
            rb.velocity *= Speeding;
            animator.speed *= Speeding;
         }
      }
      else
      {
         rb.velocity = Vector2.zero;
      }

      if (!Input.GetKeyDown(KeyCode.LeftShift))
      {
         animator.speed = 1;
      }
   }

   #region PlayerAt
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

   public void SetInteract(InteractableItem item)
   {
      playerInteract.SetInteract(item);
   }

   public void Interact(InputAction.CallbackContext context)
   {
      if (context.performed)
      {
         playerInteract.Interact();
      }
   }
   

   #endregion


   #region Dialogue

   public void DialogueMove(InputAction.CallbackContext context)
   {
      if (context.performed)
      {
         ApplicationFacade.Instance.SendNotification(NotificationConst.
            Player_After_Choose_Dialogue_Option,
            context.ReadValue<Vector2>().y>0?-1:1);
      }
   }

   public void DialogueEnd(InputAction.CallbackContext context)
   {
      if (context.performed)
      {
         ApplicationFacade.Instance.SendNotification(NotificationConst.
            Player_Confirm_Choose_Dialogue_Option);
      }
   }

   #endregion
 
   
}
