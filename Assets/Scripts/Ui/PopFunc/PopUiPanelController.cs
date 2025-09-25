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
  
  
  [SerializeField] PopUiBasePanel currentPopUiPanel;
  
  protected override void Awake()
  {
    base.Awake();
    PopHolder = transform;
    NowLayer = ConstVariable.PopLayerStart;
    PopStack=new Stack<GameObject>();
  }


  public GameObject CreatePopUiPanel(GameObject prefab,bool needFadeOut = true)
  {
    if (prefab == null)
    {
      Debug.LogError("CreatePopUiPanel: prefab is null!");
      return null;
    }
    
    Canvas canvas = prefab.GetComponent<Canvas>();
    if (!canvas)
    {
      Debug.LogError("CreatePopUiPanel: prefab does not have Canvas component!");
      return null;
    }
    prefab.gameObject.SetActive(false);
    // 修复Canvas设置
    canvas.overrideSorting = true;
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    
    
    currentPopUiPanel=prefab.GetComponent<PopUiBasePanel>();
    
    
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
    CanvasGroup cgp = prefab.GetComponent<CanvasGroup>();
    if (!cgp)
    {
      cgp=prefab.AddComponent<CanvasGroup>();
    }
    
    currentPopUiPanel=prefab.GetComponent<PopUiBasePanel>();
    
    currentPopUiPanel?.BeforeShowPopPanel();
    
    
    
    // 确保UI元素激活
    prefab.SetActive(true);
   
    currentPopUiPanel=prefab.GetComponent<PopUiBasePanel>();
    currentPopUiPanel?.AfterShowPopPanel();
    
    if (needFadeOut)
    {
      cgp.alpha = 0;
      cgp.DOFade(1f, fadeOutTime);
      
    }
    
    
    // 强制刷新Canvas
    Canvas.ForceUpdateCanvases();
  
    Debug.Log($"CreatePopUiPanel: Successfully created UI panel '{prefab.name}' with sorting order {NowLayer}");
    PopStack.Push(prefab);
    return prefab;
  }
  
  
  public GameObject CreatePopUiPanel(string prefabPath,bool needFadeOut = true)
  {
 
    GameObject prefab = ResourceLoader.Instance.LoadObject(prefabPath,PopHolder);
    return CreatePopUiPanel(prefab,needFadeOut);
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
    
    currentPopUiPanel=pop.GetComponent<PopUiBasePanel>();
    
    currentPopUiPanel?.BeforeHidePopPanel();
    cgp.DOFade(0,fadeOutTime).OnComplete(()=>
    {
      
      Destroy(pop);
      
    });
  }

  public void CloseTargetPanel(GameObject target)
  {
    if (target == null)
      return;

    // 创建一个临时栈来保存弹出的元素
    Stack<GameObject> tempStack = new Stack<GameObject>();
    bool found = false;

    // 遍历 PopStack 查找 target
    while (PopStack.Count > 0)
    {
      GameObject pop = PopStack.Pop();
      if (pop == target)
      {
        found = true;
        break;
      }
      tempStack.Push(pop);
    }

    // 把临时栈中的元素重新放回 PopStack
    while (tempStack.Count > 0)
    {
      PopStack.Push(tempStack.Pop());
    }

    // 如果找到了 target，才执行关闭逻辑
    if (found)
    {
      CanvasGroup cgp = target.GetComponent<CanvasGroup>();
      if (!cgp)
      {
        cgp = target.AddComponent<CanvasGroup>();
      }

      NowLayer -= ConstVariable.PopLayerAdd;

      PopUiBasePanel targetPanel = target.GetComponent<PopUiBasePanel>();
      targetPanel?.BeforeHidePopPanel();

      cgp.DOFade(0, fadeOutTime).OnComplete(() =>
      {
        Destroy(target);
      });
    }
  }
}
