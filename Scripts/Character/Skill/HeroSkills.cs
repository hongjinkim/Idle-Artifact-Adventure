using System.Collections.Generic;
using System.Linq;

public abstract class HeroSkills
{
    protected PlayerStat playerStat;
    protected List<Skill> skills = new List<Skill>();
    public int SkillPoints { get; private set; }
    public HeroSkills(PlayerStat stat)
    {
        playerStat = stat;
        InitializeSkills();
    }

    protected abstract void InitializeSkills();

    public void ApplyPassiveEffects()
    {
        foreach (var skill in skills.Where(s => s.Type == SkillType.Passive))
        {
            skill.ApplyEffect(playerStat);
        }
    }

    public void AddSkillPoints(int points)
    {
        SkillPoints += points;
    }

    public bool LevelUpSkill(string skillName)
    {
        var skill = skills.Find(s => s.Name == skillName);
        if (skill != null && skill.IsUnlocked && skill.Level < skill.MaxLevel && SkillPoints > 0)
        {
            if (skill.LevelUp())
            {
                SkillPoints--;
                ApplyPassiveEffects();
                return true;
            }
        }
        return false;
    }

    public void UnlockSkills(int playerLevel)
    {
        foreach (var skill in skills)
        {
            if (!skill.IsUnlocked && playerLevel >= skill.UnlockLevel)
            {
                skill.Unlock();
            }
        }
    }

    public void UseActiveSkill(string skillName)
    {
        var skill = skills.Find(s => s.Name == skillName) as ActiveSkill;
        if (skill != null)
        {
            skill.Use(playerStat);
        }
    }
}