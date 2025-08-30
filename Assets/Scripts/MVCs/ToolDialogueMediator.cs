using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDialogueMediator : BaseMediator
{
   public ToolDialogueMediator() : base("ToolDialogueMediator")
   {
      RegisterEntityAction(NotificationConst.Player_After_Choose_Dialogue_Option,OnbeginChosenOption);
      
      RegisterEntityAction(NotificationConst.Player_Confirm_Choose_Dialogue_Option,OnConfirm_Choose_Dialogue_Option);
   }
   
   void OnbeginChosenOption(object param)
   {
      object body = ApplicationFacade.Unpackage(param).Body;
      if(body is int optionIndex)
      {
         ToolDialogueSkin.Instance.CurrentOption+=optionIndex;
         ToolDialogueSkin.Instance.CurrentOption %= ToolDialogueSkin.
            Instance.CurrentoptionNodes.Count;
         if (ToolDialogueSkin.Instance.CurrentOption < 0)
         {
            ToolDialogueSkin.Instance.CurrentOption = ToolDialogueSkin.
               Instance.CurrentoptionNodes.Count-1;
         }
      }
      else
      {
         Debug.LogError("OnbeginChosenOption: 参数类型错误，应该是int类型的选项索引");
      }
   }
   
   void OnConfirm_Choose_Dialogue_Option(object data)
   {
      ToolDialogueSkin.Instance.optionConfirm = ToolDialogueSkin.Instance.IsinOptions;
   }
}
