using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : cjr.Single.SingleMon<RoomManager>
{
  [SerializeField] private List<BaseRoom> rooms = new List<BaseRoom>();


  public StartRoom GetStartRoom()
  {
    return rooms[0] as StartRoom;
  }

  public BaseRoom GetRoomByIndex(int roomID)
  {
    if(roomID < 0 || roomID >= rooms.Count)
      return null;
    return rooms[roomID];
  }
  
  
  
  
}
