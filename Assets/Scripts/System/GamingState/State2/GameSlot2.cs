using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot2",fileName = "slot2")]
public class GameStateSlot2 : GameStateSlot
{


   [SerializeField] private GameObject outOfHouse;
   
   [SerializeField] Vector3 outOfHousePosition;
   
   public override void onEnter()
   {
      base.onEnter();
      outOfHouse = ResourceLoader.Instance.LoadObject("Prefabs/GameState/Slot2/OutOfHouse");
      outOfHouse.transform.position = outOfHousePosition;
   }


   public override void onExit()
   {
      base.onExit();
      GameObjectFactory.Instance.DestroyObject(outOfHouse);
   }

   public override int GetState()
   {
      return 2;
   }
}
