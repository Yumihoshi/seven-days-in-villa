using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSlot : ScriptableObject
{
   public virtual void update()
   {
      
   }

   public virtual int GetState()
   {
       return 0;
   }


   public virtual void Step()
   {
      
   }
   public virtual void fixedUpdate()
   {
      
   }

   public virtual void lateUpdate()
   {
      
   }

   public virtual void onEnter()
   {
       SaveSystemManager.Instance.SaveGameState();
   }


   public virtual void onExit()
   {
     
   }
   
}
