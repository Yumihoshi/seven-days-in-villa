using UnityEngine;
using UnityEngine.UI;

public class ScrollingFilm : MonoBehaviour
{
    [Header("胶卷横向总格数")]
    public int totalFrames = 24;

    [Header("播放速度：格/秒")]
    public float framesPerSecond = 12f;

    // 内部缓存
    bool isUI;          // 是 Image 还是 SpriteRenderer
    SpriteRenderer spR;
    Image img;
    Material mat;       // 运行时材质实例
    float invTotalFrames;

    void Awake()
    {
        spR = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();

        if (img != null)
        {
            isUI = true;
            // Image 的材质实例
            mat = img.material;
        }
        else if (spR != null)
        {
            isUI = false;
            // SpriteRenderer 的材质实例
            mat = spR.material;
        }
        else
        {
            Debug.LogError("请把脚本挂在带 SpriteRenderer 或 Image 的物体上！", this);
            enabled = false;
            return;
        }

        invTotalFrames = 1f / totalFrames;
    }

    void Update()
    {
        int index = Mathf.FloorToInt(Time.time * framesPerSecond) % totalFrames;
        Vector2 offset = new Vector2(index * invTotalFrames, 0);

        // 通用：改 mainTextureOffset
        mat.mainTextureOffset = offset;
    }
}