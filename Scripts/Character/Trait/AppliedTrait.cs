using System;

[Serializable]
public class AppliedTrait
{
    public TraitType Type;
    public int Level;
    public bool IsLeft;

    public AppliedTrait(TraitType type, int level, bool isLeft)
    {
        Type = type;
        Level = level;
        IsLeft = isLeft;
    }
}