using System;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// ×Ô¶¯±£´æ
/// </summary>
public class SavingBase : MonoBehaviour,RequireSavingItem
{
    [SerializeField] private string baseKey;
    

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Start()
    {
        baseKey = SaveSystemManager.Instance.SaveSlotName +cjr.Scence.SceneManager.Instance.GetCurrentScene();
        Load();
        
    }
    
    public void Save()
    {
        
        ES3.Save<GameObject>(baseKey, gameObject);
        
    }
    public void Load()
    {
        if (ES3.KeyExists(baseKey))
        {
           ES3.LoadInto(baseKey, gameObject);
        }
    }
}
