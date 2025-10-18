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
          // 把每个点画出点来
          Debug.DrawRay(pt, Vector3.up * 0.1f, Color.cyan);
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

        // 遍历所有像素找出边界点
        int count = 0;
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (IsEdge(x, y))
                {
                    // 按 samplingStep 采样
                    if (count % samplingStep == 0)
                    {
                        // 转为世界坐标
                        Vector2 localPos = new Vector2(
                            (x - sprite.pivot.x) / sprite.pixelsPerUnit,
                            (y - sprite.pivot.y) / sprite.pixelsPerUnit
                        );
                        Vector3 worldPos = sprender.transform.TransformPoint(localPos);
                        worldPoints.Add(worldPos);
                    }
                    count++;
                }
            }
        }

        return worldPoints;
    }
}



public static class PolygonRandomSampler
{
    /// <summary>
    /// 从轮廓点集合（含Vector3.positiveInfinity分隔符）随机采样多边形内部一点，只使用第一个轮廓
    /// </summary>
    public static Vector3 GetRandomPointInPolygon(List<Vector3> polygon)
    {
        // 只取第一个轮廓点集合，过滤所有无穷分隔符和无穷点
        List<Vector2> poly2D = new List<Vector2>();
        float zValue = 0f;
        foreach (var pt in polygon)
        {
            if (pt == Vector3.positiveInfinity || float.IsInfinity(pt.x) || float.IsInfinity(pt.y)) break;
            poly2D.Add(new Vector2(pt.x, pt.y));
            zValue = pt.z;
        }

        if (poly2D.Count < 3)
        {
            Debug.LogError($"PolygonRandomSampler: 输入轮廓点不足, 有效点数量={poly2D.Count}");
            return Vector3.zero;
        }

        // 计算AABB
        float minX = poly2D[0].x, maxX = poly2D[0].x;
        float minY = poly2D[0].y, maxY = poly2D[0].y;
        foreach (var p in poly2D)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        // 随机采样
        for (int attempt = 0; attempt < 1000; attempt++)
        {
            float rx = Random.Range(minX, maxX);
            float ry = Random.Range(minY, maxY);
            Vector2 testPt = new Vector2(rx, ry);
            if (IsPointInPolygon(testPt, poly2D))
                return new Vector3(rx, ry, zValue);
        }

        // 没采到就返回重心
        Vector2 center = Vector2.zero;
        foreach (var p in poly2D) center += p;
        center /= poly2D.Count;
        return new Vector3(center.x, center.y, zValue);
    }

    // 射线法判断点是否在多边形内部
    static bool IsPointInPolygon(Vector2 pt, List<Vector2> poly)
    {
        int n = poly.Count;
        bool inside = false;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            if (((poly[i].y > pt.y) != (poly[j].y > pt.y)) &&
                 (pt.x < (poly[j].x - poly[i].x) * (pt.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x))
                inside = !inside;
        }
        return inside;
    }
}