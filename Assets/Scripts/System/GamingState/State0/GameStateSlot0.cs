using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot0",fileName = "slot0")]
public class GameStateSlot0 : GameStateSlot
{

    [SerializeField] private int EnteredRoom = 0;


    public override void Step()
    {
        base.Step();
        EnteredRoom++;
        Debug.LogWarning(EnteredRoom);
    }

    public override void onEnter()
    {
        base.onEnter();
        
    }

    public override void onExit()
    {
        base.onExit();
    }
}
