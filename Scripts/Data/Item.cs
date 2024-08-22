using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class Item 
{
    public string id;
    public Modifier modifier;

    public Stat stat;
    public int luckyPercent;
    public int luckyPoint;

    public int battlePoint;

    public int count;


    public bool isLocked;
    public bool isSelected { get; set; }

    public Item(string id, int count = 1)
    {
        this.id = id;
        this.count = count;

        RandomStat(this);
    }

    public Item(string id, Modifier modifier,Stat stat, int luckyPercent, int luckyPoint,int battlePoint, int count = 1)
    {
        this.id = id;
        this.count = count;
        this.modifier = modifier;
        this.stat = stat;
        this.luckyPercent = luckyPercent;
        this.luckyPoint = luckyPoint;
        this.battlePoint = battlePoint;
    }

    public Item Clone()
    {
        return new Item(id, modifier,stat, luckyPercent, luckyPoint, battlePoint, count);
    }

    public Item RandomStat(Item item)
    {
        if (item.IsEquipment)
        {
            var statData = GameDataManager.instance.equipmentStatData[item.Params.Index];
            item.stat = new Stat(statData.BasicStats);

            luckyPoint = 0;
            luckyPercent = 0;

            for (int i = 0; i < item.stat.basic.Count; i++)
            {
                item.luckyPoint += item.stat.basic[i].value - item.stat.basic[i].minValue;
                item.luckyPercent += item.luckyPoint * 100 / item.stat.basic[i].maxValue / item.stat.basic.Count;
            }

            var rarity = item.Params.Rarity;
            List<MagicStat> enumValues = new List<MagicStat>((MagicStat[])Enum.GetValues(typeof(MagicStat)));

            for (int i = 0; i <= (int)(rarity); i++)
            {
                var randomIndex = Random.Range(0, enumValues.Count);

                item.stat.magic.Add(new Magic { id = (MagicStat)enumValues[randomIndex], value = Random.Range(1, (int)rarity*10+10)/*���� ��  ����*/});
                enumValues.RemoveAt(randomIndex);
            }
            battlePoint = CalcItemBattlePoint();
        }

        return item;
    }

    private int CalcItemBattlePoint()
    {
        int result = 0;


        // ���� ���� ���� ���� �ٸ��� ���� �ʿ�
        if(stat.basic.Count != 0)
        {
            foreach (Basic basic in stat.basic)
            {
                result += basic.value;
            }
        }
        if (stat.magic.Count != 0)
        {
            foreach (Magic magic in stat.magic)
            {
                result += magic.value;
            }
        }
            

        return result;
    }

    [JsonIgnore] public ItemParams Params => ItemCollection.active.GetItemParams(this);
    [JsonIgnore] public ItemSprite Sprite => ItemCollection.active.GetItemSprite(this);
    [JsonIgnore] public ItemIcon Icon => ItemCollection.active.GetItemIcon(this);

    [JsonIgnore] public int Hash => $"{id}.{modifier?.id}.{modifier?.level}".GetHashCode();
    [JsonIgnore] public bool IsModified => modifier != null;
    [JsonIgnore] public bool IsEquipment => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings || Params.Type == ItemType.Weapon || Params.Type == ItemType.Shield;
    [JsonIgnore] public bool IsArmor => Params.Type == ItemType.Helmet || Params.Type == ItemType.Armor || Params.Type == ItemType.Vest || Params.Type == ItemType.Bracers || Params.Type == ItemType.Leggings;
    [JsonIgnore] public bool IsWeapon => Params.Type == ItemType.Weapon;
    [JsonIgnore] public bool IsShield => Params.Type == ItemType.Shield;
    [JsonIgnore] public bool IsDagger => Params.Class == ItemClass.Dagger;
    [JsonIgnore] public bool IsSword => Params.Class == ItemClass.Sword;
    [JsonIgnore] public bool IsAxe => Params.Class == ItemClass.Axe;
    [JsonIgnore] public bool IsPickaxe => Params.Class == ItemClass.Pickaxe;
    [JsonIgnore] public bool IsWand => Params.Class == ItemClass.Wand;
    [JsonIgnore] public bool IsBlunt => Params.Class == ItemClass.Blunt;
    [JsonIgnore] public bool IsLance => Params.Class == ItemClass.Lance;
    [JsonIgnore] public bool IsMelee => Params.Type == ItemType.Weapon && Params.Class != ItemClass.Bow && Params.Class != ItemClass.Firearm;
    [JsonIgnore] public bool IsBow => Params.Class == ItemClass.Bow;
    [JsonIgnore] public bool IsFirearm => Params.Class == ItemClass.Firearm;
    [JsonIgnore] public bool IsOneHanded => !IsTwoHanded;
    [JsonIgnore] public bool IsTwoHanded => Params.Class == ItemClass.Bow || Params.Tags.Contains(ItemTag.TwoHanded);
    [JsonIgnore] public bool IsCanStacked => Params.Type == ItemType.Usable || Params.Type == ItemType.Material || Params.Type == ItemType.Crystal || Params.Type == ItemType.Exp;
}
