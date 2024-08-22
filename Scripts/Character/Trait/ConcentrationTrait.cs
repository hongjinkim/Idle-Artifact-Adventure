using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentrationTrait : Trait
{
    private Dictionary<int, (string leftName, Action<Character> leftEffect, string rightName, Action<Character> rightEffect)> traitLevels;

    public ConcentrationTrait() : base(TraitType.Concentration, "집중", "집중력을 높여 전투 능력을 향상시킵니다.", 1, true)
    {
        InitializeTraitLevels();
    }

    private void InitializeTraitLevels()
    {
        traitLevels = new Dictionary<int, (string, Action<Character>, string, Action<Character>)>
        {
            {10, ("막을 수 없는 힘", ApplyUnstoppableForce, "잔인한 힘", ApplyCruelForce)},
            {20, ("가죽 벗기기", ApplySkinning, "무자비", ApplyRuthless)},
            {30, ("분쇄", ApplyCrushing, "영혼의 수확", ApplySoulHarvest)},
            {40, ("전투 능력", ApplyCombatProwess, "폭력적인 성격", ApplyViolentNature)}
        };
    }

    public override void ApplyEffect(Character character)
    {
        if (traitLevels.TryGetValue(Level, out var traitEffect))
        {
            if (!character.info.IsTraitApplied(TraitType.Concentration, Level, IsLeftTrait))
            {
                if (IsLeftTrait)
                    traitEffect.leftEffect(character);
                else
                    traitEffect.rightEffect(character);

                character.info.AddAppliedTrait(TraitType.Concentration, Level, IsLeftTrait);
            }
        }
    }

    public void ChooseTrait(int level, bool isLeftTrait)
    {
        if (traitLevels.ContainsKey(level))
        {
            SetLevel(level);
            SetIsLeftTrait(isLeftTrait);
        }
    }

    // 트레이트 효과 구현
    private void ApplyUnstoppableForce(Character character)
    {
        character.info.defensePenetration += 3;
    }

    private void ApplyCruelForce(Character character)
    {
        character.info.damageAmplification += 3;
        Debug.Log(character.name);
        //if (character != null && character.info != null)
        //{
        //    character.info.damageAmplification += 3;
        //}
        //else
        //{
        //    Debug.LogError("Character or character.info is null in ApplyCruelForce");
        //}
    }

    private int skinningHitCount = 0;
    private void ApplySkinning(Character character)
    {
        character.OnHit += (target) => {
            skinningHitCount++;
            if (skinningHitCount >= 5)
            {
                float additionalDamage = character.info.attackDamage * 0.08f;
                target.TakeDamage(character, additionalDamage);
                skinningHitCount = 0;
            }
        };
    }

    private int ruthlessHitCount = 0;
    private float ruthlessTimer = 0f;
    private void ApplyRuthless(Character character)
    {
        character.OnHit += (target) => {
            ruthlessHitCount++;
            if (ruthlessHitCount >= 5)
            {
                character.info.attackSpeed += 20;
                ruthlessTimer = 3f;
                ruthlessHitCount = 0;
            }
        };

        character.OnUpdate += (deltaTime) => {
            if (ruthlessTimer > 0)
            {
                ruthlessTimer -= deltaTime;
                if (ruthlessTimer <= 0)
                {
                    character.info.attackSpeed -= 20;
                }
            }
        };
    }

    private void ApplyCrushing(Character character)
    {
        character.OnHit += (target) => {
            target.info.defense = Mathf.RoundToInt(target.info.defense * 0.94f);
            target.info.magicResistance = Mathf.RoundToInt(target.info.magicResistance * 0.94f);
        };
    }

    private void ApplySoulHarvest(Character character)
    {
        character.OnHit += (target) => {
            if (target is Character targetCharacter)
            {
                character.StartCoroutine(ReduceHealthRegen(targetCharacter));
            }
        };
    }

    private IEnumerator ReduceHealthRegen(Character target)
    {
        float originalHealthRegen = target.info.healthRegen;
        target.info.healthRegen = Mathf.Max(0, target.info.healthRegen - 20);
        yield return new WaitForSeconds(3f);
        target.info.healthRegen = originalHealthRegen;
    }

    private float combatProwessTimer = 0f;
    private void ApplyCombatProwess(Character character)
    {
        character.OnKill += (killedCharacter) => {
            character.info.attackSpeed += 25;
            combatProwessTimer = 3f;
        };

        character.OnUpdate += (deltaTime) => {
            if (combatProwessTimer > 0)
            {
                combatProwessTimer -= deltaTime;
                if (combatProwessTimer <= 0)
                {
                    character.info.attackSpeed -= 25;
                }
            }
        };
    }

    private float violentNatureTimer = 0f;
    private void ApplyViolentNature(Character character)
    {
        character.OnKill += (killedCharacter) => {
            character.info.damageAmplification += 12;
            violentNatureTimer = 3f;
        };

        character.OnUpdate += (deltaTime) => {
            if (violentNatureTimer > 0)
            {
                violentNatureTimer -= deltaTime;
                if (violentNatureTimer <= 0)
                {
                    character.info.damageAmplification -= 12;
                }
            }
        };
    }
}