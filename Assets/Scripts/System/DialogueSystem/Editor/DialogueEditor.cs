using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using DialogueSystem;

public class DialogueEditor : OdinEditorWindow
{
    [MenuItem("对话配置/配置csv文件")]
    private static void ShowWindow()
    {
        var window = GetWindow<DialogueEditor>();
        window.titleContent = new GUIContent("文本输入");
        window.minSize = new Vector2(400, 200);
        window.Show();
    }



    [SerializeField]
    [LabelText("文件路径")]
    [LabelWidth(100)]
    [ReadOnly]
    [TextArea(2, 3)]
    private string filePath = "";

    [SerializeField]
    [LabelText("输出目录")]
    [LabelWidth(100)]
    [ReadOnly]
    [TextArea(2, 3)]
    private string outputDirectory = "";

    [HorizontalGroup("Buttons")]
    [Button("选择文件", ButtonSizes.Small)]
    private void SelectFile()
    {
        string path = EditorUtility.OpenFilePanel("选择CSV文件", "", "csv");
        if (!string.IsNullOrEmpty(path))
        {
            filePath = path;
            Debug.Log($"选择的文件路径: {filePath}");
        }
    }

    [HorizontalGroup("Buttons")]
    [Button("选择目录", ButtonSizes.Small)]
    private void SelectDirectory()
    {
        string path = EditorUtility.OpenFolderPanel("选择输出目录", Application.dataPath, "");
        if (!string.IsNullOrEmpty(path))
        {
            outputDirectory = path;
            Debug.Log($"选择的输出目录: {outputDirectory}");
        }
    }

    [HorizontalGroup("Buttons")]
    [Button("生成文件")]
    private void makeFile()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            EditorUtility.DisplayDialog("错误", "请先选择CSV文件", "确定");
            return;
        }

        if (string.IsNullOrEmpty(outputDirectory))
        {
            EditorUtility.DisplayDialog("错误", "请先选择输出目录", "确定");
            return;
        }

        if (!File.Exists(filePath))
        {
            EditorUtility.DisplayDialog("错误", "选择的文件不存在", "确定");
            return;
        }

        try
        {
            // 使用您现有的DialogueParser解析CSV文件
            List<DialogueNode> nodes = DialogueParser.ParseDialogueNodes(filePath);
            
            // 创建DialogueTree
            DialogueTree dialogueTree = ScriptableObject.CreateInstance<DialogueTree>();
            dialogueTree.dialogueName = Path.GetFileNameWithoutExtension(filePath);
            dialogueTree.nodes = nodes;

            // 将绝对路径转换为相对于Assets的路径
            string relativePath = "";
            if (outputDirectory.StartsWith(Application.dataPath))
            {
                // 如果输出目录在Assets文件夹内，转换为相对路径
                relativePath = "Assets" + outputDirectory.Substring(Application.dataPath.Length);
            }
            else
            {
                // 如果不在Assets文件夹内，默认保存到Assets文件夹
                relativePath = "Assets/DialogueTrees";
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "DialogueTrees"));
            }
            
            // 生成输出文件路径
            string outputPath = Path.Combine(relativePath, $"{dialogueTree.dialogueName}.asset");
            
            Debug.LogWarning($"相对路径: {outputPath}");
            
            // 保存ScriptableObject
            AssetDatabase.CreateAsset(dialogueTree, outputPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("成功", $"DialogueTree已生成到: {outputPath}", "确定");
            Debug.Log($"DialogueTree已生成: {outputPath}");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("错误", $"生成文件时出错: {e.Message}", "确定");
            Debug.LogError($"生成DialogueTree时出错: {e.Message}");
        }
    }

}
