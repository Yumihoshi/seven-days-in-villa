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
    public SpriteRenderer spriteRenderer;

    public List<Vector3> MyPolyEdge;
    
    

    private void OnEnable()
    {
        spriteRenderer=transform.GetChild(0).GetComponent<SpriteRenderer>();
        MyPolyEdge=SpriteBoundaryExtractor.GetSpriteAlphaBoundaryWorldPoints(spriteRenderer,3);
        
    }


    public Vector3 GetRomdomPosition()
    {
        return PolygonRandomSampler.GetRandomPointInPolygon(MyPolyEdge);
    }
    
}
