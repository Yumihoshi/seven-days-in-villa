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
    }


    void RegisterMediator(Mediator mediator)
    {
        Debug.LogWarning(mediator.MediatorName);
        ApplicationFacade.Instance.RegisterMediator(mediator);
        
    }
}
