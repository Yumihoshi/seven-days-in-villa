using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopUiPanelController : cjr.Single.SingleMon<PopUiPanelController>
{
  public Transform PopHolder;

  public Stack<GameObject> PopStack;
  public int NowLayer;

  [SerializeField] private float fadeOutTime = 0.5f;
  
  protected override void Awake()
  {
    base.Awake();
    PopHolder = transform;
    NowLayer = ConstVariable.PopLayerStart;
    PopStack=new Stack<GameObject>();
  }


  public void CreatePopUiPanel(GameObject prefab)
  {
    if (prefab == null)
    {
      Debug.LogError("CreatePopUiPanel: prefab is null!");
      return;
    }
    
    Canvas canvas = prefab.GetComponent<Canvas>();
    if (!canvas)
    {
      Debug.LogError("CreatePopUiPanel: prefab does not have Canvas component!");
      return;
    }
    
    // 修复Canvas设置
    canvas.overrideSorting = true;
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    
    // 设置父级
    prefab.transform.SetParent(PopHolder, false);
    
    // 更新层级
    NowLayer += ConstVariable.PopLayerAdd;
    canvas.sortingOrder = NowLayer;
    
    // 修复RectTransform
    RectTransform rectTransform = prefab.GetComponent<RectTransform>();
    if (rectTransform != null)
    {
      rectTransform.localScale = Vector3.one;
      rectTransform.sizeDelta = new Vector2(1920, 1080);
      rectTransform.anchorMin = Vector2.zero;
      rectTransform.anchorMax = Vector2.one;
      rectTransform.anchoredPosition = Vector2.zero;
    }
    
    // 确保UI元素激活
    prefab.SetActive(true);
    
    // 强制刷新Canvas
    Canvas.ForceUpdateCanvases();
  
    Debug.Log($"CreatePopUiPanel: Successfully created UI panel '{prefab.name}' with sorting order {NowLayer}");
    PopStack.Push(prefab);
  }
  
  
  public void CreatePopUiPanel(string prefabPath)
  {
    Debug.LogWarning(prefabPath);
    GameObject prefab = ResourceLoader.Instance.LoadObject(prefabPath,PopHolder);
    CreatePopUiPanel(prefab);
  }

  public void CloseCurrentPopUiPanel()
  {
    GameObject pop = PopStack.Pop();
    CanvasGroup cgp = pop.GetComponent<CanvasGroup>();
    if (!cgp)
    {
      cgp=pop.AddComponent<CanvasGroup>();
    }
    NowLayer -= ConstVariable.PopLayerAdd;
    cgp.DOFade(0,fadeOutTime).OnComplete(()=>
    {
      
      Destroy(pop);
      
    });
  }
  
}
