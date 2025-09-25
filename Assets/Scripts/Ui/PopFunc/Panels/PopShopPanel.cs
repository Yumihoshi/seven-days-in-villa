using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PopShopPanel : PopUiBasePanel
{

    public const string IsFirst = "FirstShopAppearFirst";

    [SerializeField] private bool opened;


    [SerializeField] private Sprite[] oriClass;
    [SerializeField] private Sprite[] SwithchClass;

    [SerializeField] private List<Image> ChosenBar;


    [SerializeField] private int CurrentChosen;
    private Vector3 pos;

    [SerializeField] private Image ItemIcon;
    
    public override void BeforeShowPopPanel()
    {
        pos = ItemIcon.transform.position;
       
        base.BeforeShowPopPanel();
        // Debug.LogWarning("用你心智的清明，来换取肉体的存续");
        if (!SaveSystemManager.Instance.IsContainKey(IsFirst))
        {
            
            UiGameobject.Instance.SetPopHintPopPanel("用你心智的清明，来换取肉体的存续",0.4f,true);
            
            SaveSystemManager.Instance.SaveObject(IsFirst,opened);
        }
        else
        {
            Debug.LogWarning("i am saved opened ");
        }

        CurrentChosen = 0;
        PlayerAction.Instance.playerInput.SwitchCurrentActionMap("ShopInput");
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelCreate,this);
    }

    public override void AfterShowPopPanel()
    {
        pos = ItemIcon.transform.position;
        
        SetChosen(CurrentChosen);
    }


    [SerializeField] private float downPoi = 10f;
    [SerializeField] private float duration = .5f;
    
    void AppearIcon()
    {
      
       
        ItemIcon.transform.localScale = Vector3.zero;
        Vector3 downpos = pos;
        downpos.y -= downPoi;
        ItemIcon.color = new Color(1f, 1f, 1f, 0);
        // ItemIcon.transform.position = downpos;

        ItemIcon.transform.position = downpos;
        
        ItemIcon.transform.DOMoveY(pos.y, duration);
        ItemIcon.DOFade(1, duration);
        ItemIcon.transform.DOScale(Vector3.one, duration);
        
    }
    
    public void ClearSp()
    {
        for (int i = 0; i < ChosenBar.Count; i++)
        {
            ChosenBar[i].sprite = oriClass[i];
            ChosenBar[i].SetNativeSize();
        }
    }
    
    public void SetChosen(int chosen)
    {
        ClearSp();
        ChosenBar[CurrentChosen].sprite = SwithchClass[chosen];
        ChosenBar[CurrentChosen].SetNativeSize();
        //todo
        
        AppearIcon();
    }
    
    public void SwitchGoods(Vector2 v)
    {

        if (v.x != 0)
        {
            CurrentChosen+=(int) (v.x / Mathf.Abs(v.x));
            if (CurrentChosen >= ChosenBar.Count)
            {
                CurrentChosen = 0;
            }

            if (CurrentChosen < 0)
            {
                CurrentChosen = 2;
            }
        }
        SetChosen(CurrentChosen);
    }

    public override void BeforeHidePopPanel()
    {
        base.BeforeHidePopPanel();
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelHide);
    }
}
