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
    
    

    // 放在类内部
    private static readonly Dictionary<RoomType, string> ChineseNames = new Dictionary<RoomType, string>
    {
        { RoomType.Lobby,         "大厅" },
        { RoomType.Storage,       "储藏室" },
        { RoomType.Corridor,      "走廊" },
        { RoomType.Entrance_hall, "门厅" },
        { RoomType.PlayingRoom,   "游戏室" },
        { RoomType.Kitchen,       "厨房" },
        { RoomType.PaintingRoom,  "画室" },
        { RoomType.StartRoom,     "起始房间" },
        { RoomType.exerciseRoom,  "健身房" },
        { RoomType.LibraryRoom,   "图书馆" }
    };

    /// <summary>
    /// 返回当前房间的中文名字
    /// </summary>
    public string GetChineseRoomName()
    {
        return ChineseNames.TryGetValue(roomName, out var cn) ? cn : "未知房间";
    }
    
    private void OnEnable()
    {
        spriteRenderer=transform.GetChild(0).GetComponent<SpriteRenderer>();
        try
        {
            
            MyPolyEdge=SpriteBoundaryExtractor.GetSpriteAlphaBoundaryWorldPoints(spriteRenderer,3);
        }
        catch (System.Exception ex)  // 或简写为 catch (Exception ex)
        {
            Debug.LogError(roomName + ": " + ex.Message);
        }
        
    }


    public Vector3 GetRomdomPosition()
    {
        return PolygonRandomSampler.GetRandomPointInPolygon(MyPolyEdge);
    }
    
}
