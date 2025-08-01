using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Proxy;
using UnityEngine;

public class ProxyFactory 
{
    public ProxyFactory()
    {
        InitProxy();
    }

    void InitProxy()
    {
       
    }

    void RegisterProxy(Proxy proxy)
    {
        Debug.LogWarning(proxy.ProxyName);
        ApplicationFacade.Instance.RegisterProxy(proxy);
    }
}