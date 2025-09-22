using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yumihoshi.Task;



[Serializable]
public enum RoomType
{
    Lobby,
    Storage,
    Corridor,
    Entrance_hall,
    PlayingRoom,
    Kitchen,
    PaintingRoom,
    StartRoom,
    
}

public class BaseRoom : MonoBehaviour
{
    public RoomType roomName;
    
    
}
