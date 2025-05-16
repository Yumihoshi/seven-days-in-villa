using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VcmManager :cjr.Single.Singleton<VcmManager>
{
    public CinemachineConfiner2D Confiner2D;

    private void Awake()
    {
        Confiner2D = GetComponentInChildren<CinemachineConfiner2D>();
    }


    public void GetClosestConfiner()
    {
        Confiner2D = GetComponentInChildren<CinemachineConfiner2D>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Confiner");
        float minDist = float.MaxValue;
        var player= PlayerAction.Instance.transform.position;
        foreach (GameObject obj in objs)
        {
            var dis=Vector3.Distance(obj.transform.position, player);
            if (dis < minDist)
            {
                minDist = dis;
                if(obj.GetComponent<PolygonCollider2D>())
                    Confiner2D.m_BoundingShape2D= obj.GetComponent<PolygonCollider2D>();
            }
        }
    }
    public void SwitchConfiner2D(PolygonCollider2D collider)
    {
        Confiner2D.m_BoundingShape2D = collider;
        collider.isTrigger = true;
    }
}
