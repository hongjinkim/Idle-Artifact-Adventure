using System;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionTrait : Trait
{
    private Dictionary<int, (string leftName, Action<Character> leftEffect, string rightName, Action<Character> rightEffect)> traitLevels;

    public ProtectionTrait() : base(TraitType.Protection, "보호", "방어 능력을 향상시킵니다.", 1, true)
    {
        InitializeTraitLevels();
    }

    private void InitializeTraitLevels()
    {
        traitLevels = new Dictionary<int, (string, Action<Character>, string, Action<Character>)>
        {
            {10, ("거대한 혈통", ApplyGiantLineage, "강화된 피부", ApplyReinforcedSkin)},
            {20, ("무한한 활력", ApplyInfiniteVitality, "강인함", ApplyToughness)},
            {30, ("파괴불가", ApplyIndestructible, "마법 저항", ApplyMagicResistance)},
            {40, ("방패 들기", ApplyShieldUp, "견고한 지원", ApplySolidSupport)}
        };
    }

    public override void ApplyEffect(Character character)
    {
        if (traitLevels.TryGetValue(Level, out var traitEffect))
        {
            if (!character.info.IsTraitApplied(TraitType.Protection, Level, IsLeftTrait))
            {
                if (IsLeftTrait)
                    traitEffect.leftEffect(character);
                else
                    traitEffect.rightEffect(character);

                character.info.AddAppliedTrait(TraitType.Protection, Level, IsLeftTrait);
            }
        }
    }

    private void ApplyGiantLineage(Character character)
    {
        character.maxHealth = Mathf.RoundToInt(character.maxHealth * 1.03f);
        character.currentHealth = character.maxHealth;
    }

    private void ApplyReinforcedSkin(Character character)
    {
        character.info.defense = Mathf.RoundToInt(character.info.defense * 1.04f);
    }

    private void ApplyInfiniteVitality(Character character)
    {
        character.info.defense += Mathf.RoundToInt(character.info.strength * 0.1f);
    }

    private void ApplyToughness(Character character)
    {
        character.info.magicResistance += Mathf.RoundToInt(character.info.strength * 0.1f);
    }

    private void ApplyIndestructible(Character character)
    {
        // TODO: Implement physical damage increase logic
        // character.OnTakeDamage += (damage, damageType) =>
        // {
        //     if (damageType == DamageType.Physical)
        //     {
        //         return damage * 1.05f;
        //     }
        //     return damage;
        // };
    }

    private void ApplyMagicResistance(Character character)
    {
        // TODO: Implement magic damage reduction logic
        // character.OnTakeDamage += (damage, damageType) =>
        // {
        //     if (damageType == DamageType.Magical)
        //     {
        //         return damage * 0.95f;
        //     }
        //     return damage;
        // };
    }

    private void ApplyShieldUp(Character character)
    {
        character.OnTakeDamage += (attacker, damage) =>
        {
            if (attacker.info.attackRange == 1)
            {
                return damage * 0.92f;
            }
            return damage;
        };
    }

    private void ApplySolidSupport(Character character)
    {
        character.OnTakeDamage += (attacker, damage) =>
        {
            if (attacker.info.attackRange == 4)
            {
                return damage * 0.92f;
            }
            return damage;
        };
    }
}