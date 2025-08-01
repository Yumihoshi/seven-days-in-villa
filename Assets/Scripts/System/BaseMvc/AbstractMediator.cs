using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Mediator;
using UnityEngine;

public class AbstractMediator : Mediator
{
    public AbstractMediator(string mediatorName, object viewComponent = null) : base(mediatorName, viewComponent)
    {
        // Debug.LogWarning("AbstractMediator:" + mediatorName);
    }

    public AbstractMediator():base(null, null)
    {
        
    }
   
}
