using System;
using UnityEngine;

/// <summary>
/// 用于标记调试函数的特性
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DebugFunctionAttribute : Attribute
{
    /// <summary>
    /// 函数在Debug窗口中显示的名称（如果为null则使用方法名）
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// 函数的描述
    /// </summary>
    public string Description { get; }
    
    /// <summary>
    /// 函数所属的类别，用于在Debug窗口中分组
    /// </summary>
    public string Category { get; }
    
    /// <summary>
    /// 在同一类别中的排序顺序
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// 创建调试函数特性
    /// </summary>
    /// <param name="name">函数名称（可选，为null时使用方法名）</param>
    /// <param name="description">函数描述</param>
    /// <param name="category">所属类别</param>
    /// <param name="order">排序顺序</param>
    public DebugFunctionAttribute(string name = null, string description = "", string category = "General", int order = 0)
    {
        Name = name;
        Description = description;
        Category = category;
        Order = order;
    }
}