using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameState:cjr.Single.SingleMon<GameState>
{
   [SerializeField] private int NowDays;


   [SerializeField] private GameStateSlot currentStateSlot;


   [SerializeField] private GameStateSlot slot0Test;
   [SerializeField] private GameStateSlot slot1Test;



   public int GetGameState()
   {
      
      
      return currentStateSlot.GetState();
   }
   
   public GameStateSlot CurrentStateSlot
   {
      get { return currentStateSlot; }
   }
   
   public void SwitchStateSlot(GameStateSlot newStateSlot)
   {
      currentStateSlot?.onExit();
      currentStateSlot = newStateSlot;
      currentStateSlot?.onEnter();
   }

   public void SwitchStateSlot(int newState)
   {
       
      SwitchStateSlot(ResourceLoader.Instance.LoadSO<GameStateSlot>(
         ConstVariable.GameStateSo+newState.ToString()));
      SaveSystemManager.Instance.SaveGameState();

   }

   public void LoadState()
   {
    
      int state=SaveSystemManager.Instance.LoadGameState();
      
      //todo
      state=SaveSystemManager.Instance.LoadGameState();
      
      SwitchStateSlot(ResourceLoader.Instance.LoadSO<GameStateSlot>(
         ConstVariable.GameStateSo+state.ToString()));
      
      
   }
   
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

   [Button("test0 set")]
   public void test0()
   {
      Debug.LogWarning("test0 set");
      SwitchStateSlot(slot0Test);
   }

   [Button("test1 set")]
   public void test1()
   {
      SwitchStateSlot(slot1Test);
   }
   
}
