using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTrait : Trait
{
    private Dictionary<int, (string leftName, Action<Character> leftEffect, string rightName, Action<Character> rightEffect)> traitLevels;

    public MagicTrait() : base(TraitType.Magic, "요술", "마법 능력을 향상시킵니다.", 1, true)
    {
        InitializeTraitLevels();
    }

    private void InitializeTraitLevels()
    {
        traitLevels = new Dictionary<int, (string, Action<Character>, string, Action<Character>)>
        {
            {10, ("에너지 흡수", ApplyEnergyAbsorption, "에너지 사이펀", ApplyEnergySiphon)},
            {20, ("에테르 충격", ApplyEtherShock, "비전 유성", ApplyArcaneMeteor)},
            {30, ("비전 침식", ApplyArcaneErosion, "금지된 주문", ApplyForbiddenSpell)},
            {40, ("마나 정제", ApplyManaRefinement, "힘의 충격", ApplyPowerSurge)}
        };
    }

    public override void ApplyEffect(Character character)
    {
        if (traitLevels.TryGetValue(Level, out var traitEffect))
        {
            if (!character.info.IsTraitApplied(TraitType.Magic, Level, IsLeftTrait))
            {
                if (IsLeftTrait)
                    traitEffect.leftEffect(character);
                else
                    traitEffect.rightEffect(character);

                character.info.AddAppliedTrait(TraitType.Magic, Level, IsLeftTrait);
            }
        }
    }

    private void ApplyEnergyAbsorption(Character character)
    {
        character.OnKill += (target) => {
            if (Vector3.Distance(character.transform.position, target.transform.position) <= character.info.attackRange)
            {
                character.Energy += 60;
            }
        };
    }

    private void ApplyEnergySiphon(Character character)
    {
        character.OnHit += (target) => {
            character.Energy += 18;
        };
    }

    private void ApplyEtherShock(Character character)
    {
        // TODO: Implement magic damage system
        // character.OnHit += (target) => {
        //     if (UnityEngine.Random.value <= 0.1f)
        //     {
        //         float magicDamage = character.info.attackDamage * 0.5f;
        //         target.TakeMagicDamage(character, magicDamage);
        //     }
        // };
    }

    private float arcaneMeteorCooldown = 0f;
    private void ApplyArcaneMeteor(Character character)
    {
        character.OnSkillHit += (target, skillDamage) => {
            if (arcaneMeteorCooldown <= 0)
            {
                float additionalDamage = skillDamage * 0.3f;
                target.TakeDamage(character, additionalDamage);
                arcaneMeteorCooldown = 12f;
            }
        };

        character.OnUpdate += (deltaTime) => {
            if (arcaneMeteorCooldown > 0)
            {
                arcaneMeteorCooldown -= deltaTime;
            }
        };
    }

    private void ApplyArcaneErosion(Character character)
    {
        character.OnSkillHit += (target, skillDamage) => {
            character.StartCoroutineFromTrait(ReduceHealthRegen(target));
        };
    }

    private IEnumerator ReduceHealthRegen(Character target)
    {
        float originalHealthRegen = target.info.healthRegen;
        target.info.healthRegen = Mathf.Max(0, target.info.healthRegen - 25);
        yield return new WaitForSeconds(3f);
        target.info.healthRegen = originalHealthRegen;
    }

    private void ApplyForbiddenSpell(Character character)
    {
        character.SkillDamageMultiplier *= 1.1f;
    }

    private void ApplyManaRefinement(Character character)
    {
        character.OnSkillUse += () => {
            character.Energy += 20;
        };
    }

    private void ApplyPowerSurge(Character character)
    {
        character.OnSkillUse += () => {
            character.StartCoroutineFromTrait(IncreaseAttackSpeed(character));
        };
    }

    private IEnumerator IncreaseAttackSpeed(Character character)
    {
        character.info.attackSpeed += 30;
        yield return new WaitForSeconds(3f);
        character.info.attackSpeed -= 30;
    }
}