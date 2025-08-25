using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

/// <summary>
/// 使用Odin库实现的自定义Debug窗口，用于在编辑器中执行调试功能
/// </summary>
public class OdinDebugWindow : OdinMenuEditorWindow
{
    private string searchText = "";
    private List<DebugFunction> debugFunctions = new List<DebugFunction>();

    // 添加菜单项以打开窗口
    [MenuItem("Debug/Open Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<OdinDebugWindow>("Debug").Show();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshDebugFunctions();
    }

    // 刷新所有调试函数
    public void RefreshDebugFunctions()
    {
        debugFunctions.Clear();
        
        // 查找所有带有DebugFunction属性的方法
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            try
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    foreach (var method in methods)
                    {
                        var attribute = method.GetCustomAttribute<DebugFunctionAttribute>();
                        if (attribute != null)
                        {
                            debugFunctions.Add(new DebugFunction
                            {
                                Name = attribute.Name ?? method.Name,
                                Description = attribute.Description,
                                Category = attribute.Category,
                                Method = method,
                                Order = attribute.Order
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading types from assembly: {e.Message}");
            }
        }

        // 按类别和顺序排序
        debugFunctions = debugFunctions.OrderBy(f => f.Category).ThenBy(f => f.Order).ToList();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(true);
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;

        // 按类别分组添加调试函数
        string currentCategory = null;
        foreach (var function in debugFunctions)
        {
            if (currentCategory != function.Category)
            {
                currentCategory = function.Category;
            }
            
            tree.Add($"{function.Category}/{function.Name}", function);
        }

        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

        // 绘制刷新按钮
        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
            {
                RefreshDebugFunctions();
                ForceMenuTreeRebuild();
            }
            GUILayout.FlexibleSpace();
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    protected override void DrawEditors()
    {
        var selected = this.MenuTree.Selection.FirstOrDefault();
        if (selected != null)
        {
            var function = selected.Value as DebugFunction;
            if (function != null)
            {
                DrawDebugFunction(function);
            }
        }
    }

    private void DrawDebugFunction(DebugFunction function)
    {
        GUILayout.Space(10);
        
        // 函数名称和描述
        EditorGUILayout.LabelField(function.Name, EditorStyles.boldLabel);
        if (!string.IsNullOrEmpty(function.Description))
        {
            EditorGUILayout.LabelField(function.Description, EditorStyles.wordWrappedLabel);
        }
        
        GUILayout.Space(10);
        
        // 执行按钮
        if (GUILayout.Button("Execute", GUILayout.Height(30)))
        {
            
            
            function.Method.Invoke(null, null);
            
           
        }
    }

    // 表示一个调试函数的数据结构
    public class DebugFunction
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public MethodInfo Method { get; set; }
        public int Order { get; set; }
    }
}