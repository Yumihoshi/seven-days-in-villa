// using System;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;
//
// namespace DialogueSystem
// {
//     public class TestDialogueParser
//     {
//          /// <summary>
//         /// 将JSON字符串解析为DialogueNode列表
//         /// </summary>
//         /// <param name="jsonContent">JSON字符串</param>
//         /// <returns>DialogueNode列表</returns>
//         public static List<DialogueNode> ParseDialogueJson(string jsonContent)
//         {
//             try
//             {
//                 // 使用Unity的JsonUtility解析JSON
//                 DialogueTree dialogueTree = JsonUtility.FromJson<DialogueTree>(jsonContent);
//                 
//                 if (dialogueTree == null || dialogueTree.nodes == null)
//                 {
//                     Debug.LogError("JSON解析失败：对话树为空或格式错误");
//                     return new List<DialogueNode>();
//                 }
//                 
//                 // 为每个DialogueNode处理其选项节点
//                 ProcessOptionNodes(dialogueTree.nodes);
//                 
//                 // 返回解析后的节点列表
//                 return dialogueTree.nodes;
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError($"解析对话JSON时出错: {e.Message}");
//                 return new List<DialogueNode>();
//             }
//         }
//         
//         /// <summary>
//         /// 处理选项节点中的nodeType，确保它们都是正确的枚举类型
//         /// </summary>
//         /// <param name="nodes">要处理的节点列表</param>
//         private static void ProcessOptionNodes(List<DialogueNode> nodes)
//         {
//             foreach (var node in nodes)
//             {
//                 // 确保节点类型正确 - 防止从字符串解析到枚举时可能的错误
//                 if (node.nodeType != NodeType.dialogue && node.nodeType != NodeType.option)
//                 {
//                     node.nodeType = NodeType.dialogue; // 默认对话类型
//                 }
//                 
//                 // 递归处理所有选项节点
//                 if (node.OptionNodes != null && node.OptionNodes.Count > 0)
//                 {
//                     //ProcessOptionNodes(node.OptionNodes);
//                 }
//             }
//         }
//         
//         /// <summary>
//         /// 获取对话树中的所有节点（包括选项）作为扁平列表
//         /// </summary>
//         /// <param name="jsonContent">JSON字符串</param>
//         /// <returns>所有节点的扁平列表</returns>
//         public static List<DialogueNode> GetAllNodesAsFlatList(string jsonContent)
//         {
//             List<DialogueNode> result = new List<DialogueNode>();
//             List<DialogueNode> rootNodes = ParseDialogueJson(jsonContent);
//             
//             // 将所有节点添加到扁平列表
//             foreach (var node in rootNodes)
//             {
//                 result.Add(node);
//                 CollectChildNodes(node, result);
//             }
//             
//             return result;
//         }
//         
//         /// <summary>
//         /// 递归收集所有子节点
//         /// </summary>
//         private static void CollectChildNodes(DialogueNode node, List<DialogueNode> collector)
//         {
//             if (node.OptionNodes == null || node.OptionNodes.Count == 0)
//             {
//                 return;
//             }
//             
//             foreach (var option in node.OptionNodes)
//             {
//                 collector.Add(option);
//                 CollectChildNodes(option, collector);
//             }
//         }
//         
//         /// <summary>
//         /// 根据ID查找节点
//         /// </summary>
//         public static DialogueNode FindNodeById(List<DialogueNode> nodes, string id)
//         {
//             // 在主节点列表中查找
//             foreach (var node in nodes)
//             {
//                 if (node.id == id)
//                 {
//                     return node;
//                 }
//                 
//                 // 在选项中查找
//                 if (node.OptionNodes != null)
//                 {
//                     foreach (var option in node.OptionNodes)
//                     {
//                         if (option.id == id)
//                         {
//                             return option;
//                         }
//                     }
//                 }
//             }
//             
//             return null;
//         }
//     }
// }