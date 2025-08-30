using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionSlot : MonoBehaviour
{
   public TextMeshProUGUI Content;
   public TextMeshProUGUI Nos;

   public void SetTextContent(string content)
   {
      Content.text = content;
   }

   public void SetTextNos(string nos)
   {
      Nos.text = nos;
   }
   
}
