using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : cjr.Single.SingleMon<DialogueManager>
{
    [SerializeField] DialogueBaseItem needLoad;
    
    [SerializeField] Transform  dialogueUi;
    [SerializeField] TMPro.TextMeshProUGUI textContent;
    [SerializeField] TMPro.TextMeshProUGUI SpeakerName;
    [SerializeField] Image speaker1Image;
    [SerializeField] Image speaker2Image;
    [SerializeField] private float scale_ori = 0.6f;
    [SerializeField] private float scale_tar = 1.2f;

    [SerializeField] private float singleInterval = 0.06f;
    
    [SerializeField] bool IsSpeedUp = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SpeedUp()
    {
        IsSpeedUp = true;
    }
    private void OnEnable()
    {
    }

    public void SetUp()
    {
        textContent.text = "";
        speaker1Image.transform.localScale = new Vector3(scale_ori, scale_ori, scale_ori);
        speaker2Image.transform.localScale = new Vector3(scale_ori, scale_ori, scale_ori);
        dialogueUi.gameObject.SetActive(true);
    }

    /// <summary>
    /// ʹ�öԻ�ÿ����Ҫ�����öԻ���Ԫ������
    /// Ȼ������ü���
    /// </summary>
    /// <param name="item"></param>
    public void DialogueStart(DialogueBaseItem item)
    {
        StartCoroutine(ChatAll(item));
    }


    void ActiveImage(SingleDialogueElement it)
    {
        var speakerType = it.SpeakerType;
        Vector3 oriScale=new Vector3(scale_ori, scale_ori, scale_ori);
        Vector3 tarScale=new Vector3(scale_tar, scale_tar, scale_tar);
        switch (speakerType)
        {
            case SpeakerType.Speaker1:
                speaker1Image.transform.DOScale(tarScale, 0.1f);
                speaker1Image.sprite = it.Mysprite;
                speaker2Image.sprite = it.NextSprite;
                speaker2Image.transform.DOScale(oriScale, 0.1f);
                break;
            case SpeakerType.Speaker2:
                speaker2Image.sprite = it.Mysprite;
                speaker1Image.sprite = it.NextSprite;
                speaker2Image.transform.DOScale(tarScale, 0.1f);
                speaker1Image.transform.DOScale(oriScale, 0.1f);
                break;
        }
    }

    void SetSpeakerName(Speakers speakerType)
    {
        SpeakerName.text = speakerType.ToString();
    }
    IEnumerator ChatAll(DialogueBaseItem item)
    {
        SetUp();
        foreach (var it in item.AllElements)
        {
            
            ActiveImage(it);
            SetSpeakerName(it.Speaker);
            IsSpeedUp = false;
            yield return chat(it.text);
            IsSpeedUp = false;
        }

        dialogueUi.gameObject.SetActive(false);
        yield return null;
    }


    IEnumerator chat(string text)
    {
        textContent.text ="";
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(singleInterval);
            if(IsSpeedUp)
                break;
            textContent.text += text[i];
        }
        IsSpeedUp=false;
        textContent.text = text;
        float alltime=singleInterval*textContent.text.Length;
        while (!IsSpeedUp && alltime > 0)
        {
            yield return null;
            alltime-=Time.deltaTime;
        }
        IsSpeedUp=false;
    }
}
