using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace cjr.Scence
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance
        {
            get
            {
                if(instance == null)
                    instance = FindObjectOfType<SceneManager>();
                return instance;
            }
        }
        string CurrentScene;
        static SceneManager instance;


        /// <summary>
        /// 获取当前激活的场景名称
        /// </summary>
        /// <returns></returns>
        public string GetCurrentScene()
        {
            CurrentScene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return CurrentScene;
        }
    }
    
}
