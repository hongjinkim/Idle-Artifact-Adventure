public class HeavenlyBlessing : PlayerSkill
{
    private float[] hpBonus = { 0.02f, 0.05f, 0.07f, 0.10f, 0.12f };
    private float[] defenseBonus = { 0.05f, 0.10f, 0.15f, 0.20f, 0.25f };

    public HeavenlyBlessing() : base(
        "천국의 축복",
        "흔들리지 않는 믿음으로 인해 Knight는 추가 HP와 방어를 얻습니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    {
    }

    public void ApplyEffect(Knight knight)
    {
        int index = Level - 1;
        float additionalHp = knight.info.hp * hpBonus[index];
        float additionalDefense = knight.info.defense * defenseBonus[index];

        knight.info.hp += (int)additionalHp;
        knight.info.defense += (int)additionalDefense;

        
    }

    public override void UseSkill(Hero caster) { } // 패시브 스킬이므로 비워둡니다.
}