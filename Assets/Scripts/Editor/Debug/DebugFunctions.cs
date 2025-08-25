using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 调试功能示例类
/// </summary>
public static class DebugFunctions
{
  
    [DebugFunction(name: "生成poplayer", 
        description: "", category: "工具")]
    public static void CreateWeapenSprite()
    {
      PopUiPanelController.Instance.CreatePopUiPanel(ConstVariable.InventoryPanel);
    }
  
   
}