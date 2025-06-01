using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SingleHpBar : MonoBehaviour
{
    [SerializeField] private float tarscale;

    private void OnEnable()
    {
        tarscale = transform.localScale.x;
        Appear();
    }
/// <summary>
/// 为血量加入渐变的出现
/// </summary>
    public void Appear()
    {
        transform.localScale =Vector3.zero;
        transform.DOScale(new Vector3(tarscale, tarscale, tarscale), .2f);
    }
}
