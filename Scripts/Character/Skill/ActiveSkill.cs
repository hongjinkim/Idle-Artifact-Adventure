public abstract class ActiveSkill : Skill
{
    public float Cooldown { get; protected set; }
    public float EnergyCost { get; protected set; }

    protected ActiveSkill(string name, string description, int maxLevel, float cooldown, float energyCost,int unlockLevel)
        : base(name, description, maxLevel, SkillType.Active, unlockLevel)
    {
        Cooldown = cooldown;
        EnergyCost = energyCost;
    }

    public abstract void Use(PlayerStat playerStat);
}