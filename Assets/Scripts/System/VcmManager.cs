using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VcmManager : Singleton<VcmManager>
{
    public CinemachineConfiner2D Confiner2D;

    private void Awake()
    {
        Confiner2D = GetComponentInChildren<CinemachineConfiner2D>();
    }

    public void SwitchConfiner2D(PolygonCollider2D collider)
    {
        Confiner2D.m_BoundingShape2D = collider;
        collider.isTrigger = true;
    }
}
