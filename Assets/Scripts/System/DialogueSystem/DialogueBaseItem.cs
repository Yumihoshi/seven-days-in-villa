using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue System/Dialogue BaseItem/Default")]
public class DialogueBaseItem : ScriptableObject
{
    public List<SingleDialogueElement> AllElements;
    
}


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