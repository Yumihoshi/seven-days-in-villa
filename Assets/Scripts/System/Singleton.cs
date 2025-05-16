using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace cjr.Single 
{
   public class Singleton<T> : MonoBehaviour where T : Singleton<T>
   {
      private static T instance;

      // 通过属性访问单例实例
      public static T Instance
      {
         get
         {
            // 如果实例不存在，尝试从场景中查找
            if (instance == null)
            {
               instance = FindObjectOfType<T>();
               // 如果没有找到，可以在此处选择是否自动创建实例
               // if (instance == null) Debug.LogError($"未找到 {typeof(T).Name} 的实例");
            }
            return instance;
         }
      }

      protected virtual void Awake()
      {
         // 如果实例已存在且不是当前对象，销毁多余的实例
         if (instance != null && instance != this)
         {
            Destroy(gameObject);
            return;
         }

         // 初始化单例实例
         instance = this as T;

         // 可选：跨场景保留单例对象
         DontDestroyOnLoad(gameObject);
      }
   }
}