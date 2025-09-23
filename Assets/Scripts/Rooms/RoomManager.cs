using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomManager : cjr.Single.SingleMon<RoomManager>
{
  [SerializeField] private List<BaseRoom> rooms = new List<BaseRoom>();

  public int GetRoomsCount()
  {
    return rooms.Count;
  }

  public StartRoom GetStartRoom()
  {
    return rooms[0] as StartRoom;
  }


  public BaseRoom GetClosestRoom(Vector3 position)
  {
    float closestDistance = Mathf.Infinity;
    BaseRoom closestRoom = null;

    for (int i = 0; i < rooms.Count; i++)
    {
      if(!rooms[i].gameObject.activeSelf)
        continue;
      float distance = Vector3.Distance(position, rooms[i].transform.position);
      if (distance < closestDistance)
      {
        closestDistance = distance;
        closestRoom = rooms[i];
      }
    }
    
    
    return closestRoom;
  }
  
  public BaseRoom GetRoomByIndex(int roomID)
  {
    if(roomID < 0 || roomID >= rooms.Count)
      return null;
    return rooms[roomID];
  }

  //todo
    [SerializeField] List<Vector3> roomPositions = new List<Vector3>();


    private void Update()
    {
        Vector3? last = null;
        foreach (var pt in roomPositions)
        {
            if (pt == Vector3.positiveInfinity)
            {
                last = null; // 新轮廓
                continue;
            }
            if (last.HasValue)
                Debug.DrawLine(last.Value, pt, Color.cyan);
            last = pt;
        }
    }

    [SerializeField] private int ee = 2;
    [Button("test")]
    public void test()
    {
        roomPositions = SpriteBoundaryExtractor.GetSpriteAlphaBoundaryWorldPoints(
            rooms[ee].spriteRenderer,3);
        
    }

  
}




public static class SpriteBoundaryExtractor
{
    // 增加一个参数 samplingStep 用于控制采样疏密
     public static List<Vector3> GetSpriteAlphaBoundaryWorldPoints(SpriteRenderer sprender, int samplingStep = 1)
    {
        Sprite sprite = sprender.sprite;
        Texture2D tex = sprite.texture;
        Rect spriteRect = sprite.textureRect;
        int w = (int)spriteRect.width;
        int h = (int)spriteRect.height;
        Color[] pixels = tex.GetPixels(
            (int)spriteRect.x, (int)spriteRect.y,
            w, h);

        bool[,] visited = new bool[w, h];
        List<Vector3> worldPoints = new List<Vector3>();

        // 边界检测辅助
        bool IsEdge(int x, int y)
        {
            if (pixels[y * w + x].a <= 0.1f) return false;
            for (int ny = -1; ny <= 1; ny++)
                for (int nx = -1; nx <= 1; nx++)
                {
                    if (nx == 0 && ny == 0) continue;
                    int xx = x + nx, yy = y + ny;
                    if (xx < 0 || xx >= w || yy < 0 || yy >= h) return true;
                    if (pixels[yy * w + xx].a <= 0.1f) return true;
                }
            return false;
        }

        // Moore邻域
        int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
        int[] dy = { 0, -1, -1, -1, 0, 1, 1, 1 };

        // 查找下一个未访问过的边界点，开启新轮廓追踪
        for (int y = 1; y < h - 1; y++)
        {
            for (int x = 1; x < w - 1; x++)
            {
                if (visited[x, y]) continue;
                if (IsEdge(x, y))
                {
                    // 新轮廓起点
                    Vector2Int start = new Vector2Int(x, y);
                    Vector2Int curr = start;
                    int dir = 0;
                    List<Vector3> contour = new List<Vector3>();

                    do
                    {
                        visited[curr.x, curr.y] = true;
                        // 像素坐标转本地坐标
                        Vector2 localPos = new Vector2(
                            (curr.x - sprite.pivot.x) / sprite.pixelsPerUnit,
                            (curr.y - sprite.pivot.y) / sprite.pixelsPerUnit
                        );
                        Vector3 worldPos = sprender.transform.TransformPoint(localPos);
                        contour.Add(worldPos);

                        // 按邻域顺序找下一个边界点
                        bool foundNext = false;
                        for (int i = 0; i < 8; i += samplingStep)
                        {
                            int nx = curr.x + dx[i];
                            int ny = curr.y + dy[i];
                            if (nx < 1 || nx >= w - 1 || ny < 1 || ny >= h - 1) continue;
                            if (!visited[nx, ny] && IsEdge(nx, ny))
                            {
                                curr = new Vector2Int(nx, ny);
                                foundNext = true;
                                break;
                            }
                        }
                        if (!foundNext) break;
                    } while (curr != start);

                    // 添加到总集合，并用Infinity分隔
                    worldPoints.AddRange(contour);
                    worldPoints.Add(Vector3.positiveInfinity);
                }
            }
        }

        // 去掉最后一个分隔点
        if (worldPoints.Count > 0 && worldPoints[worldPoints.Count - 1] == Vector3.positiveInfinity)
            worldPoints.RemoveAt(worldPoints.Count - 1);

        return worldPoints;
    }
}