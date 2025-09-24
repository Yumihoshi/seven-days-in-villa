using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns.Observer;
using UnityEngine;

public class ShopSystemMediator : BaseMediator
{
  public ShopSystemMediator() : base("ShopSystemMediator")
  {

    RegisterEntityAction(NotificationConst.ShopSwitch,OnShopSwitch);
    RegisterEntityAction(NotificationConst.ShopPanelCreate,OnShopPanelCreate);
    RegisterEntityAction(NotificationConst.ShopPanelHide,OnShopPanelHide);
  }


  private PopShopPanel popShopPanel;
  void OnShopSwitch(object para)
  {
    var pack = para as Notification;
    
    if (pack.Body is Vector2)
    {
      Vector2 v = (Vector2)pack.Body;

      Debug.LogWarning(popShopPanel);
      
      if (popShopPanel)
      {
        popShopPanel.SwitchGoods(v);
      }
      
      
    }
   
  }

  void OnShopPanelCreate(object entity)
  {
    var pack = entity as Notification;
                                                           // ShopInput
   
    
    PopShopPanel popPanel = pack.Body as PopShopPanel;

    
    if (popPanel != null)
    {
      
        popShopPanel=popPanel;
      //todo
      
    }
    
  }


  void OnShopPanelHide(object entity)
  {
    popShopPanel = null;
    PlayerAction.Instance.playerInput.SwitchCurrentActionMap("Menu");
  }
  
  
}
