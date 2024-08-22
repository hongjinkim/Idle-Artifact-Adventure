using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[Serializable]
public class HeroInfo
{
    public bool? seungsoo=null;
    public Dictionary<TraitType, Dictionary<int, bool>> selectedTraits = new Dictionary<TraitType, Dictionary<int, bool>>();
    public Character character;
    public List<AppliedTrait> appliedTraits = new List<AppliedTrait>();
    public List<AppliedTraitEffect> appliedTraitEffects = new List<AppliedTraitEffect>();
    public int id; // �߰�
    public List<string> equippedItemIds = new List<string>(); // �߰�
    public string heroName;
    public HeroClass heroClass;
    public CharacteristicType characteristicType;
    public int level;
    public float currentExp;
    public float neededExp;
    public string imagePath;
    [JsonIgnore] public int battlePoint => CalculateBattlePoint();
    // �⺻ ����
    public int strength;
    public int agility;
    public int intelligence;
    public int stamina;

    // �߰� ����
    public float energy;
    public int hp
    {
        get { return _hp; }
        set
        {
            if (_hp != value)
            {
                _hp = value;
                OnHpChanged?.Invoke();
            }
        }
    }
    public int attackDamage;
    public int defense;
    public int magicResistance;
    public float attackSpeed;
    public float healthRegen;
    public float energyRegen;
    public int attackRange;
    public int expAmplification;
    public int trueDamage;
    public int damageBlock;
    public int lifeSteal;
    public int damageAmplification;
    public int damageReduction;
    public int criticalChance;
    public int criticalDamage;
    public int defensePenetration;
    public string spritePath;
    public List<Trait> traits = new List<Trait>();
    public List<PlayerSkill> skills = new List<PlayerSkill>();
    public PlayerSkill activeSkill;
    [JsonIgnore] private Sprite _sprite;

    [SerializeField] private int _hp;

    public event Action OnExperienceChanged;
    public event Action OnLevelUp;
    public event Action<int> OnTraitSelectionAvailable;
    public event Action OnHpChanged;
    //
    public int hpLevel = 1;
    public int strengthLevel = 1;
    public int defenseLevel = 1;
    public int masicResistanceLevel = 1;

    // Equipment
    public List<Item> EquippedItems;
    public Dictionary<ItemType, Item> EquippedItemDictionary = new Dictionary<ItemType, Item>();

    public string potionId;
    public Item potionItem => GameDataManager.instance.FindItem(potionId, ItemType.Usable);


    public float potionUseHp = 0.5f;


