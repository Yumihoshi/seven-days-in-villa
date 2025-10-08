using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Street : MonoBehaviour
{

    [SerializeField] private BoxCollider2D box;
    // Start is called before the first frame update

    [SerializeField] private int DecrationNums = 4;

    private const string decrationPath = "Prefabs/Maze/";
    
    // 每个 BoxCollider2D 单独记录上一次 X
    private  Dictionary<int, float> s_lastXMap = new Dictionary<int, float>();

    // 默认最小间隔（可按需要改）
    public  float DefaultMinInterval = 1.5f;

    // 最大尝试次数，防止极端情况死循环
    private const int MAX_TRY = 12;

    void GenerateRandomDec()
    {
        for (int i = 0; i < DecrationNums; i++)
        {
            Vector3 pos = GetRandomPointInside(box);
            int type = Random.Range(1, 4);
            Debug.LogWarning(decrationPath + $"eye{type}");
            
            var dec= ResourceLoader.
                Instance.LoadObject(decrationPath + $"eye{type}",transform);
            
            dec.transform.position = pos;
            
            
        }
    }
    
    
    
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        
    }

    void Start()
    {
      GenerateRandomDec();

    }

    /// <summary>
    /// 获取 BoxCollider2D 世界坐标系下的区域内随机点
    /// </summary>
    /// <param name="box">目标碰撞器</param>
    /// <returns>世界坐标系下的随机点</returns>
    public  Vector3 GetRandomPointInside(BoxCollider2D box, float? minInterval = null)
    {
        if (box == null)
        {
            Debug.LogError("BoxCollider2D 为 null");
            return Vector3.zero;
        }

        float interval = minInterval ?? DefaultMinInterval;
        int key = box.GetInstanceID();

        // 如果还没记录，先塞一个非法值，保证第一次必过
        if (!s_lastXMap.TryGetValue(key, out float lastX))
            lastX = Mathf.Infinity;

        Vector3 worldPoint = Vector3.zero;
        bool ok = false;

        // 先尝试随机抽
        for (int t = 0; t < MAX_TRY && !ok; t++)
        {
            worldPoint = RawRandomPoint(box);
            if (Mathf.Abs(worldPoint.x - lastX) >= interval)
                ok = true;
        }

        // 如果一直抽不到，就强制平移
        if (!ok)
        {
            worldPoint = RawRandomPoint(box);
            float dir = worldPoint.x < lastX ? -1f : 1f;
            worldPoint.x = lastX + dir * interval;
            // 平移后可能超出碰撞器范围，再 Clamp 一下
            Bounds b = box.bounds;
            worldPoint.x = Mathf.Clamp(worldPoint.x, b.min.x, b.max.x);
        }

        // 记录本次 X
        s_lastXMap[key] = worldPoint.x;
        return worldPoint;
    }
    
    private  Vector3 RawRandomPoint(BoxCollider2D box)
    {
        Vector2 size = box.size;
        Vector2 local = new Vector2(
            Random.Range(-size.x * 0.5f, size.x * 0.5f),
            Random.Range(-size.y * 0.5f, size.y * 0.5f)
        ) + box.offset;

        Vector3 world = box.transform.TransformPoint(local);
        world.z = box.bounds.center.z;
        return world;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
