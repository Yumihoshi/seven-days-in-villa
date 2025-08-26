using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerInfo
{
    public Sex Mysex;
    public int Hp;
    public int San;
}

public enum Sex
{
    male = 0,
    female = 1
}
