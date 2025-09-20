using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Observer;
using UnityEngine;

public class ShopSystemMediator : BaseMediator
{
  public ShopSystemMediator() : base("ShopSystemMediator")
  {

    RegisterEntityAction(NotificationConst.ShopPanelCreate,OnShopPanelCreate);
    RegisterEntityAction(NotificationConst.ShopPanelHide,OnShopPanelHide);
  }

  void OnShopPanelCreate(object entity)
  {
    var pack = entity as Notification;
    
    PlayerAction.Instance.playerInput.SwitchCurrentActionMap("ShopInput");
    
    PopShopPanel popPanel = pack.Body as PopShopPanel;

    if (popPanel != null)
    {
      Debug.LogWarning(popPanel);
      //todo
      
    }
    
  }


  void OnShopPanelHide(object entity)
  {
    PlayerAction.Instance.playerInput.SwitchCurrentActionMap("Menu");
  }
  
  
}
