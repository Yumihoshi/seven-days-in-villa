using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using Unity.VisualScripting;
using UnityEngine;

public class BaseProxy : AbstractProxy
{
    const string NAME = "BaseProxy";
    
    protected List<string> notificationInterests = new List<string>();
    
    public BaseProxy(string name) : base(name)
    {
    }
    
    protected Dictionary<string, Action<object>> _entityActions = new Dictionary<string, Action<object>>();

    protected void RegisterEntityAction(string entityName, Action<object> action)
    {
        if(!notificationInterests.Contains(entityName))
            notificationInterests.Add(entityName);
        _entityActions[entityName] = action;
    }
   
    public virtual string[] ListNotificationInterests()
    {
        Debug.LogWarning(notificationInterests);
        return notificationInterests.ToArray();
    }

    protected virtual void HandleEntityAction(string entityName, object Para){}
    
    public virtual void HandleNotification(INotification notification)
    {
        //HandleEntityAction(notification.Name, notification.Body);
        _entityActions[notification.Name]?.Invoke(notification);
    }
}