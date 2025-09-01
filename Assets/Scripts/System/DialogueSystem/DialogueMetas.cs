using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class DialogueMetas:cjr.Single.Singleton<DialogueMetas>
{
  Dictionary<string, Dictionary<int,DialogueTree>> Dialoguedict = new Dictionary
    <string, Dictionary<int,DialogueTree>>();
  Dictionary<string, Dictionary<int,DialogueTree>> ToolDialoguedict = new Dictionary
      <string, Dictionary<int,DialogueTree>>();



  public void InitToolDialogueDict(string Name, DialogueTree dialogueTree)
  {
      if(ToolDialoguedict==null)
          ToolDialoguedict = new Dictionary<string, Dictionary<int,DialogueTree>>();
      DialogueTree tmpTree=new DialogueTree();
      int start = 1;
      foreach (var single in dialogueTree.nodes)
      {
          var toolnode = single as ToolDialogueNode;
          if (toolnode == null)
          {
              Debug.LogError("DialogueTree must be a ToolDialogueNode\n  Halt");
              return;
          }

          if (start == toolnode.Grade)
          {
              tmpTree.nodes.Add(toolnode);
          }
          else
          {
              ToolDialoguedict[Name][start] = tmpTree;
              start++;
              tmpTree=new DialogueTree();
          }
      }
  }


  public DialogueTree GetToolDialogueTree(string Name, int Grade)
  {
      if (ToolDialoguedict[Name][Grade] == null)
      {
          var nodes = DialogueParser.ParseDialogueNodes(FileConstPath.ToolDialoguePath,
              Name, true);
         
          DialogueTree tmpTree = new DialogueTree();
          tmpTree.nodes=nodes;
          InitToolDialogueDict(Name, tmpTree);
          
      }
      
      return ToolDialoguedict[Name][Grade];
  }
  
}
