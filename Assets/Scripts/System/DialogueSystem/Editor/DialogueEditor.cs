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
        string path = EditorUtility.OpenFilePanel("选择CSV文件", "", "xlsx");
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

    
    
    
    
    [HorizontalGroup("BatchButtons")]
[Button("批量生成所有Sheet")]
private void makeFilesFromAllSheets()
{
    if (string.IsNullOrEmpty(filePath))
    {
        EditorUtility.DisplayDialog("错误", "请先选择Excel文件", "确定");
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
        // 获取文件名（不含扩展名）
        string baseFileName = Path.GetFileNameWithoutExtension(filePath);
        
        // 获取表中sheet的数量
        int sheetCount = DialogueParser.GetSheetCount(filePath);
        
        if (sheetCount == 0)
        {
            EditorUtility.DisplayDialog("警告", "未找到任何Sheet", "确定");
            return;
        }

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

        int successCount = 0;

        for (int i = 0; i < sheetCount; i++)
        {
            try
            {
                // 显示进度条
                EditorUtility.DisplayProgressBar("批量生成中", $"正在处理Sheet: {baseFileName}_{i} ({i + 1}/{sheetCount})", (float)i / sheetCount);
                
                // 解析指定sheet索引的对话节点
                List<DialogueNode> nodes = DialogueParser.ParseDialogueNodes(filePath, i);
                
                // 如果该sheet没有数据，跳过
                if (nodes == null || nodes.Count == 0)
                {
                    Debug.LogWarning($"Sheet {baseFileName}_{i} 没有数据，跳过");
                    continue;
                }
                
                // 创建DialogueTree
                DialogueTree dialogueTree = ScriptableObject.CreateInstance<DialogueTree>();
                dialogueTree.dialogueName = $"{baseFileName}_{i}"; // 表名_sheet索引
                dialogueTree.nodes = nodes;
                
                // 生成输出文件路径
                string outputPath = Path.Combine(relativePath, $"{dialogueTree.dialogueName}.asset");
                
                // 保存ScriptableObject
                AssetDatabase.CreateAsset(dialogueTree, outputPath);
                
                successCount++;
                Debug.Log($"成功生成: {outputPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"处理Sheet {baseFileName}_{i} 时出错: {e.Message}");
            }
        }

        // 清除进度条
        EditorUtility.ClearProgressBar();
        
        // 保存并刷新资源
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("批量生成完成", 
            $"成功生成 {successCount}/{sheetCount} 个DialogueTree文件\n输出目录: {relativePath}", 
            "确定");
        
        Debug.Log($"批量生成完成，成功: {successCount}, 总数: {sheetCount}");
    }
    catch (System.Exception e)
    {
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("错误", $"批量生成文件时出错: {e.Message}", "确定");
        Debug.LogError($"批量生成DialogueTree时出错: {e.Message}");
    }
}
    
    
    
    
    
    
    
    
}
