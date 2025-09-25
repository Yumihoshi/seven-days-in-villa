using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlexFramework.Excel;
using UnityEngine;

public class BaseMeta
{
    public Dictionary<string, object> _data;

    public BaseMeta()
    {
        
    }
    
    public BaseMeta(Dictionary<string, object> data)
    {
        _data = data;
    }

    // 通过字段名拿值
    public object Get(string key)
    {
        if (_data.TryGetValue(key, out var value))
            return value;
        return null;
    }


    public object Get(int index)
    {
        if(index < 0 || index >= _data.Count)
            return null;
        return _data.ElementAt(index);
    }
    // 可选：简化用法
}


public static class ExcelParser
{
    public static List<BaseMeta> ParseExcel(string filePath)
    {
        var metas = new List<BaseMeta>();
        WorkBook book = new WorkBook(filePath);
        var sheet = book[0];
        // 取表头
        var header = new List<string>();
        bool isFirstRow = true;
        int nameIndex = -1; // 记录“物品名称”列的位置

        foreach (var row in sheet.Rows)
        {
            if (isFirstRow)
            {
                for (int idx = 0; idx < row.Cells.Count; idx++)
                {
                    string colName = row.Cells[idx].Value?.ToString() ?? "";
                    header.Add(colName);
                    if (colName == "ID")
                        nameIndex = idx;
                }
                isFirstRow = false;
                continue;
            }

            // 如果没找到“物品名称”列，直接跳过
            if (nameIndex < 0 || nameIndex >= row.Cells.Count)
                continue;

            var nameCell = row.Cells[nameIndex];
            var nameValue = nameCell.Value?.ToString();
            if (string.IsNullOrEmpty(nameValue))
                continue; // 名称为 null 或空，跳过

            var dict = new Dictionary<string, object>();
            for (int i = 0; i < row.Cells.Count && i < header.Count; i++)
            {
                dict[header[i]] = row.Cells[i].Value;
            }
            metas.Add(new BaseMeta(dict));
        }

        return metas;
    }
}
