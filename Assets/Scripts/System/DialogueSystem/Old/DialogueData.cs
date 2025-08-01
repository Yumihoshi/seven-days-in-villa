using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOption
{
    public string id;
    public string text;
    public string nextNodeId;
    public string conditions;
    public string setVariable;
}

[Serializable]
public class DialogueNode
{
    public string id;
    public string type; // dialogue, options, end
    public string speaker;
    [TextArea(3, 8)]
    public string text;
    public string nextNodeId;
    public string conditions;
    public string file;
    public List<DialogueOption> options = new List<DialogueOption>();
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string dialogueFile;
    public List<DialogueNode> nodes = new List<DialogueNode>();
    
    // 添加查找节点的辅助方法
    public DialogueNode GetNodeById(string id)
    {
        return nodes.Find(node => node.id == id);
    }
    
    // 获取起始节点
    public DialogueNode GetStartNode()
    {
        if (nodes.Count > 0)
            return nodes[0];
        return null;
    }
}