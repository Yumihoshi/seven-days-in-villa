using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;


namespace cjr.Scence
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup Mask;

        public TweenerCore<float,float,FloatOptions> FainOut(float targetAlpha, float duration)
        {
            return  Mask.DOFade(targetAlpha, duration);
        }


        #region 单例
        public static SceneManager Instance
        {
            get
            {
                if(instance == null)
                    instance = FindObjectOfType<SceneManager>();
                return instance;
            }
        }
        static SceneManager instance;

        

        #endregion
        
        string CurrentScene;



        public void BackToMainMenu()
        {
            CoroutineFactory.Instance.RunCoroutine(BackToMainMenuCoroutine());
        }
        
        
        IEnumerator BackToMainMenuCoroutine()
        {
            Mask.DOFade(1, 0.3f);
            AsyncOperation operation = UnityEngine.SceneManagement.
                SceneManager.LoadSceneAsync(0);
            operation.allowSceneActivation = false; // 先禁止自动激活
            Mask.alpha = 0;
            SaveSystemManager.Instance.SaveGame();
            // 等待加载到 90%
            while (operation.progress < 0.9f)
            {
                yield return null;
            }

            // 此时加载已到 90%，允许激活场景
            operation.allowSceneActivation = true;

            // 等待场景完全激活
            while (!operation.isDone)
            {
                yield return null;
            }

            Mask.alpha = 0;
        
            yield return null;
            yield return null;
        
           
            
        }

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
