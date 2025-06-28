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


   IEnumerator PointDownCorotinue()
   {
      SaveSystemManager.Instance.SaveGame();
      yield return new WaitForSeconds(0.2f);
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
   public void PointDown()
   {

      StartCoroutine(PointDownCorotinue());

   }
}
