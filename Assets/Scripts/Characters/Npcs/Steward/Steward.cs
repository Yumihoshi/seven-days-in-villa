using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class Steward : BaseNpcEntity
{
   
   
   public override void Interact()
   {
      int state = GameState.Instance.GetGameState();
      if (state <= 1)
      {
         DialogueTree dialogueTree = ResourceLoader.Instance.LoadSO<DialogueTree>("DialogueData/¶Ô»°2");
         DialogueSystemManager.Instance.StartDialogue(dialogueTree);
      }
   }
}
