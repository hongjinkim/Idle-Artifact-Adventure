using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class CharacterInventorySetup
{
    public static void Setup(CharacterBase character, List<Item> equipped, CharacterAppearance appearance)
    {
        if (!appearance.Underwear.IsEmpty())
        {
            character.Underwear = character.SpriteCollection.Armor.Single(i => i.id == appearance.Underwear).sprites;
        }

        character.UnderwearColor = appearance.UnderwearColor;
        character.ShowHelmet = appearance.ShowHelmet;
        appearance.Setup(character, initialize: false);
        Setup(character, equipped);
    }

    public static void Setup(CharacterBase character, List<Item> equipped)
    {
        character.ResetEquipment();
        character.HideEars = false;
        character.CropHair = false;

        foreach (var item in equipped)
        {
            try
            {
                var spriteCollection = character.SpriteCollection;
                switch (item.Params.Type)
                {
                    case ItemType.Weapon:

                        switch (item.Params.Class)
                        {
                            case ItemClass.Bow:
                                character.WeaponType = WeaponType.Bow;
                                character.CompositeWeapon = spriteCollection.Bow.FindSprites(item.Params.SpriteId);
                                break;
                            case ItemClass.Firearm:
                                character.WeaponType = WeaponType.Paired;
                                character.SecondaryWeapon = spriteCollection.Firearm1H.Union(spriteCollection.Firearm2H).FindSprites(item.Params.SpriteId).Single(i => i.name == "Side");
                                break;
                            default:
                                if (equipped.Any(i => i.IsFirearm))
                                {
                                    character.WeaponType = WeaponType.Paired;
                                    character.PrimaryWeapon = spriteCollection.MeleeWeapon1H.FindSprite(item.Params.SpriteId);
                                }
                                else
                                {
                                    character.WeaponType = item.Params.Tags.Contains(ItemTag.TwoHanded) ? WeaponType.Melee2H : WeaponType.Melee1H;
                                    character.PrimaryWeapon = (character.WeaponType == WeaponType.Melee1H ? spriteCollection.MeleeWeapon1H : spriteCollection.MeleeWeapon2H).FindSprite(item.Params.SpriteId);
                                }
                                break;
                            case ItemClass.Bomb:
                                character.WeaponType = WeaponType.Throwable;
                                character.PrimaryWeapon = spriteCollection.Throwable.FindSprite(item.Params.SpriteId);
                                break;
                        }
                        break;
                    case ItemType.Shield:
                        character.Shield = spriteCollection.Shield.FindSprites(item.Params.SpriteId);
                        character.WeaponType = WeaponType.Melee1H;
                        break;
                    case ItemType.Armor:
                        character.Armor = spriteCollection.Armor.FindSprites(item.Params.SpriteId);
                        break;
                    case ItemType.Helmet:
                        var entry = spriteCollection.Armor.Single(i => i.id == item.Params.SpriteId);
                        character.Equip(entry, EquipmentPart.Helmet);
                        character.HideEars = !entry.tags.Contains("ShowEars");
                        character.CropHair = !entry.tags.Contains("FullHair");
                        break;
                    case ItemType.Vest:
                        character.Equip(spriteCollection.Armor.Single(i => i.id == item.Params.SpriteId), EquipmentPart.Vest, Color.white);
                        break;
                    case ItemType.Bracers:
                        character.Equip(spriteCollection.Armor.Single(i => i.id == item.Params.SpriteId), EquipmentPart.Bracers, Color.white);
                        break;
                    case ItemType.Leggings:
                        character.Equip(spriteCollection.Armor.Single(i => i.id == item.Params.SpriteId), EquipmentPart.Leggings, Color.white);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Unable to equip {0} ({1})", item.Params.SpriteId, e.Message);
            }
        }

        foreach (var part in new[] { ItemType.Vest, ItemType.Bracers, ItemType.Leggings })
        {
            if (equipped.Any(i => i.Params.Type == part))
            {
            }
            else if (character.Underwear.Any())
            {
                var entry = character.SpriteCollection.Armor.Single(i => i.sprites.Contains(character.Underwear[0]));

                character.Equip(entry, part.ToString().ToEnum<EquipmentPart>(), character.UnderwearColor);
            }
        }

        character.Initialize();
    }
}
