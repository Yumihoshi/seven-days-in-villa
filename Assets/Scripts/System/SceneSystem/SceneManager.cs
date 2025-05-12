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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// ��ȡ��ǰ����ĳ�������
        /// </summary>
        /// <returns></returns>
        public string GetCurrentScene()
        {
            CurrentScene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            return CurrentScene;
        }
    }
    
}
