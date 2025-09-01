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
      if(ToolDialoguedict == null)
          ToolDialoguedict = new Dictionary<string, Dictionary<int, DialogueTree>>();
    
// 确保Name对应的内层字典已初始化
      if(!ToolDialoguedict.ContainsKey(Name))
          ToolDialoguedict[Name] = new Dictionary<int, DialogueTree>();

      DialogueTree tmpTree = ScriptableObject.CreateInstance<DialogueTree>();
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
              // 存储当前Grade的DialogueTree
              ToolDialoguedict[Name][start] = tmpTree;
              start++;
            
              // 为新的Grade创建新的DialogueTree
              tmpTree = ScriptableObject.CreateInstance<DialogueTree>();
              tmpTree.nodes.Add(toolnode); // 别忘了添加当前节点到新树
          }
      }

// 不要忘记存储最后一个等级的树
      if (tmpTree.nodes.Count > 0)
      {
          ToolDialoguedict[Name][start] = tmpTree;
      }
  }


  public DialogueTree GetToolDialogueTree(string Name, int Grade)
  {
      if(ToolDialoguedict==null)
          ToolDialoguedict = new Dictionary<string, Dictionary<int,DialogueTree>>();

      if(!ToolDialoguedict.ContainsKey(Name))
          ToolDialoguedict[Name] = new Dictionary<int,DialogueTree>();

      Debug.LogWarning(Name+"  "+Grade);
      
      if (!ToolDialoguedict[Name].ContainsKey(Grade) || ToolDialoguedict[Name][Grade] == null)
      {
          var nodes = DialogueParser.ParseDialogueNodes(FileConstPath.ToolDialoguePath,
              Name, true);
    
          // 使用CreateInstance正确初始化ScriptableObject
          DialogueTree tmpTree = ScriptableObject.CreateInstance<DialogueTree>();
          tmpTree.nodes = nodes;
          Debug.LogWarning(tmpTree.nodes.Count);
          InitToolDialogueDict(Name, tmpTree);
          
      }
      
      return ToolDialoguedict[Name][Grade];
  }
  
}
