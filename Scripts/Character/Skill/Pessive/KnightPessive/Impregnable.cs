public class Impregnable : PlayerSkill
{
    private int[] damageReductionBonus = { 3, 6, 9, 12, 15 };

    public Impregnable() : base(
        "난공불락",
        "현재 HP가 60%보다 높을때 받는 피해가 줄어듭니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    {
    }

    public int GetDamageReductionBonus()
    {
        return damageReductionBonus[Level - 1];
    }

    public override void UseSkill(Hero caster) { } // 패시브 스킬이므로 비워둡니다.
}