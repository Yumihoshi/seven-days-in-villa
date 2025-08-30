using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolIneraction :InteractableItem
{
   [SerializeField] private string GameName;

   [SerializeField] private string Text;
   
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
      MainUiPanel.Instance.setToolSentence(Text);
   }
}
