using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public List<Basic> basic;
    public List<Magic> magic;
    

    public Stat()
    {
    }

    public Stat(List<Basic> basic)
    {
        this.basic = basic;
        this.magic = new List<Magic>();
    }

}
[Serializable]
public struct Basic
{
    public BasicStat id;
    public int value;

    public int minValue;
    public int maxValue;
}
[Serializable]
public struct Magic
{
    public MagicStat id;
    public int value;

    //public int minValue;
    //public int maxValue;
}