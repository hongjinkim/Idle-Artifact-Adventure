using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Trait
{
    public TraitType Type { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Level { get; private set; }
    public bool IsLeftTrait { get; private set; }

    protected Trait(TraitType type, string name, string description, int level, bool isLeftTrait)
    {
        Type = type;
        Name = name;
        Description = description;
        Level = level;
        IsLeftTrait = isLeftTrait;
    }

    public abstract void ApplyEffect(Character character);

    protected void SetLevel(int level)
    {
        Level = level;
    }

    protected void SetIsLeftTrait(bool isLeftTrait)
    {
        IsLeftTrait = isLeftTrait;
    }
    public virtual void ChooseTrait(int level, bool isLeftTrait)
    {
        Level = level;
        IsLeftTrait = isLeftTrait;
    }
}