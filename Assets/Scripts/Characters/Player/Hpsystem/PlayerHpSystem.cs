using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHpSystem : cjr.Single.SingleMon<PlayerHpSystem>,RequireSavingItem
{

  #region UI
    [SerializeField] Transform HpBar;
    [SerializeField] Transform HpHolder;
    [SerializeField] private GameObject SinGleHp;
  #endregion

  #region 数值

    [SerializeField] int HealthPoints;
    [SerializeField] const string PlayerHp = "PlayerHp";
  #endregion

  protected override void Awake()
  {
    base.Awake();
  }

  [Button("AddHp")]
  public void AddHp()
  {
    HealthPoints++;
    Instantiate(SinGleHp, HpHolder);
  }


  [SerializeField] private int now_San = 1000;


  public int GetMySan()
  {
      return now_San;
  }

  public bool subSan(int sub)
  {
    if (sub > now_San)
      return false;
    now_San -= sub;
    return true;
  }
  public void Initialize()
  {
    for (int i = 0; i < HpHolder.childCount; i++)
    {
      Destroy(HpHolder.GetChild(i).gameObject);
    }

    for (int i = 0; i < HealthPoints; i++)
    {
      Instantiate(SinGleHp, HpHolder);
    }
  }

  public void Save()
  {
    string key=SaveSystemManager.Instance.SaveSlotName+PlayerHp;
    Debug.LogWarning(key);
    ES3.Save(key,HealthPoints);
  }

  public void Load()
  {
    string key = SaveSystemManager.Instance.SaveSlotName + PlayerHp;
    Debug.LogWarning(key);
    if (ES3.KeyExists(key))
    {
      int hp = ES3.Load<int>(key);
      HealthPoints = hp;
    }
    else
    {
      Debug.LogWarning(key+"not exist");
      HealthPoints = 4;
    }
    Initialize();
  }
}
