using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marksmanship : PlayerSkill
{
    private int[] attackSpeedBonus = { 3, 6, 9, 12, 15 };

    public Marksmanship() : base(
        "사격술",
        "Archer는 타의 추종을 불허하는 공격 속도를 가지고 있습니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetAttackSpeedBonus()
    {
        return attackSpeedBonus[Level - 1];
    }
    public override void UseSkill(Hero caster)
    {
        // 패시브 스킬이므로 실제로 아무 것도 하지 않습니다.
    }
}