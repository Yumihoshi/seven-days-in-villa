using System.Collections;
using System.Collections.Generic;
using cjr.Scence;
using UnityEngine;

public class MainUiButtons : MonoBehaviour
{
   public void ShowPopInventoryPanel()
   {
      PopUiPanelController.Instance.CreatePopUiPanel(ConstVariable.InventoryPanel);
   }


   public void ShowPopShopPanel()
   {
      PopUiPanelController.Instance.CreatePopUiPanel(ConstVariable.ShopPanel);
   }

   public void BackToMainMenu()
   {
      SceneManager.Instance.BackToMainMenu();
   }
   
   
   
}
