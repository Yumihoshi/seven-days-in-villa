using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameState/slot3",fileName = "slot3")]
public class GameStateSlot3 : GameStateSlot
{
    public override int GetState()
    {
        return 3;
    }
}
