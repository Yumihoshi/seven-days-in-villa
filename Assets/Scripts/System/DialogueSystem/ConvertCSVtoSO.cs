using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ConvertCSVtoSO : MonoBehaviour
{
    public string TargetPosition = ".\\Resources\\DialogueData";
    [TextArea(minLines: 2, maxLines: 4)]
    public string SourcePosition = ".\\Resources\\DialogueData";


    [Button("Convert CSV to SO")]
    public void ConvertCSVtoSOFunc()
    {
        string[] lines = System.IO.File.ReadAllLines(SourcePosition);
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log(lines[i]);
        }
        DialogueBaseItem dialogueBaseItem = ScriptableObject.CreateInstance<DialogueBaseItem>();
        string name = "aaa.asset";
        string fullPath = System.IO.Path.Combine(TargetPosition, name);
        Debug.Log(fullPath);
        AssetDatabase.CreateAsset(dialogueBaseItem, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
