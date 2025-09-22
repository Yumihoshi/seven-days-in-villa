using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot1",fileName = "slot1")]
public class GameStateslot1 : GameStateSlot
{
    [SerializeField] int EnteredRoom = 0;
    public override void Step()
    {
        base.Step();
        EnteredRoom++;
        Debug.LogWarning(EnteredRoom);
    }
}
