using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot1",fileName = "slot1")]
public class GameStateslot1 : GameStateSlot
{
    
    
    public new const int state = 1;
    public override void onEnter()
    {
        base.onEnter();
        Debug.LogWarning("In slot1 ");
    }

    public override int GetState()
    {
        return 1;
    }

    public override void Step()
    {
        base.Step();
    }
}
