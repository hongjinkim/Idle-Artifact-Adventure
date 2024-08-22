using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticResonance : PlayerSkill
{
    private float[] skillDamageAmplification = { 0.05f, 0.08f, 0.11f, 0.15f, 0.20f };

    public MysticResonance() : base(
        "신기한 공명",
        "마법사의 액티브 스킬 데미지를 강화시킵니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public float GetSkillDamageAmplification() => skillDamageAmplification[Level - 1];

    public override void UseSkill(Hero caster) { } // 패시브 스킬이므로 비워둡니다.
}