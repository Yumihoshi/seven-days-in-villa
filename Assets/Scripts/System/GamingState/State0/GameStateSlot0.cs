using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState/slot0",fileName = "slot0")]
public class GameStateSlot0 : GameStateSlot
{

    //todo

    
    public new const int state = 0;
    
    [SerializeField] private int maxX;
    [SerializeField] private int nowStep;


    [SerializeField] private List<GameObject> entered;
    public override void Step()
    {
        base.Step();
        nowStep++;
        if (nowStep >= maxX)
        {
            GameState.Instance.test1();
        }
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
        nowStep = 0;
        Transform actionPoint = RoomManager.Instance.GetStartRoom().GetActionPoint();
        
        PlayerAction.Instance.transform.position = actionPoint.position;
        VcmManager.Instance.GetClosestConfiner();
        entered = new List<GameObject>();
        cjr.Scence.SceneManager.Instance.FainOut(0, 0.1f);
        for (int i = 0; i < RoomManager.Instance.GetRoomsCount(); i++)
        {
            var game = (ResourceLoader.Instance.LoadObject(ConstVariable.WalkedRoomCheck));
            game.transform.position= RoomManager.Instance.GetRoomByIndex(i).transform.position;
            game.SetActive(RoomManager.Instance.GetRoomByIndex(i).gameObject.activeSelf);
            game.name = RoomManager.Instance.GetRoomByIndex(i).name;
            entered.Add(game);
        }
        //todo
        maxX = RoomManager.Instance.GetRoomsCount()-4;
    }
    
    public override void onExit()
    {
        base.onExit();
        CoroutineFactory.Instance.RunCoroutine(reset());
    }

    IEnumerator reset()
    {

        for (int i = 0; i < RoomManager.Instance.GetRoomsCount(); i++)
        {
            GameObjectFactory.Instance.DestroyObject(entered[i]);
            yield return null;
        }
    }
}
