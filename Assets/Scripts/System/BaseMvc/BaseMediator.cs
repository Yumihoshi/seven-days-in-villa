using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using Unity.VisualScripting;
using UnityEngine;

public class BaseMediator : AbstractMediator
{
    
   
    const string NAME = "BaseMediator";
    
    protected List<string> notificationInterests = new List<string>();
    public BaseMediator( string name) : base(name)
    {
    }
    
    protected Dictionary<string,Action<object>> _entityActions = new Dictionary<string, Action<object>>();

    protected void RegisterEntityAction(string entityName, Action<object> action)
    {
        if(!notificationInterests.Contains(entityName))
            notificationInterests.Add(entityName);
        _entityActions[entityName] = action;
        
    }
   
    public override string[] ListNotificationInterests()
    {
        Debug.LogWarning(notificationInterests);
        return notificationInterests.ToArray();
    }


    protected virtual void HandleEntityAction(string entityName, object Para){}
    
    public override void HandleNotification(INotification notification)
    {
        //HandleEntityAction(notification.Name, notification.Body);
        _entityActions[notification.Name]?.Invoke(notification);
    }
}
