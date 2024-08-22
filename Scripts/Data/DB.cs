using System.Collections.Generic;



public class DB
{
    [System.Serializable]
    public class CharacterBaseData
    {
        public int idx;
        public string name;
        public CharacteristicType characteristicType;
        public int hp;
        public int attackDamage;
        public int defense;
        public int magigResistance;
        public int strength;
        public int agility;
        public int intelligence;
        public int stamina;
        public float attackSpeed;
        public float healthRegen;
        public float energyRegen;
        public int attackRange;
        public float expAmplification;
        public int trueDamage;
        public int damageBlock;
        public float lifeSteal;
        public float damageAmplification;
        public float damageReduction;
        public float criticalChance;
        public int criticalDamage;
        public float defensePenetration;
       
        //public CharacterBaseData(int idx, string name, CharacteristicType characteristicType, int hp, int attackDamage, int defense, int magigResistance, int strength,
        //    int agility, int intelligence, int stamina, float attackSpeed, float healthRegen, float energyRegen, int attackRange, float expAmplification, int trueDamage,
        //    int damageBlock, float lifeSteal, float damageAmplification, float damageReduction, float criticalChance, int criticalDamage, float defensePenetration)
        //{
        //    this.idx = idx;
        //    this.name = name;
        //    this.characteristicType = characteristicType;
        //    this.hp = hp;
        //    this.attackDamage = attackDamage;
        //    this.defense = defense;
        //    this.magigResistance = magigResistance;
        //    this.strength = strength;
        //    this.agility = agility;
        //    this.intelligence = intelligence;
        //    this.stamina = stamina;
        //    this.attackSpeed = attackSpeed;
        //    this.healthRegen = healthRegen;
        //    this.energyRegen = energyRegen;
        //    this.attackRange = attackRange;
        //    this.expAmplification = expAmplification;
        //    this.trueDamage = trueDamage;
        //    this.damageBlock = damageBlock;
        //    this.lifeSteal = lifeSteal;
        //    this.damageAmplification = damageAmplification;
        //    this.damageReduction = damageReduction;
        //    this.criticalChance = criticalChance;
        //    this.criticalDamage = criticalDamage;
        //    this.defensePenetration = defensePenetration;
        //}
    }
    [System.Serializable]
    public class ItemBaseData
    {
        public int Index;
        public string Id;
        public string Name;
        public int Level;
        public ItemRarity Rarity;
        public ItemType Type;
        public ItemClass Class;
        public ItemTag Tag1;
        public ItemTag Tag2;
        public ItemTag Tag3;
        public int Price;
        public string IconId;
        public string SpriteId;
        public string Meta;
    }
    [System.Serializable]
    public class EquipmentStatData
    {
        public int Index;
        public string Id;
        public BasicStat StatId1;
        public int StatValueMin1;
        public int StatValueMax1;
        public BasicStat StatId2;
        public int StatValueMin2;
        public int StatValueMax2;
        public BasicStat StatId3;
        public int StatValueMin3;
        public int StatValueMax3;

        public List<Basic> BasicStats
        {
            get
            {
                var basicStats = new List<Basic>();

                // 각 StatValueMin에 따라 Basic 객체 추가
                if (StatValueMin1 > 0)
                {
                    basicStats.Add(new Basic
                    {
                        id = StatId1,
                        value = UnityEngine.Random.Range(StatValueMin1, StatValueMax1),
                        minValue = StatValueMin1,
                        maxValue = StatValueMax1
                    });
                }

                if (StatValueMin2 > 0)
                {
                    basicStats.Add(new Basic
                    {
                        id = StatId2,
                        value = UnityEngine.Random.Range(StatValueMin2, StatValueMax2),
                        minValue = StatValueMin2,
                        maxValue = StatValueMax2
                    });
                }

                if (StatValueMin3 > 0)
                {
                    basicStats.Add(new Basic
                    {
                        id = StatId3,
                        value = UnityEngine.Random.Range(StatValueMin3, StatValueMax3),
                        minValue = StatValueMin3,
                        maxValue = StatValueMax3
                    });
                }

                return basicStats;
            }
        }
 
    }
}
