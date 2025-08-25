using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUiButtons : MonoBehaviour
{
   public void ShowPopInventoryPanel()
   {
      PopUiPanelController.Instance.CreatePopUiPanel(ConstVariable.InventoryPanel);
   }
}
