using Sirenix.OdinInspector;
using UnityEngine;

public class SavingBase : MonoBehaviour,RequireSavingItem
{
    [SerializeField] private string baseKey;

    private void Awake()
    {
        
    }

    private void Start()
    {
        baseKey = SaveSystemManager.Instance.SaveSlotName +cjr.Scence.SceneManager.Instance.GetCurrentScene();
        
    }

    [Button("Save Data")]
    public void Save()
    {
        
        ES3.Save<GameObject>(baseKey, gameObject);
        
    }
    [Button("Load Data")]
    public void Load()
    {
        if (ES3.KeyExists(baseKey))
        {
           ES3.LoadInto(baseKey, gameObject);
        }
    }
}
