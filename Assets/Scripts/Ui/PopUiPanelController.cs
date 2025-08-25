using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUiPanelController : cjr.Single.SingleMon<PopUiPanelController>
{
  public Transform PopHolder;

  public int NowLayer;
  protected override void Awake()
  {
    base.Awake();
    PopHolder = transform;
    NowLayer = ConstVariable.PopLayerStart;
  }


  public void CreatePopUiPanel(GameObject prefab)
  {
    
    Canvas canvas=prefab.GetComponent<Canvas>();
    if (!canvas)
    {
      return;
    }
    prefab.transform.SetParent(PopHolder);
    NowLayer += ConstVariable.PopLayerAdd;
    canvas.sortingOrder = NowLayer;
  }

  public void CreatePopUiPanel(string prefabPath)
  {
    GameObject prefab = ResourceLoader.Instance.LoadObject(prefabPath,PopHolder);
    CreatePopUiPanel(prefab);
  }

  
  
}
