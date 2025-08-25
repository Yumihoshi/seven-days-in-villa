using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 调试功能示例类
/// </summary>
public static class DebugFunctions
{
  
    [DebugFunction(name: "测试weapen 的精灵图", 
        description: "在场景中生成随机位置的立方体", category: "工具")]
    public static void CreateWeapenSprite()
    {
      Debug.Log("aaaa");
    }
  
   
}