using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopShopPanel : PopUiBasePanel
{

    public const string IsFirst = "FirstShopAppearFirst";

    [SerializeField] private bool opened;


    [SerializeField] private Sprite[] oriClass;
    [SerializeField] private Sprite[] SwithchClass;

    [SerializeField] private List<Image> ChosenBar;
    
    public override void BeforeShowPopPanel()
    {
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
        
        PlayerAction.Instance.playerInput.SwitchCurrentActionMap("ShopInput");
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelCreate,this);
    }

    public override void AfterShowPopPanel()
    {
        base.AfterShowPopPanel();
    }

    public void SwitchGoods(Vector2 v)
    {
        //todo
        
    }

    public override void BeforeHidePopPanel()
    {
        base.BeforeHidePopPanel();
        ApplicationFacade.Instance.SendNotification(NotificationConst.ShopPanelHide);
    }
}
