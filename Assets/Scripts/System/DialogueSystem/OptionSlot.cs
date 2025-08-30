using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlot : MonoBehaviour
{
   public TextMeshProUGUI Content;
   public TextMeshProUGUI Nos;

   public Image Chosen;

   public void Highlight()
   {
      Chosen.gameObject.SetActive(true);
   }

   public void Unhighlight()
   {
      Chosen.gameObject.SetActive(false);
   }
   
   public void SetTextContent(string content)
   {
      Content.text = string.Empty;
      Content.text = content;
   }

   public void SetTextNos(string nos)
   {
      Nos.text = string.Empty;
      Nos.text = nos;
   }
   
}
