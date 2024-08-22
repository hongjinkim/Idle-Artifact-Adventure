using System.Collections.Generic;

public class HolyGrace : PlayerSkill
{
    private int[] defenseBonus = { 2, 4, 6, 8, 10 };
    private int[] magicResistanceBonus = { 2, 4, 6, 8, 10 };

    public HolyGrace() : base(
        "신성한 은혜",
        "사제의 빛은 근처 영웅들을 강화시킵니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetDefenseBonus() => defenseBonus[Level - 1];
    public int GetMagicResistanceBonus() => magicResistanceBonus[Level - 1];

    public struct BuffInfo
    {
        public int DefenseBonus;
        public int MagicResistanceBonus;
    }

    private Dictionary<Hero, BuffInfo> appliedBuffs = new Dictionary<Hero, BuffInfo>();

    public void ApplyEffect(Hero hero)
    {
        int defenseBonus = GetDefenseBonus();
        int magicResistanceBonus = GetMagicResistanceBonus();

        hero.info.defense += defenseBonus;
        hero.info.magicResistance += magicResistanceBonus;

        appliedBuffs[hero] = new BuffInfo { DefenseBonus = defenseBonus, MagicResistanceBonus = magicResistanceBonus };
    }

    public void RemoveEffect(Hero hero)
    {
        if (appliedBuffs.TryGetValue(hero, out BuffInfo buffInfo))
        {
            hero.info.defense -= buffInfo.DefenseBonus;
            hero.info.magicResistance -= buffInfo.MagicResistanceBonus;
            appliedBuffs.Remove(hero);
        }
    }

    public void RemoveAllEffects()
    {
        foreach (var pair in appliedBuffs)
        {
            Hero hero = pair.Key;
            BuffInfo buffInfo = pair.Value;
            hero.info.defense -= buffInfo.DefenseBonus;
            hero.info.magicResistance -= buffInfo.MagicResistanceBonus;
        }
        appliedBuffs.Clear();
    }
    public override void UseSkill(Hero caster) { }
}