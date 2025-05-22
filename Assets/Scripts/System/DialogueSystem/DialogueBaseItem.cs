using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue System/Dialogue BaseItem/Default")]
public class DialogueBaseItem : ScriptableObject
{
    public List<SingleElement> AllElements;
    
}


[Serializable]
public struct SingleElement
{
    [TextArea(3,6)]
    public string text;
    public string Speaker;
    public Sprite sprite;


    public void FindSprite()
    {
        //todo
        //根据说话的名字查找对应的sprite
    }
}
