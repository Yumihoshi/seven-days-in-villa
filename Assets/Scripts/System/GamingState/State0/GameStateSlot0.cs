using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot0",fileName = "slot0")]
public class GameStateSlot0 : GameStateSlot
{

    //todo
 


    public override void Step()
    {
        base.Step();
    }

    public override void onEnter()
    {
        base.onEnter();
        Debug.LogWarning("now game state 0");
        CoroutineFactory.Instance.RunCoroutine(init());
      
    }

    IEnumerator init()
    {
        yield return null;
        yield return null;
        cjr.Scence.SceneManager.Instance.FainOut(1, 0.1f);

        Transform actionPoint = RoomManager.Instance.GetStartRoom().GetActionPoint();
        
        PlayerAction.Instance.transform.position = actionPoint.position;
        VcmManager.Instance.GetClosestConfiner();
        
        cjr.Scence.SceneManager.Instance.FainOut(0, 0.1f);
    }
    
    public override void onExit()
    {
        base.onExit();
    }
}
