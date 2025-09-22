using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour挂载版的GameObjectFactory，提供高效的游戏物体生成和销毁管理。
/// 支持对象池，也支持非工厂创建物体的销毁。推荐挂载到常驻场景的单例物体上。
/// </summary>
public class GameObjectFactory : cjr.Single.SingleMon<GameObjectFactory>
{
    public static GameObjectFactory Instance { get; private set; }

    // 跟踪已创建的物体
    private readonly HashSet<GameObject> _createdObjects = new HashSet<GameObject>();

    // 对象池：按Prefab分组
    private readonly Dictionary<GameObject, Queue<GameObject>> _pool = new Dictionary<GameObject, Queue<GameObject>>();

    [Header("是否启用对象池")]
    public bool usePool = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 创建游戏物体
    /// </summary>
    public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj = null;
        if (usePool && prefab != null)
        {
            if (!_pool.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                _pool[prefab] = queue;
            }
            if (queue.Count > 0)
            {
                obj = queue.Dequeue();
                obj.transform.SetPositionAndRotation(position, rotation);
                obj.transform.SetParent(parent, false);
                obj.SetActive(true);
            }
        }
        if (obj == null)
        {
            obj = Instantiate(prefab, position, rotation, parent);
        }
        _createdObjects.Add(obj);
        return obj;
    }

    /// <summary>
    /// 销毁游戏物体（自动回收或销毁，无论是不是Factory创建的）
    /// </summary>
    public void DestroyObject(GameObject obj)
    {
        if (obj == null) return;
        if (_createdObjects.Contains(obj))
        {
            _createdObjects.Remove(obj);
            if (usePool)
            {
                obj.SetActive(false);
                obj.transform.SetParent(null); // 可选：移出场景树
                // 回收到池中（需知道Prefab类型）
                foreach (var kv in _pool)
                {
                    if (obj.name.StartsWith(kv.Key.name))
                    {
                        kv.Value.Enqueue(obj);
                        return;
                    }
                }
            }
            Destroy(obj);
        }
        else
        {
            // 非Factory创建的物体也能销毁
            Destroy(obj);
        }
    }

    /// <summary>
    /// 清理所有已创建的物体（通常用于场景切换等）
    /// </summary>
    public void Clear()
    {
        foreach (var obj in _createdObjects)
        {
            Destroy(obj);
        }
        _createdObjects.Clear();
        foreach (var queue in _pool.Values)
        {
            while (queue.Count > 0)
            {
                var pooledObj = queue.Dequeue();
                Destroy(pooledObj);
            }
        }
        _pool.Clear();
    }
}