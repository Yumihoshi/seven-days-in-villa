using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolIneraction :InteractableItem
{
   [SerializeField] private string GameName;

   [SerializeField] private string Text;


   [SerializeField] private int Grade = 1;
   protected override void Awake()
   {
      
      base.Awake();
   }

   protected override void OnCollisionEnter2D(Collision2D other)
   {
      base.OnCollisionStay2D(other);
      UiGameobject.Instance.SetInteractableInfo("етЪЧ"+GameName,0.5f*GameName.Length);
   }

   public override void Interact()
   {
      base.Interact();
      ToolDialogueSkin.Instance.StartDialogue(DialogueMetas.Instance.GetToolDialogueTree(GameName, Grade));
   }

   protected override void OnCollisionExit2D(Collision2D other)
   {
      base.OnCollisionExit2D(other);
      if(other.gameObject.CompareTag("Player"))
         MainUiPanel.Instance.CloseToolPanel();
   }
}
