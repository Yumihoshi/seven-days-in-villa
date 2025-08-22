using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChosenPanel : MonoBehaviour
{
   public Transform choosing;
   public Transform chosed;

   public bool isChosen = false;
   public int choice;
   private void Start()
   {
      choosing.gameObject.SetActive(true);
      chosed.gameObject.SetActive(false);
      choice = 0;
      doChosing(choice);
   }

   void CleanChildren(Transform holder)
   {
      for (int i = 0; i < holder.childCount; i++)
      {
         Transform child = holder.GetChild(i);
         child.gameObject.SetActive(false);
      }
   }
   void doChosing(int choice)
   {
      if(isChosen)
         return;
      CleanChildren(choosing);
      CleanChildren(chosed);
      choosing.GetChild(choice).gameObject.SetActive(true);
   }

   void dochosed()
   {
      CleanChildren(choosing);
      CleanChildren(chosed);
      choosing.gameObject.SetActive(false);
      chosed.gameObject.SetActive(true);
      chosed.GetChild(choice).gameObject.SetActive(true);
      isChosen = true;
   }
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A))
      {
         choice--;
      }

      if (Input.GetKeyDown(KeyCode.D))
      {
         choice++;
      }

      if (choice < 0)
      {
         choice +=2;
      }
      choice %= 2;
      doChosing(choice);
      if (Input.GetKeyDown(KeyCode.Return))
      {
        dochosed();
      }
   }
}
