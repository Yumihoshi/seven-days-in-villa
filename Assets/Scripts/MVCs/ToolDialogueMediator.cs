using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDialogueMediator : BaseMediator
{
   public ToolDialogueMediator() : base("ToolDialogueMediator")
   {
      RegisterEntityAction(NotificationConst.Player_After_Choose_Dialogue_Option,OnbeginChosenOption);
   }
   
   void OnbeginChosenOption(object param)
   {
      object body = ApplicationFacade.Unpackage(param).Body;
      if(body is int optionIndex)
      {
         Debug.LogWarning(optionIndex);
      }
      else
      {
         Debug.LogError("OnbeginChosenOption: 参数类型错误，应该是int类型的选项索引");
      }
   }
}
