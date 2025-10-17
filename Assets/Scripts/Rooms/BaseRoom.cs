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
    exerciseRoom,
    LibraryRoom
    
}

public class BaseRoom : MonoBehaviour
{
    public RoomType roomName;
    public SpriteRenderer spriteRenderer;

    public List<Vector3> MyPolyEdge;
    
    

    private void OnEnable()
    {
        spriteRenderer=transform.GetChild(0).GetComponent<SpriteRenderer>();
        try
        {
            
            MyPolyEdge=SpriteBoundaryExtractor.GetSpriteAlphaBoundaryWorldPoints(spriteRenderer,3);
        }
        catch (System.Exception ex)  // »ò¼òÐ´Îª catch (Exception ex)
        {
            Debug.LogError(roomName + ": " + ex.Message);
        }
        
    }


    public Vector3 GetRomdomPosition()
    {
        return PolygonRandomSampler.GetRandomPointInPolygon(MyPolyEdge);
    }
    
}
