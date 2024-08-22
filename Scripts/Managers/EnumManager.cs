using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacteristicType
{
    MuscularStrength,
    Agility,
    Intellect
}
public enum JobType
{
    warrior,
    archer,
    wizard,
    priest
}

/// <summary>
/// Item Enums
/// </summary>
/// 
public enum ElementId
{
    Physic = 0,
    Magic = 1,
    Fire = 2,
    Ice = 3,
    Lightning = 4,
    Light = 5,
    Darkness = 6,
    Explosive = 7
}
public enum ItemClass
{
    Undefined,
    Dagger,
    Sword,
    Axe,
    Blunt,
    Lance,
    Wand,
    Bow,
    Light,
    Heavy,
    Ring,
    Necklace,
    Food,
    Potion,
    Scroll,
    Bomb,
    Pickaxe,
    Claw,
    Fang,
    Skin,
    Firearm,
    Talisman,
    Wood,
    Ore,
    Alloy,
    Wings,
    Shell,
    Bone,
    Leather,
    Tail,
    Gunpowder
}
public enum ItemMaterial
{
    Unknown,
    Wood,
    Leather,
    Metal,
    Fruit,
    Meat,
    Liquid,
    Soup,
    Gold
}
public enum ItemRarity
{
    Legacy = -2,
    Basic = -1,
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}
public enum ItemTag
{
    Undefined = 0,
    NotForSale = 1,
    Quest = 2,
    TwoHanded = 3,
    Light = 4,
    Heavy = 5,
    Short = 6,
    Long = 7,
    Christmas = 8,
    Farm = 9,
    NoFragments = 10
}
public enum ItemType
{
    //default
    Undefined,
    //Equipments
    Weapon = 0,
    Shield = 1,
    Armor = 2,
    Helmet = 3,
    Pauldrons = 4,
    Bracers = 5,
    Gloves = 6,
    Vest = 7,
    Belt = 8,
    Leggings = 9,
    Boots = 10,
    Jewelry = 11,
    Backpack = 12,
    //Usables
    Usable = 13,
    Exp = 14,
    //Material
    Material = 15,
    //Crystal
    Crystal = 16,
    Gold = 17
}
public enum MagicStat
{
    AttackPower = 1,
    Strength = 2,
    Agillity = 3,
    Intelligence = 4,
    Defense = 5,
    MagicResistance = 6,
    Health = 7,
    DamageBlock = 8,
    HealthRegeneration = 9,
    EnergyRegeneration = 10,
    AttackSpeed = 11,
    TrueDamage = 12
}
public enum BasicStat
{
    AttackPower = 1,
    Strength = 2,
    Agillity = 3,
    Intelligence = 4,
    Defense = 5,
    MagicResistance = 6,
    Health = 7
}
public enum TraitType
{
    Concentration, // 집중
    Plunder,       // 약탈
    Magic,         // 요술
    Protection,    // 보호
    Life,          // 생명
    Explosion      // 폭발
}

public enum CharacterState
{
    Idle = 0,
    Ready = 1,
    Walk = 2,
    Run = 3,
    Jump = 4,
    Climb = 5,
    Death = 9,
    ShieldBlock = 10,
    WeaponBlock = 11,
    Evasion = 12,
    Dance = 13
}

public enum WeaponType
{
    Melee1H = 0,
    Melee2H = 1,
    Paired = 2,
    Bow = 3,
    Crossbow = 4,
    Firearm1H = 5,
    Firearm2H = 6,
    Throwable = 7
}

public enum BodyPart
{
    Body,
    Head,
    Hair,
    Ears,
    Eyebrows,
    Eyes,
    Mouth,
    Beard,
    Makeup
}

public enum EquipmentPart
{
    Armor,
    Helmet,
    Vest,
    Bracers,
    Leggings,
    MeleeWeapon1H,
    MeleeWeapon2H,
    Bow,
    Crossbow,
    SecondaryMelee1H,
    SecondaryFirearm1H,
    Shield,
    Earrings,
    Cape,
    Quiver,
    Back,
    Mask,
    Firearm1H,
    Firearm2H,
    Wings
}

public enum EventType
{
    FundsUpdated,
    LevelUpdated,
    BattlePointUpdated,
    ItemUpdated,
    ItemPickup,
    HeroUpdated,
    DungeonEntered
}

public enum InventoryType
{
    Equipment,
    Usable,
    Material,
    Crystal 
}