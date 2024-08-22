using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Modifier
{
    public MagicStat id;
    public int level;

    public Modifier()
    {
    }

    public Modifier(MagicStat id, int level)
    {
        this.id = id;
        this.level = level;
    }
}