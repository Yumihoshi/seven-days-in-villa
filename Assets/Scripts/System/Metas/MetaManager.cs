using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaManager : cjr.Single.SingleMon<MetaManager>
{
   List<BaseMeta> ToolMetas = new List<BaseMeta>();

   [SerializeField] private string Path = "Assets/Resources/SvnResource/文案/道具商品相关表格/物品表.xlsx";
   private void OnEnable()
   {
      ToolMetas=ExcelParser.ParseExcel(Path);
   }


   public BaseMeta GetToolMeta(int ToolID)
   {
      foreach (var meta in ToolMetas)
      {
         int id = int.Parse((string)meta.Get("ID"));
         if(id==ToolID)
            return meta;
      }
      return null;
   }
}
