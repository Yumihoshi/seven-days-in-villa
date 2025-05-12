using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cjr.Single
{
   public class Singleton<T> :MonoBehaviour where T:Singleton<T>
   {
      static T instance;

      public static T Instance
      {
         get
         {
            if(instance == null)
               instance = (T)FindObjectOfType(typeof(T));
            return instance;
         }
      }
   }
   
}
