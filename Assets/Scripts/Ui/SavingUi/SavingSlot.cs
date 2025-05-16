using System;
using System.Collections;
using System.Collections.Generic;
using cjr.Scence;
using UnityEngine;
using UnityEngine.UI;

public class SavingSlot : MonoBehaviour
{
   [SerializeField] SlotName slotName;
   [SerializeField] Button saveButton;

   private void Awake()
   {
      saveButton = GetComponent<Button>();
      saveButton.onClick.AddListener(()=>PointDown());
   }

   public void PointDown()
   {
      SaveSystemManager.Instance.SaveGame();
      SaveSystemManager.Instance.gameData_SO.CurrentSaveSlotName = slotName.ToString();
      string sceneName = PlayerSaving.Instance.GetLastSceneName();
      if (sceneName == "")
      {
         sceneName = "1F";
      }

      //todo
      //更好的场景切换
      UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
      SaveSystemManager.Instance.LoadGame();
   }
}
