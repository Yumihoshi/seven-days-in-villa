using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Mediator;
using UnityEngine;

public class MediatorFactory 
{
    public MediatorFactory()
    {
        InitMediator();
    }

    void InitMediator()
    {
       RegisterMediator(new DialogueMediator());
       RegisterMediator(new ToolDialogueMediator());
       RegisterMediator(new ShopSystemMediator());
    }


    void RegisterMediator(Mediator mediator)
    {
        ApplicationFacade.Instance.RegisterMediator(mediator);
        
    }
    
    /// <summary>
    /// 如果指定名字的 Mediator 已注册，则直接移除
    /// </summary>
    /// <param name="mediatorName">注册时使用的名字</param>
    public void UnregisterIfExists(string mediatorName)
    {
        if (ApplicationFacade.Instance.HasMediator(mediatorName))
        {
            ApplicationFacade.Instance.RemoveMediator(mediatorName);
            Debug.Log($"[{mediatorName}] 已存在，已取消注册。");
        }
        else
        {
            Debug.Log($"[{mediatorName}] 未注册，无需取消。");
        }
    }
}
