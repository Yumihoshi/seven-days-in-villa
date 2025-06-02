using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHpSystem : cjr.Single.Singleton<PlayerHpSystem>,RequireSavingItem
{

  #region UI展示部分
    [SerializeField] Transform HpBar;
    [SerializeField] Transform HpHolder;
    [SerializeField] private GameObject SinGleHp;
  #endregion

  #region 具体逻辑

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
    ES3.Save(key,HealthPoints);
  }

  public void Load()
  {
    string key = SaveSystemManager.Instance.SaveSlotName + PlayerHp;
    if (ES3.KeyExists(key))
    {
      int hp = ES3.Load<int>(key);
      HealthPoints = hp;
    }
    Initialize();
  }
}
