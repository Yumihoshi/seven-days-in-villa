using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace DialogueSystem
{
   
   [Serializable]
   public enum NodeType
   {
     dialogue,
     option,
   }
   
   [Serializable]
   public class DialogueNode 
   {
      public string id;
      public NodeType nodeType;
      public string SpeakerName;
      
      [TextArea(3, 10)]
      public string Content;
      
      public string nextNode;
      public List<DialogueOptionNode> OptionNodes;
      
      
      public DialogueNode()
      {
         id = String.Empty;
         nodeType = NodeType.dialogue;
         SpeakerName = string.Empty;
         Content = string.Empty;
         nextNode = string.Empty;
         OptionNodes = new List<DialogueOptionNode>();
         OptionNodes.Clear();
      }

      public virtual void Debug()
      {
          UnityEngine.Debug.LogWarning(id+' '+nodeType+' '+SpeakerName+' '+Content);
      }
      
      
   }

   [Serializable]
   public class ToolDialogueNode : DialogueNode
   {
       public int Grade;
       public override void Debug()
       {
           UnityEngine.Debug.LogWarning(id+' '+nodeType+' '+SpeakerName+' '+Content+' '+Grade);
           
       }
   }
   
   
   [Serializable]
   public class DialogueOptionNode
   {
       public string id;
       [TextArea(3, 10)]
       public string OptionText;
       public string nextNodeId;

       public DialogueOptionNode()
       {
           OptionText= string.Empty;
           nextNodeId = string.Empty;
       }
       
   }
   

}

