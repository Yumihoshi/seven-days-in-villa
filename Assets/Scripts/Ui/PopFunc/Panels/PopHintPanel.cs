using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopHintPanel : PopUiBasePanel
{
  [SerializeField] private TextMeshProUGUI myText;

  [SerializeField] string text;
  [SerializeField] private float MyWaitTime;

  private void Awake()
  {
    myText = GetComponentInChildren<TextMeshProUGUI>();
  }

  IEnumerator TypingMachine(string info,float waitingTime)
  {
    if(myText == null|| text==String.Empty)
      yield break;
    WaitForSeconds wait = new WaitForSeconds(UiGameobject.Instance.GetSingleWordInterval());
    WaitForSeconds End = new WaitForSeconds(UiGameobject.Instance.GetSingleWordInterval()* (info.Length / 2 + 1));

    myText.text = String.Empty;

    for (int i = 0; i < info.Length; i++)
    {
      myText.text += info[i];
      yield return wait;
    }
    yield return End;

    wait =new WaitForSeconds(waitingTime);
    yield return wait;
    text=string.Empty;
    PopUiPanelController.Instance.CloseTargetPanel(this.gameObject);

  }

  public void SetInfos(string info, float waitingTime)
  {
    text = info;
    MyWaitTime = waitingTime;
  }
  
  
  public void StartInfo()
  {
        CoroutineFactory.Instance.RunCoroutine(TypingMachine(text, MyWaitTime));
  }


}
