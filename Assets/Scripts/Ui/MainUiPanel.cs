using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
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


  public void StartToolDialogue(DialogueTree tree)
  {
    if(tree == null)
      return;
    toolDialogueSkin.gameObject.SetActive(true);
    toolDialogueSkin.StartDialogue(tree);
  }
  public void CloseToolPanel()
  {
    toolDialogueSkin.gameObject.SetActive(false);
  }
  
}
