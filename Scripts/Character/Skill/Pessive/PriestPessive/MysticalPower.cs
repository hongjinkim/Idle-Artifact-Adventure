using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticalPower : PlayerSkill
{
    private int[] energyRegeneration = { 2, 4, 6, 8, 10 };

    public MysticalPower() : base(
        "신비로운 힘",
        "사제 근처 영웅들이 에너지를 1초마다 회복시킵니다.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetEnergyRegeneration() => energyRegeneration[Level - 1];

    public override void UseSkill(Hero caster) { }
}