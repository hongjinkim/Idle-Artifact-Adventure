public abstract class PassiveSkill
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int MaxLevel { get; protected set; }
    public int CurrentLevel { get; private set; }

    protected PassiveSkill(string name, string description, int maxLevel)
    {
        Name = name;
        Description = description;
        MaxLevel = maxLevel;
        CurrentLevel = 0;
    }

    public virtual bool LevelUp()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
            return true;
        }
        return false;
    }

    //public abstract void ApplyEffect(PlayerStat playerStat);
}