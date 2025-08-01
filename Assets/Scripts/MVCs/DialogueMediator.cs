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
    }
    
    //todo
    void OnbeginChosenOption(object param)
    {
        if (!_dialogueManager.IsinOptions)
            return;
        object body = ApplicationFacade.Unpackage(param).Body;
        if(body is int optionIndex)
        {
            _dialogueManager.DoChosen(optionIndex);
        }
        else
        {
            Debug.LogError("OnbeginChosenOption: 参数类型错误，应该是int类型的选项索引");
        }
    }
    
    
}