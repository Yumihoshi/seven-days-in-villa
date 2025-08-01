using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace cjr.Single 
{
   public class SingleMon<T> : MonoBehaviour where T : SingleMon<T>
   {
      private static T instance;



      protected virtual void OnDestroy()
      {
         
      }
      
      public static T Instance
      {
         get
         {
            
            if (instance == null)
            {
               instance = FindObjectOfType<T>();
              
               // if (instance == null) Debug.LogError($"δ�ҵ� {typeof(T).Name} ��ʵ��");
            }
            return instance;
         }
      }

      protected virtual void Awake()
      {
         // ���ʵ���Ѵ����Ҳ��ǵ�ǰ�������ٶ����ʵ��
         if (instance != null && instance != this)
         {
            Destroy(gameObject);
            return;
         }

         // ��ʼ������ʵ��
         instance = this as T;

         // ��ѡ���糡��������������
         DontDestroyOnLoad(gameObject);
      }
   }
}