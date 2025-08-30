using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUiPanel : cjr.Single.SingleMon<MainUiPanel>
{
  [SerializeField] private ToolDialogueSkin toolDialogueSkin;

  public void setToolSentence(string sentence)
  {
    if (toolDialogueSkin.gameObject.activeSelf)
    {
      toolDialogueSkin.gameObject.SetActive(false);
      return;
    }
    toolDialogueSkin.SetSentence(sentence);
  }


  public void CloseToolPanel()
  {
    toolDialogueSkin.gameObject.SetActive(false);
  }
  
}
