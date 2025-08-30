using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState:cjr.Single.SingleMon<GameState>
{
   [SerializeField] private int NowDays;
   
   [SerializeField] GameStateSlot currentStateSlot;


   private void Update()
   {
      currentStateSlot?.update();
   }

   private void FixedUpdate()
   {
      currentStateSlot?.fixedUpdate();
      
   }


   private void LateUpdate()
   {
      currentStateSlot?.lateUpdate();
   }
}
