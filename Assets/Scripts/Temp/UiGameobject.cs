using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 场景持久物体
/// </summary>
public class UiGameobject : cjr.Single.Singleton<UiGameobject>
{
    #region 有交互的提示

    [SerializeField] private CanvasGroup interactbleInfo;
    [SerializeField] TMPro.TextMeshProUGUI Infotext;
    [SerializeField] float infoDuration;
    [SerializeField] private float singleWordInterval = 0.2f;
    [SerializeField] private Coroutine IntervalCoroutine;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        IntervalCoroutine = null;
    }


    public void SetInteractbleInfoClose()
    {
        if(IntervalCoroutine != null)
            StopCoroutine(IntervalCoroutine);
        interactbleInfo.DOFade(0, .15f).OnComplete(() =>
        {
            interactbleInfo.gameObject.SetActive(false);

        });
    }
    public void SetInteractableInfo(string info,float waitingTime)
    {
        
        if (IntervalCoroutine != null)
        {
            StopCoroutine(IntervalCoroutine);
            IntervalCoroutine = null;
        }

        Infotext.text = string.Empty;
        interactbleInfo.alpha = 0;
        interactbleInfo.gameObject.SetActive(true);
        interactbleInfo.DOFade(1,0.2f).OnComplete(()=>
        {
            IntervalCoroutine = StartCoroutine(TypeingMachine(info,waitingTime));
        });
    }

    IEnumerator TypeingMachine(string info,float waitingTime)
    {
        Infotext.text = string.Empty;
        int i = 0;
        while (Infotext.text.Length < info.Length)
        {
            Infotext.text += info[i];
            i++;
            yield return new WaitForSeconds(singleWordInterval);
        }
        yield return new WaitForSeconds(waitingTime);
        interactbleInfo.DOFade(0, 0.2f).OnComplete(() =>
        {
            interactbleInfo.gameObject.SetActive(false);
        });
    }

    public void Update()
    {
        
    }
}
