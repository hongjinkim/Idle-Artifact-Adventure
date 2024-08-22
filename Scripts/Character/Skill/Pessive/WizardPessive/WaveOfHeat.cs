using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOfHeat : PlayerSkill
{
    private int[] attackSpeedReduction = { 2, 4, 6, 8, 10 };

    public WaveOfHeat() : base(
        "열의 파도",
        "마법사의 신기한 힘은 장벽을 형성하여 인근 적의 공격 속도를 줄입니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetAttackSpeedReduction() => attackSpeedReduction[Level - 1];

    public override void UseSkill(Hero caster) { } // 패시브 스킬이므로 비워둡니다.
}