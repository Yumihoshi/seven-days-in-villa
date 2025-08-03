using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using PureMVC.Patterns.Mediator;
using UnityEngine;


public class DialogueMediator : BaseMediator
{
    new public const string NAME = "DialogueMediator";
    
    DialogueSystemManager _dialogueManager;
    
    
    
    public DialogueMediator() : base(NAME)
    {
        // 初始化对话系统
        
        _dialogueManager= DialogueSystemManager.Instance;
        Init();
    }

    void Init()
    {
        RegisterEntityAction(NotificationConst.Player_After_Choose_Dialogue_Option,OnbeginChosenOption);
        RegisterEntityAction(NotificationConst.Start_Dialogue,OnActionChange2Menu);
        RegisterEntityAction(NotificationConst.Player_Confirm_Choose_Dialogue_Option,OnConfirm_Choose_Dialogue_Option);
    }


    void OnConfirm_Choose_Dialogue_Option(object data)
    {
        DialogueSystemManager.Instance.optionConfirm = DialogueSystemManager.Instance.IsinOptions;
    }
    
    void OnActionChange2Menu(object parma)
    {
        PlayerAction.Instance.playerInput.SwitchCurrentActionMap("Dialogue");
        
    }
    //todo
    void OnbeginChosenOption(object param)
    {
        if (!_dialogueManager.IsinOptions)
            return;
        object body = ApplicationFacade.Unpackage(param).Body;
        if(body is int optionIndex)
        {
            DialogueSystemManager.Instance.CurrentOption+=optionIndex;
            DialogueSystemManager.Instance.CurrentOption %= DialogueSystemManager.
                Instance.CurrentoptionNodes.Count;
            if (DialogueSystemManager.Instance.CurrentOption < 0)
            {
                DialogueSystemManager.Instance.CurrentOption = DialogueSystemManager.
                    Instance.CurrentoptionNodes.Count-1;
            }
        }
        else
        {
            Debug.LogError("OnbeginChosenOption: 参数类型错误，应该是int类型的选项索引");
        }
    }
    
    
}