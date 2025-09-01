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
    }


    void RegisterMediator(Mediator mediator)
    {
        ApplicationFacade.Instance.RegisterMediator(mediator);
        
    }
}
