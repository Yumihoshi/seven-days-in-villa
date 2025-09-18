using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// ???????????
/// </summary>
public class UiGameobject : cjr.Single.SingleMon<UiGameobject>
{
    #region ???????????

    [SerializeField] private CanvasGroup interactbleInfo;
    [SerializeField] TMPro.TextMeshProUGUI Infotext;
    [SerializeField] float infoDuration;
    [SerializeField] private float singleWordInterval = 0.2f;
    [SerializeField] private Coroutine IntervalCoroutine;
    #endregion


    public float GetSingleWordInterval()
    {
        return singleWordInterval;
    }
    
    protected override void Awake()
    {
        base.Awake();
        IntervalCoroutine = null;
    }

    #region nono
    public void SetInteractbleInfoClose()
    {
        if(IntervalCoroutine != null)
            StopCoroutine(IntervalCoroutine);
        interactbleInfo.DOFade(0, .15f).OnComplete(() =>
        {
            interactbleInfo.gameObject.SetActive(false);

        });
    }
    /// <summary>
    /// ?????????????????
    /// </summary>
    /// <param name="info"></param>
    /// <param name="waitingTime"></param>
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
        while (Infotext.text.Length < info.Length&& i<info.Length)
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

    

    #endregion

    public void SetPopHintPopPanel(string info, float waitingTime)
    {
        var Gobject = PopUiPanelController.Instance.CreatePopUiPanel(ConstVariable.PopHintPanel);
        
        PopHintPanel hintPanel = Gobject.GetComponent<PopHintPanel>();

        if (hintPanel != null)
        {
            hintPanel.SetInfos(info, waitingTime);
            
            hintPanel.StartInfo();
        }
        
    }
}
