public enum SkillType
{
    Passive,
    Active
}

public abstract class Skill
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int Level { get; protected set; }
    public int MaxLevel { get; protected set; }
    public SkillType Type { get; protected set; }
    public bool IsUnlocked { get; private set; }
    public int UnlockLevel { get; protected set; }
    protected Skill(string name, string description, int maxLevel, SkillType type, int unlockLevel)
    {
        Name = name;
        Description = description;
        MaxLevel = maxLevel;
        Level = 0;
        Type = type;
        UnlockLevel = unlockLevel;
        IsUnlocked = false;
    }
    public void Unlock()
    {
        IsUnlocked = true;
    }
    public virtual bool LevelUp()
    {
        if (Level < MaxLevel)
        {
            Level++;
            return true;
        }
        return false;
    }

    public abstract void ApplyEffect(PlayerStat playerStat);
}