using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlexFramework.Excel;
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
           dialogueNode.nextNode = varas[4];
           if(varas[5]!="null")
            dialogueNode.OptionNodes = ParseDialogueOptionNodes(varas[5]);
           return dialogueNode;
        }


        public static ToolDialogueNode ParseToolDialogueNode(string Content)
        {
            ToolDialogueNode dialogueNode = new ToolDialogueNode();
            var varas = Content.Split(',');
            dialogueNode.id = varas[0];
            dialogueNode.nodeType= varas[1]=="dialogue" ? NodeType.dialogue : NodeType.option;
            dialogueNode.SpeakerName = varas[2];
            dialogueNode.Content = varas[3];
            dialogueNode.nextNode = varas[4];
            if(varas[5]!="null")
                dialogueNode.OptionNodes = ParseDialogueOptionNodes(varas[5]);
            dialogueNode.Grade= int.Parse(varas[6]);
            return dialogueNode;
        }
        
        public static List<DialogueNode> ParseDialogueNodes(string filePath,int sheetIndex=0,bool isTool=false)
        {
            var fileContent = System.IO.File.ReadAllLines(filePath,System.Text.Encoding.UTF8);
            WorkBook book = new FlexFramework.Excel.WorkBook(filePath);
            var sheet = book[sheetIndex];
            
            List<DialogueNode> dialogueNodes = new List<DialogueNode>();
            // for (int i = 1; i < fileContent.Length; i++)
            // {
            //     dialogueNodes.Add(ParseDialogueNode(fileContent[i]));
            // }



            if (!isTool)
            {

                for (int r = 1; r < sheet.Rows.Count; r++)
                {
                    var row = sheet.Rows[r];

                    // 用 StringBuilder 拼当前行
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int c = 0; c < row.Cells.Count; c++)
                    {
                        sb.Append(row.Cells[c].Value);
                        if (c < row.Cells.Count - 1) sb.Append(',');
                    }

                    dialogueNodes.Add(ParseDialogueNode(sb.ToString()));

                }
            }
            else
            {
                for (int r = 1; r < sheet.Rows.Count; r++)
                {
                    var row = sheet.Rows[r];

                    // 用 StringBuilder 拼当前行
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int c = 1; c < row.Cells.Count; c++)
                    {
                        sb.Append(row.Cells[c].Value);
                        sb.Append(',');
                    }

                    object value = row.Cells[0].Value;
                    if (value == null)
                    {
                        value=sheet.Rows[r-1].Cells[0].Value;
                        sheet.Rows[r].Cells[0].Value=sheet.Rows[r-1].Cells[0].Value;
                    }
                    sb.Append(value);
                    if(row.Cells.Count < 6)
                        break;
                    dialogueNodes.Add(ParseToolDialogueNode(sb.ToString()));

                }
            }



            return dialogueNodes;
        }
       
      
          public static List<DialogueNode> ParseDialogueNodes(string filePath,string sheetIndex,bool isTool=false)
        {
            var fileContent = System.IO.File.ReadAllLines(filePath,System.Text.Encoding.UTF8);
            WorkBook book = new FlexFramework.Excel.WorkBook(filePath);
            var sheet = book[sheetIndex];
            if (sheet == null)
            {
                Debug.LogError("Sheet Not Found");
                sheet = book[0];
            }
            List<DialogueNode> dialogueNodes = new List<DialogueNode>();
            // for (int i = 1; i < fileContent.Length; i++)
            // {
            //     dialogueNodes.Add(ParseDialogueNode(fileContent[i]));
            // }



            if (!isTool)
            {

                for (int r = 1; r < sheet.Rows.Count; r++)
                {
                    var row = sheet.Rows[r];

                    // 用 StringBuilder 拼当前行
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int c = 0; c < row.Cells.Count; c++)
                    {
                        sb.Append(row.Cells[c].Value);
                        if (c < row.Cells.Count - 1) sb.Append(',');
                    }

                    dialogueNodes.Add(ParseDialogueNode(sb.ToString()));

                }
            }
            else
            {
                for (int r = 1; r < sheet.Rows.Count; r++)
                {
                    var row = sheet.Rows[r];

                    // 用 StringBuilder 拼当前行
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int c = 1; c < row.Cells.Count; c++)
                    {
                        sb.Append(row.Cells[c].Value);
                        sb.Append(',');
                    }

                    object value = row.Cells[0].Value;
                    if (value == null)
                    {
                        value=sheet.Rows[r-1].Cells[0].Value;
                        sheet.Rows[r].Cells[0].Value=sheet.Rows[r-1].Cells[0].Value;
                    }
                    sb.Append(value);
                    if(row.Cells.Count < 6)
                        break;
                    dialogueNodes.Add(ParseToolDialogueNode(sb.ToString()));

                }
            }



            return dialogueNodes;
        }
       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
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




