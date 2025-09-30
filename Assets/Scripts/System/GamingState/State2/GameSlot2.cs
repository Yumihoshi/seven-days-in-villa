using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot2",fileName = "slot2")]
public class GameStateSlot2 : GameStateSlot
{
   public override void onEnter()
   {
      base.onEnter();
      
   }

   public override int GetState()
   {
      return 2;
   }
}
