using System;
using System.Collections;
using System.Collections.Generic;
using cjr.Scence;
using cjr.Single;
using UnityEngine;

public class PlayerSaving :cjr.Single. SingleMon<PlayerSaving>,RequireSavingItem
{
   [SerializeField] Vector3 lastPosition;
   [SerializeField] string LastSceneName;
   public const string LAST_Position = "lastPosition"; 
   public const string LAST_SCENE = "lastScene";
   protected override void Awake()
   {
      base.Awake();
     
   }

   public string GetLastSceneName()
   {
      if (!ES3.KeyExists(SaveSystemManager.Instance.SaveSlotName + LAST_SCENE))
         return "";
      return ES3.Load<string>(SaveSystemManager.Instance.SaveSlotName+LAST_SCENE);
   } 
   public void Save()
   {
      ES3.Save(SaveSystemManager.Instance.SaveSlotName+LAST_Position, transform.position);
      ES3.Save(SaveSystemManager.Instance.SaveSlotName+LAST_SCENE,cjr.Scence.SceneManager.Instance.GetCurrentScene());
      
   }

   public void Load()
   {
      if (!SaveSystemManager.Instance.IsDebug)
      {
         if (ES3.KeyExists(SaveSystemManager.Instance.SaveSlotName +LAST_Position))
         {
            lastPosition = ES3.Load<Vector3>(SaveSystemManager.Instance.SaveSlotName +LAST_Position);
         }
         else
         {
            lastPosition = Vector3.zero;
         }
         if (ES3.KeyExists(SaveSystemManager.Instance.SaveSlotName + LAST_SCENE))
         {
            LastSceneName = ES3.Load<string>(SaveSystemManager.Instance.SaveSlotName + LAST_SCENE);
         }
         transform.position=lastPosition;
         VcmManager.Instance.GetClosestConfiner();
         //todo
      }   
   }

   private void OnApplicationQuit()
   {
      // Save();
      //todo
      //����ϵͳ
   }
}
