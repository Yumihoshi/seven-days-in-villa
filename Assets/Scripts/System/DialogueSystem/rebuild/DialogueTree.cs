using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DialogueSystem
{
    
    [CreateAssetMenu(fileName = "DialogueTree", menuName = "Dialogue System/Dialogue Tree")]
    public class DialogueTree : ScriptableObject
    {

        public string dialogueName;
        public List<DialogueNode> nodes = new List<DialogueNode>();
           
    }
    
}
