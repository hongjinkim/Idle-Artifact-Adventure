using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedBow : PlayerSkill
{
    private int[] trueDamageBonus = { 4, 8, 12, 16, 20 };

    public EnhancedBow() : base(
        "강화된 활",
        "강력한 활은 화살에 의한 피해를 크게 증가시킵니다. 레인저는 모든 공격마다 추가 고정피해를 입힙니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetTrueDamageBonus()
    {
        return trueDamageBonus[Level - 1];
    }
    public override void UseSkill(Hero caster)
    {
        // 패시브 스킬이므로 실제로 아무 것도 하지 않습니다.
    }
}