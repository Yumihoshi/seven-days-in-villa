using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfHouse : InteractableItem
{
   protected override void Awake()
   {
      base.Awake();
      var collider = this.gameObject.GetComponent<Collider2D>();
      collider.isTrigger = true;
   }


   public override void OnTriggerEnter2D(Collider2D other)
   {
      base.OnTriggerEnter2D(other);
      if(other.gameObject.CompareTag("Player") )
         UiGameobject.Instance.SetInteractableInfo("     Àë¿ª±ðÊû     ",3.5f);
   }

   public override void Interact()
   {
      base.Interact();
    
      UiGameobject.Instance.SetMaskAlpha(1f,.1f);
      
      GameState.Instance.SwitchStateSlot(3);
      
   }


   public override void OnTriggerExit2D(Collider2D other)
   {
      base.OnTriggerExit2D(other);
      if(other.gameObject.CompareTag("Player") )
         UiGameobject.Instance.SetInteractbleInfoClose();
   }
}
