using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本的对话单元，有对话语句构成
/// </summary>
[CreateAssetMenu(menuName = "Dialogue System/Dialogue BaseItem/Default")]
public class DialogueBaseItem : ScriptableObject
{
    public List<SingleDialogueElement> AllElements;
    
}

/// <summary>
/// 基本对话语句
/// 包含内容，说话者
/// 精灵图等
/// </summary>
[Serializable]
public struct SingleDialogueElement
{
    [TextArea(3,6)]
    public string text;
    public Speakers Speaker;
    public Sprite Mysprite;
    public Sprite NextSprite;
    public SpeakerType SpeakerType;

    public void FindSprite()
    {
        //todo
        //根据说话的名字查找对应的sprite
    }
}
[Serializable]
public enum Speakers
{
    小A,
    小B,
    小C,
    小D,
    小E,
    小G,
}

public enum SpeakerType
{
    Speaker1,
    Speaker2,
}