using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot1",fileName = "slot1")]
public class GameStateslot1 : GameStateSlot
{
    
    [SerializeField] Vector3 StewardPosition;
    private GameObject stewardObject;
    public new const int state = 1;
    public override void onEnter()
    {
        base.onEnter();

        Debug.LogWarning("enter state1");
        stewardObject = ResourceLoader.Instance.LoadObject("Prefabs/Npcs/Steward");
        stewardObject.transform.position = StewardPosition;

    }

    public override int GetState()
    {
        return 1;
    }

    public override void Step()
    {
        base.Step();
        GameState.Instance.SwitchStateSlot(2);
    }
}
