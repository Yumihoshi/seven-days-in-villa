using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueParser
    {
        public static DialogueNode ParseDialogueNode(string Content)
        {
           DialogueNode dialogueNode=new DialogueNode();
           var varas = Content.Split(',');
           dialogueNode.id = varas[0];
           dialogueNode.nodeType= varas[1]=="dialogue" ? NodeType.dialogue : NodeType.option;
           dialogueNode.SpeakerName = varas[2];
           dialogueNode.Content = varas[3];
           dialogueNode.OptionNodes = ParseDialogueOptionNodes(varas[4]);
           return dialogueNode;
        }


        public static List<DialogueNode> ParseDialogueNodes(string filePath)
        {
            var fileContent = System.IO.File.ReadAllLines(filePath);
          
            List<DialogueNode> dialogueNodes = new List<DialogueNode>();
            foreach (var line in fileContent)
            {
                dialogueNodes.Add(ParseDialogueNode(line));
            }
            return dialogueNodes;
        }
       
        //1<询问洛夫莱斯家族的历史。>=1011<br>
        //2<询问教授口中的雕塑。>=1014<br>
        //3<询问奇怪的藏品。>=1015<br>
        //4<没什么想问的了。>=1017
        public static List<DialogueOptionNode> ParseDialogueOptionNodes(string Content)
        {
            List<DialogueOptionNode> optionNodes = new List<DialogueOptionNode>();
            var options = Content.Split("<br>");
            foreach (var option in options)
            {
                optionNodes.Add(ParseDialogueOptionNode(option));
            }
            return optionNodes;
        }
        
        public static DialogueOptionNode ParseDialogueOptionNode(string Content)
        {
            DialogueOptionNode optionNode = new DialogueOptionNode();
            int index = 0;
            int length = Content.Length;

            string currentOption = string.Empty;
            
            while (index < length&& Content[index] != '<')
            {
                currentOption += Content[index];
                index++;
            }

            optionNode.id = currentOption;
            currentOption = string.Empty;
            index++; // 跳过 '<'
            while (index < length&& Content[index] != '>')
            {
                currentOption += Content[index];
                index++;
            }

            optionNode.OptionText = currentOption;
            index++; // 跳过 '>'
            index++;
            currentOption = string.Empty;
            while (index < length )
            {
                currentOption += Content[index];
                index++;
            }

            optionNode.nextNodeId = currentOption;
            
            return optionNode;
        }
    }

}





public class DialoguePhrarer 
{
  
}




