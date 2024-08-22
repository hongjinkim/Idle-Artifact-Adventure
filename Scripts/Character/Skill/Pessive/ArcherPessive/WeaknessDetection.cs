using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessDetection : PlayerSkill
{
    private int[] defensePenetrationBonus = { 2, 4, 6, 8, 10 };

    public WeaknessDetection() : base(
        "약점 포착",
        "Archer는 적의 약점을 찾고 방어를 감소시킵니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetDefensePenetrationBonus()
    {
        return defensePenetrationBonus[Level - 1];
    }
    public override void UseSkill(Hero caster)
    {
        // 패시브 스킬이므로 실제로 아무 것도 하지 않습니다.
    }
}