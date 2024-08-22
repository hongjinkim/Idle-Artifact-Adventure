using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DazzlingLight : PlayerSkill
{
    private int[] defenseReduction = { 2, 4, 6, 8, 10 };
    private int[] magicResistanceReduction = { 2, 4, 6, 8, 10 };

    public DazzlingLight() : base(
        "´«ºÎ½Å ºû",
        "»çÁ¦ÀÇ ºûÀº ±ÙÃ³ÀÇ ÀûÀ» ¾àÈ­½ÃÅµ´Ï´Ù.",
        5,
        new int[] { 0, 0, 0, 0, 0 },
        new float[] { 0, 0, 0, 0, 0 }
    )
    { }

    public int GetDefenseReduction() => defenseReduction[Level - 1];
    public int GetMagicResistanceReduction() => magicResistanceReduction[Level - 1];

    public override void UseSkill(Hero caster) { }
}