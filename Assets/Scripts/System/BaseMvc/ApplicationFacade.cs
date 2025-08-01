using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Facade;
using UnityEngine;

public class ApplicationFacade: Facade
{
    public MediatorFactory factory;
    public ProxyFactory proxyFactory;
    public AudioFactory audioFactory;
   
    private static ApplicationFacade _instance;// = new ApplicationFacade();

    public static new ApplicationFacade Instance
    {
        get
        {
            return Facade.GetInstance(() => new ApplicationFacade()) as ApplicationFacade;
        }
    }
   
    public ApplicationFacade()
    {
        factory = new MediatorFactory();
        proxyFactory = new ProxyFactory();
        InitializeAudioFactory();
        // 延迟初始化AudioFactory，避免循环调用
    }
   
    /// <summary>
    /// 初始化音频工厂
    /// </summary>
    private void InitializeAudioFactory()
    {
        // 查找现有的AudioManager作为协程运行器
        cjr.AudioSystem.AudioManager audioManager = cjr.AudioSystem.AudioManager.Instance;
      
        if (audioManager == null)
        {
            Debug.LogError("ApplicationFacade: 未找到AudioManager，无法初始化AudioFactory");
            return;
        }
      
        // 初始化AudioFactory
        audioFactory = AudioFactory.Instance;
        audioFactory.Initialize(audioManager, initialPoolSize: 10, maxPoolSize: 50);
      
        Debug.Log("ApplicationFacade: AudioFactory 已初始化，使用现有AudioManager作为协程运行器");
    }
   
    /// <summary>
    /// 获取音频工厂实例
    /// </summary>
    /// <returns>AudioFactory实例</returns>
    public AudioFactory GetAudioFactory()
    {
        return audioFactory;
    }
   
    /// <summary>
    /// 清理资源
    /// </summary>
    public void Cleanup()
    {
        if (audioFactory != null)
        {
            audioFactory.Destroy();
            audioFactory = null;
        }
    }
}