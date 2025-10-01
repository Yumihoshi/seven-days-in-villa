using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class Steward : BaseNpcEntity
{
   
   
   public override void Interact()
   {
      
      base.Interact();
      if (!CanbeInteracted)
         return;
      int state = GameState.Instance.GetGameState();
      // if (state <= 1)
      {
         DialogueTree dialogueTree = ResourceLoader.Instance.LoadSO<DialogueTree>("DialogueData/¶Ô»°2");
         CanbeInteracted = false;
         DialogueSystemManager.Instance.StartDialogue(dialogueTree,Onend);
      }
   }

   public void Onend()
   {
     GameState.Instance.CurrentStateSlot.Step();
     
   }
}
