using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUiPanel : cjr.Single.SingleMon<MainUiPanel>
{
  [SerializeField] private ToolDialogueSkin toolDialogueSkin;

  public void setToolSentence(string sentence)
  {
    toolDialogueSkin.SetSentence(sentence);
  }
  
}
