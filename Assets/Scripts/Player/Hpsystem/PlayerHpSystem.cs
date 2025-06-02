using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHpSystem : MonoBehaviour
{

  #region UI展示部分
    [SerializeField] Transform HpBar;
    [SerializeField] Transform HpHolder;
    [SerializeField] private GameObject SinGleHp;
  #endregion

  #region 具体逻辑

    [SerializeField] int HealthPoints;

  #endregion

  [Button("AddHp")]
  public void AddHp()
  {
    HealthPoints++;
    Instantiate(SinGleHp, HpHolder);
  }
  
  
}
