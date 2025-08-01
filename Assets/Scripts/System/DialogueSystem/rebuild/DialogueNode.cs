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
      
      
      public DialogueNode nextNode;
      public List<DialogueNode> OptionNodes;


   }
  
}

