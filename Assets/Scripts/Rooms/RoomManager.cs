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
        /// <summary>
    /// 精确提取Sprite所有轮廓（防止漏面），返回轮廓点集合（以Vector3.positiveInfinity分隔），可指定采样疏密
    /// </summary>
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

        // 标记已访问的边界点
        bool[,] visited = new bool[w, h];

        // Moore邻域
        int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
        int[] dy = { 0, -1, -1, -1, 0, 1, 1, 1 };

        // 判断是否是边界点
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

        List<Vector3> worldPoints = new List<Vector3>();

        // 彻底遍历每个未访问的边界点，防止漏面
        for (int y = 1; y < h - 1; y++)
        {
            for (int x = 1; x < w - 1; x++)
            {
                if (visited[x, y]) continue;
                if (!IsEdge(x, y)) continue;

                // 新轮廓
                List<Vector2Int> contourPix = new List<Vector2Int>();
                Vector2Int start = new Vector2Int(x, y);
                Vector2Int curr = start;
                int prevDir = 0;
                bool[,] contourVisited = new bool[w, h];

                do
                {
                    contourPix.Add(curr);
                    visited[curr.x, curr.y] = true;
                    contourVisited[curr.x, curr.y] = true;

                    bool found = false;
                    // 从上一次方向开始顺序查找
                    for (int i = 0; i < 8; i++)
                    {
                        int dir = (prevDir + i) % 8;
                        int nx = curr.x + dx[dir];
                        int ny = curr.y + dy[dir];
                        if (nx < 1 || nx >= w - 1 || ny < 1 || ny >= h - 1) continue;
                        if (contourVisited[nx, ny]) continue;
                        if (IsEdge(nx, ny))
                        {
                            curr = new Vector2Int(nx, ny);
                            prevDir = dir;
                            found = true;
                            break;
                        }
                    }
                    if (!found) break;
                } while (curr != start);

                // 按采样步长简化点集
                for (int i = 0; i < contourPix.Count; i += samplingStep)
                {
                    Vector2Int pix = contourPix[i];
                    Vector2 localPos = new Vector2(
                        (pix.x - sprite.pivot.x) / sprite.pixelsPerUnit,
                        (pix.y - sprite.pivot.y) / sprite.pixelsPerUnit
                    );
                    Vector3 worldPos = sprender.transform.TransformPoint(localPos);
                    worldPoints.Add(worldPos);
                }
                // 补最后一个点，闭合
                if (contourPix.Count > 0)
                {
                    Vector2Int pix = contourPix[0];
                    Vector2 localPos = new Vector2(
                        (pix.x - sprite.pivot.x) / sprite.pixelsPerUnit,
                        (pix.y - sprite.pivot.y) / sprite.pixelsPerUnit
                    );
                    Vector3 worldPos = sprender.transform.TransformPoint(localPos);
                    worldPoints.Add(worldPos);
                }

                // 添加分隔点
                worldPoints.Add(Vector3.positiveInfinity);
            }
        }

        // 去掉最后一个分隔点
        if (worldPoints.Count > 0 && worldPoints[worldPoints.Count - 1] == Vector3.positiveInfinity)
            worldPoints.RemoveAt(worldPoints.Count - 1);

        return worldPoints;
    }
        
        
}