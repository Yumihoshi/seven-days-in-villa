using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Proxy;
using UnityEngine;

public class AbstractProxy : Proxy
{
    public AbstractProxy(string proxyName, object data = null) : base(proxyName, data)
    {
        // Debug.LogWarning("AbstractProxy:" + proxyName);
    }

    public AbstractProxy() : base(null, null)
    {
        
    }
}