    public HeroInfo(string name, HeroClass heroClass, int id, string imagePath)
    {
        this.id = id;
        this.imagePath = imagePath;
        this.heroName = name;
        this.heroClass = heroClass;
        this.level = 1;
        this.currentExp = 0;
        this.neededExp = 2;

        // ���� �⺻ ���� ����
        this.energy = 0;
        this.strength = 0;
        this.agility = 0;
        this.intelligence = 0;
        this.stamina = 0;
        this.hp = 200;
        this.attackDamage = 10;//��������
        this.defense = 10;//���
        this.magicResistance = 10;//���� ����
        this.attackSpeed = 100;//���ݼӵ�
        this.healthRegen = 0;//ü�����
        this.energyRegen = 5;//���������
        this.expAmplification = 0;//����ġ����
        this.trueDamage = 0;//��������
        this.damageBlock = 0;//���������� �ϴ� ����
        this.lifeSteal = 0;//��������
        this.damageAmplification = 0;//��������
        this.damageReduction = 0;//���ذ���
        this.criticalChance = 0;//ũ��Ƽ��Ȯ��
        this.criticalDamage = 150;//ũ��Ƽ�õ�����
        this.defensePenetration = 0;//������
        // attackRange�� characteristicType�� ���⼭ �������� ����

        EquippedItems = new List<Item>();

        InitializeTraits();
    }
    private void InitializeTraits()
    {
        switch (heroClass)
        {
            case HeroClass.Archer:
                traits.Add(new ConcentrationTrait());
                break;
            case HeroClass.Knight:
                traits.Add(new ProtectionTrait());
                break;
            case HeroClass.Wizard:
                traits.Add(new MagicTrait());
                break;
            case HeroClass.Priest:
                traits.Add(new ProtectionTrait());
                //traits.Add(new PlunderTrait()); ����
                break;
        }
    }
    // �̹����� �ε��ϴ� �޼���
    //public Sprite LoadImage()
    //{
    //    return Resources.Load<Sprite>(imagePath);
    //}
    [JsonIgnore]
    public Sprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                _sprite = Resources.Load<Sprite>(imagePath);
                if (_sprite != null)
                {
                    return _sprite;
                }
            }
            return _sprite;
        }
    }
    public void ApplyTrait(Trait trait)
    {
        // 이미 적용된 특성인지 확인
        if (appliedTraits.Exists(t => t.Type == trait.Type && t.Level == trait.Level && t.IsLeft == trait.IsLeftTrait))
        {
            Debug.Log("This trait has already been applied.");
            return;
        }

        // Character에 특성 적용 (Character가 null이 아닐 때만)
        if (character != null)
        {
            trait.ApplyEffect(character);
        }

        // HeroInfo에 특성 정보 저장
        appliedTraits.Add(new AppliedTrait(trait.Type, trait.Level, trait.IsLeftTrait));
    }
    public void SetCharacter(Character character)
    {
        this.character = character;
        Debug.Log($"Character set for {heroName}");
    }
    public void IncreaseStrength(float amount)
    {
        strength += (int)amount;
        healthRegen += (int)(0.1f * amount);
        hp += (int)(1f * amount);

        if (characteristicType == CharacteristicType.MuscularStrength)
        {
            attackDamage += (int)(0.7f * amount);
        }
    }

    public void IncreaseAgility(float amount)
    {
        agility += (int)amount;
        attackSpeed += (int)(0.1f * amount);
        defense += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Agility)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseIntelligence(float amount)
    {
        intelligence += (int)amount;
        energyRegen += (int)(0.1f * amount);
        magicResistance += (int)(0.1f * amount);

        if (characteristicType == CharacteristicType.Intellect)
        {
            attackDamage += (int)(0.9f * amount);
        }
    }

    public void IncreaseStamina(float amount)
    {
        stamina += (int)amount;
        hp += (int)(10f * amount);
    }





    public void AddExp(float exp)
    {
        if (level >= 40 && currentExp >= neededExp)
        {
            return;
        }
        float amplifiedExp = exp * (1 + (expAmplification / 100f));
        currentExp = Mathf.Min(currentExp + amplifiedExp, neededExp);

        OnExperienceChanged?.Invoke();

        if (level < 40 && currentExp >= neededExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        currentExp -= neededExp;
        neededExp *= 1.2f;
        attackDamage += 6;
        switch (characteristicType)
        {
            case CharacteristicType.Agility:
                IncreaseAgility(2);
                break;
            case CharacteristicType.MuscularStrength:
                IncreaseStrength(3);
                break;
            case CharacteristicType.Intellect:
                IncreaseIntelligence(2);
                break;
        }
        OnLevelUp?.Invoke();
        if (level == 10 || level == 20 || level == 30 || level == 40)
        {
            OnTraitSelectionAvailable?.Invoke(level);
        }
    }
    public void SelectTrait(int level, bool isLeftTrait)
    {
        foreach (var trait in traits)
        {
            if (trait.Level == level)
            {
                trait.ChooseTrait(level, isLeftTrait);
                break;
            }
        }
    }
    public void ApplyTraits(Character character)
    {
        foreach (var trait in traits)
        {
            trait.ApplyEffect(character);
        }
    }
    public void TriggerExperienceChanged()
    {
        OnExperienceChanged?.Invoke();
    }

    public void TriggerLevelUp()
    {
        OnLevelUp?.Invoke();
    }

    public void AddTrait(Trait trait)
    {
        traits.Add(trait);
        // Ư�� �߰��� ���� ���� ����
        
    }

    public void AddSkill(PlayerSkill skill)
    {
        skills.Add(skill);
    }

    public void SetActiveSkill(PlayerSkill skill)
    {
        activeSkill = skill;
    }
 

    private int CalculateBattlePoint()
    {
        return hp * 2 + attackDamage * 2 + defense * 3 + magicResistance * 3 + level * 5 + strength * 2 + intelligence * 2 + agility * 2 + damageBlock * 3;
    }
    public void SelectTrait(TraitType traitType, int level, bool isLeft)
    {
        if (!selectedTraits.ContainsKey(traitType))
        {
            selectedTraits[traitType] = new Dictionary<int, bool>();
        }
        selectedTraits[traitType][level] = isLeft;
    }

    public bool IsTraitSelected(TraitType traitType, int level, bool isLeft)
    {
        if (selectedTraits.TryGetValue(traitType, out var traits))
        {
            if (traits.TryGetValue(level, out var selectedIsLeft))
            {
                return selectedIsLeft == isLeft;
            }
        }
        return false;
    }

    public void AddTraitEffect(TraitType traitType, int level, bool isLeft, Action<Character> effect)
    {
        appliedTraitEffects.Add(new AppliedTraitEffect(traitType, level, isLeft, effect));
    }

    public void ApplyTraitEffects(Character character)
    {
        foreach (var effect in appliedTraitEffects)
        {
            effect.Apply(character);
        }
    }

    public void ClearTraits()
    {
        selectedTraits.Clear();
        appliedTraitEffects.Clear();
    }

    public void MakeEquipmentDictionary()
    {
        foreach (Item item in EquippedItems)
        {
            EquippedItemDictionary[item.Params.Type] = item;
        }
    }
    public void AddAppliedTrait(TraitType traitType, int level, bool isLeft)
    {
        appliedTraits.Add(new AppliedTrait(traitType, level, isLeft));
    }

    public bool IsTraitApplied(TraitType traitType, int level, bool isLeft)
    {
        return appliedTraits.Exists(t => t.Type == traitType && t.Level == level && t.IsLeft == isLeft);
    }
}
public class AppliedTraitEffect
{
    public TraitType TraitType;
    public int Level;
    public bool IsLeft;
    public Action<Character> Effect;

    public AppliedTraitEffect(TraitType traitType, int level, bool isLeft, Action<Character> effect)
    {
        TraitType = traitType;
        Level = level;
        IsLeft = isLeft;
        Effect = effect;
    }

    public void Apply(Character character)
    {
        Effect(character);
    }
}