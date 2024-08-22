using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroPage : HeroScreen
{
    public TraitManager traitManager;
    public HeroMenuManager heroMenuManager;

    [Header("current  hero info")]
    [SerializeField] public HeroInfo _info;
    private Hero _currentHero;
    public int _idx;

    [Header("ItemSlots")]
    public ItemSlot Weapon;
    public ItemSlot Helmet;
    public ItemSlot Armor;
    public ItemSlot Leggings;
    public ItemSlot Potion;


    [Header("Images")]
    public Image characterImage;

    [Header("Texts")]
    public TextMeshProUGUI levlText;
    public TextMeshProUGUI BattlePointText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI AttackText;
    public TextMeshProUGUI DefenseText;
    public TextMeshProUGUI ResistanceText;
    public TextMeshProUGUI potionCount;

    [Header("ExpGaegu")]
    public Image gauge;

    [SerializeField] private AttributeUI attributeUI;
    [SerializeField] private HeroPotion heroPotion;
    [SerializeField] private PlayerInfoBar playerInfoBar;


    [Header("Buttons")]
    public Button FastEquipButton;
    public Button UnEquipButton;

    

    public void Start()
    {
        if (attributeUI == null)
        {
        }
        else
        {
            attributeUI.OnHeroInfoChanged += UpdateHeroInfo;
        }

        transform.SetAsFirstSibling();
    }

    private void OnEnable()
    {
        _info.OnExperienceChanged += GaugeBarUpdate;
        _info.OnLevelUp += GaugeBarUpdate;

        FastEquipButton.onClick.AddListener(FastEquip);
        UnEquipButton.onClick.AddListener(UnEquip);
    }

    private void OnDisable()
    {
        _info.OnExperienceChanged -= GaugeBarUpdate;
        _info.OnLevelUp -= GaugeBarUpdate;
    }
    private void UpdateHeroInfo(HeroInfo updatedInfo)
    {
        _info = updatedInfo;
        UpdateUITexts();
        UpdateEquipment();
        //물약 업데이트
        UpdatePotion();
       
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
    }

    public void OnHeroSelected(HeroInfo info, int idx)
    {
        _UIManager.ToggleMenuBar(false);
        _UIManager.TogglePlayerInfo(false);
        _idx = idx;
        _info = info;
        Initialize();
        if (heroMenuManager != null)
        {
            heroMenuManager.UpdateCurrentHero(_info);
        }

        traitManager.SetCurrentHero(_info);
        attributeUI.UpdateHeroAttributes(info);
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(info);  // HeroPotion ������Ʈ
        }
        transform.SetAsLastSibling();
        
    }
    public void Initialize()
    {
        if (_info == null)
        {
            return;
        }

        characterImage.sprite = _info.Sprite;

        UpdateUITexts();
        UpdateEquipment();
        //물약 업데이트
        UpdatePotion();

        if (attributeUI != null)
        {
            attributeUI.UpdateHeroAttributes(_info);
        }

        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
    }
    public void OnBackButtonClicked()
    {
        transform.SetAsFirstSibling();
        _UIManager.ToggleMenuBar(true);
        _UIManager.TogglePlayerInfo(true);
        traitManager.HideAllPanels();
    }


    public bool UseExpScroll(string scrollType)
    {
        if (!GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Exp))
        {
            return false;
        }

        var ExpItems = GameDataManager.instance.itemDictionary[ItemType.Exp];
        Item targetScroll = ExpItems.Find(item => item.Params.Id == scrollType);

        if (targetScroll != null && targetScroll.count > 0)
        {
            targetScroll.count--;

            EventManager.TriggerEvent(EventType.FundsUpdated, null);
            EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { { "type", ItemType.Exp } });

            if (ExpScroll.Instance != null)
            {
                ExpScroll.Instance.UpdateExpScrollCount();
            }

            if (targetScroll.count == 0)
            {
                GameDataManager.instance.RemoveItem(targetScroll);
            }
            return true;
        }
        return false;
    }

    public void OnAddExpButtonClicked(int buttonType)
    {
        string scrollType = "";
        int exp = 0;

        switch (buttonType)
        {
            case 0: scrollType = "Exp_Basic"; exp = 1; break;
            case 1: scrollType = "Exp_Common"; exp = 2; break;
            case 2: scrollType = "Exp_Rare"; exp = 3; break;
            case 3: scrollType = "Exp_Epic"; exp = 4; break;
            case 4: scrollType = "Exp_Legendary"; exp = 5; break;
        }

        if (UseExpScroll(scrollType))
        {
            _info.AddExp(exp);
        }
    }
    public void OnTalentButtonClicked()
    {
        if (traitManager != null)
        {
            traitManager.ShowTraitPanel(_info);
        }
    }
    public void OnLevelupButtonClicked()
    {
        if (_info.level >= 40 && _info.currentExp == _info.neededExp)
        {
            _info.LevelUp();
        }
    }

    public void OnBookUseLevelupButtonClicked()
    {
        float neededExp = _info.neededExp - _info.currentExp;
        if (neededExp <= 0) return;

        string[] scrollTypes = { "Exp_Legendary", "Exp_Epic", "Exp_Rare", "Exp_Common", "Exp_Basic" };
        int[] expValues = { 5, 4, 3, 2, 1 };
        float totalExpGained = 0;
        Dictionary<string, int> scrollsUsed = new Dictionary<string, int>();

        for (int i = 0; i < scrollTypes.Length; i++)
        {
            int availableScrolls = GetAvailableScrollCount(scrollTypes[i]);
            int scrollsNeeded = Mathf.Min(availableScrolls, Mathf.CeilToInt(neededExp / expValues[i]));

            if (scrollsNeeded > 0)
            {
                scrollsUsed[scrollTypes[i]] = scrollsNeeded;
                float expGained = scrollsNeeded * expValues[i];
                totalExpGained += expGained;
                neededExp -= expGained;

                if (neededExp <= 0) break;
            }
        }

        // 한 번에 아이템 사용 및 경험치 증가
        if (totalExpGained > 0)
        {
            foreach (var scroll in scrollsUsed)
            {
                UseExpScrollBatch(scroll.Key, scroll.Value);
            }
            _info.AddExp(totalExpGained);

            // UI 갱신
            UpdateExpScrollUI();
        }
    }

    private int GetAvailableScrollCount(string scrollType)
    {
        if (GameDataManager.instance.itemDictionary.TryGetValue(ItemType.Exp, out var expItems))
        {
            var scroll = expItems.Find(item => item.Params.Id == scrollType);
            return scroll?.count ?? 0;
        }
        return 0;
    }
    private void UseExpScrollBatch(string scrollType, int count)
    {
        if (GameDataManager.instance.itemDictionary.TryGetValue(ItemType.Exp, out var expItems))
        {
            var scroll = expItems.Find(item => item.Params.Id == scrollType);
            if (scroll != null)
            {
                scroll.count -= count;
                if (scroll.count <= 0)
                {
                    GameDataManager.instance.RemoveItem(scroll);
                }
            }
        }
    }

    private void UpdateExpScrollUI()
    {
        EventManager.TriggerEvent(EventType.FundsUpdated, null);
        EventManager.TriggerEvent(EventType.ItemUpdated, new Dictionary<string, object> { { "type", ItemType.Exp } });
        if (ExpScroll.Instance != null)
        {
            ExpScroll.Instance.UpdateExpScrollCount();
        }
        // 추가로 필요한 UI 업데이트 로직
    }

    public void GaugeBarUpdate()
    {
        gauge.fillAmount = Mathf.Clamp01(_info.currentExp / _info.neededExp);

        UpdateUITexts();
        if (heroPotion != null)
        {
            heroPotion.UpdateCurrentHero(_info);  // HeroPotion ������Ʈ
        }
    }

    private void UpdateUITexts()
    {
        levlText.text = _info.level.ToString();
        HealthText.text = _info.hp.ToString();
        AttackText.text = _info.attackDamage.ToString();
        DefenseText.text = _info.defense.ToString();
        ResistanceText.text = _info.magicResistance.ToString();

        BattlePointText.text = _info.battlePoint.ToString();

        //GameDataManager.instance.UpdateBattlePoint();
        //EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
        int battlePoint = CalculateBattlePoint(_info);
        BattlePointText.text = battlePoint.ToString();

        

    }

    public void UpdatePotion()
    {
        if(_info.potionId != "")
        {
            Potion.icon.sprite = ItemCollection.active.GetItemIcon(_info.potionId)?.sprite;
            
            Potion.background.sprite = ItemCollection.active.GetBackground(_info.potionId) ?? ItemCollection.active.backgroundBrown;
            if(_info.potionItem == null)
            {
                Potion.icon.color = Color.grey;
                Potion.background.color = Color.grey;
                potionCount.text = "0";
            }
            else
            {
                Potion.icon.color = Color.white;
                Potion.background.color = Color.white;
                potionCount.text = _info.potionItem.count.ToString();
            }
        }
        else
        {
            Potion.icon.sprite = Potion.BaseSprite;
            Potion.icon.color = new Color32(0, 0, 0, 100);
            Potion.background.sprite = ItemCollection.active.backgroundBrown;
            potionCount.text = "";
        }
    }

    private void UpdateEquipment()
    {
        UpdateWeapon();
        UpdateHelmet();
        UpdateArmor();
        UpdateLeggings();
    }

    private void UpdateWeapon()
    {
        if(_info.EquippedItemDictionary.ContainsKey(ItemType.Weapon))  
        {
            Weapon.icon.sprite = ItemCollection.active.GetItemIcon(_info.EquippedItemDictionary[ItemType.Weapon])?.sprite;
            Weapon.icon.color = Color.white;
            Weapon.background.sprite = ItemCollection.active.GetBackground(_info.EquippedItemDictionary[ItemType.Weapon]) ?? ItemCollection.active.backgroundBrown;
            Weapon.background.color = Color.white;
        }
        else
        {
            Weapon.icon.sprite = Weapon.BaseSprite;
            Weapon.icon.color = new Color32(0, 0, 0, 100);
            Weapon.background.sprite = ItemCollection.active.backgroundBrown;
        }
        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
    }
    private void UpdateHelmet()
    {
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Helmet))
        {
            Helmet.icon.sprite = ItemCollection.active.GetItemIcon(_info.EquippedItemDictionary[ItemType.Helmet])?.sprite;
            Helmet.icon.color = Color.white;
            Helmet.background.sprite = ItemCollection.active.GetBackground(_info.EquippedItemDictionary[ItemType.Helmet]) ?? ItemCollection.active.backgroundBrown;
            Helmet.background.color = Color.white;
        }
        else
        {
            Helmet.icon.sprite = Helmet.BaseSprite;
            Helmet.icon.color = new Color32(0, 0, 0, 100);
            Helmet.background.sprite = ItemCollection.active.backgroundBrown;
        }
        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
    }
    private void UpdateArmor()
    {
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Armor))
        {
            Armor.icon.sprite = ItemCollection.active.GetItemIcon(_info.EquippedItemDictionary[ItemType.Armor])?.sprite;
            Armor.icon.color = Color.white;
            Armor.background.sprite = ItemCollection.active.GetBackground(_info.EquippedItemDictionary[ItemType.Armor]) ?? ItemCollection.active.backgroundBrown;
            Armor.background.color = Color.white;
        }
        else
        {
            Armor.icon.sprite = Armor.BaseSprite;
            Armor.icon.color = new Color32(0, 0, 0, 100);
            Armor.background.sprite = ItemCollection.active.backgroundBrown;
        }
        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
    }
    private void UpdateLeggings()
    {
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Leggings))
        {
            Leggings.icon.sprite = ItemCollection.active.GetItemIcon(_info.EquippedItemDictionary[ItemType.Leggings])?.sprite;
            Leggings.icon.color = Color.white;
            Leggings.background.sprite = ItemCollection.active.GetBackground(_info.EquippedItemDictionary[ItemType.Leggings]) ?? ItemCollection.active.backgroundBrown;
            Leggings.background.color = Color.white;
        }
        else
        {
            Leggings.icon.sprite = Leggings.BaseSprite;
            Leggings.icon.color = new Color32(0, 0, 0, 100);
            Leggings.background.sprite = ItemCollection.active.backgroundBrown;
        }
        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
    }

    private void FastEquip()
    {
        if(GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Weapon))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Weapon]);
            if (!_info.EquippedItemDictionary.ContainsKey(ItemType.Weapon))
            {
                var i = item.Clone();

                UpdateEquipmentStat(_info, i, true);
                _info.EquippedItems.Add(i);
                _info.EquippedItemDictionary[ItemType.Weapon] = i;


                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if(_info.EquippedItemDictionary[ItemType.Weapon].battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Weapon]);
                    _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Weapon]);

                    var i = item.Clone();

                    UpdateEquipmentStat(_info, i, true);
                    _info.EquippedItems.Add(i);
                    _info.EquippedItemDictionary[ItemType.Weapon] = i;

                    GameDataManager.instance.RemoveItem(item);
                }
            }
            UpdateWeapon();
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Helmet))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Helmet]);
            if (!_info.EquippedItemDictionary.ContainsKey(ItemType.Helmet))
            {
                var i = item.Clone();

                UpdateEquipmentStat(_info, i, true);
                _info.EquippedItems.Add(i);
                _info.EquippedItemDictionary[ItemType.Helmet] = i;

                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.EquippedItemDictionary[ItemType.Helmet].battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Helmet]);
                    _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Helmet]);

                    var i = item.Clone();

                    UpdateEquipmentStat(_info, i, true);
                    _info.EquippedItems.Add(i);
                    _info.EquippedItemDictionary[ItemType.Helmet] = i;

                    GameDataManager.instance.RemoveItem(item);
                }
            }
            UpdateHelmet();
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Armor))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Armor]);
            if (!_info.EquippedItemDictionary.ContainsKey(ItemType.Armor))
            {
                var i = item.Clone();

                UpdateEquipmentStat(_info, i, true);
                _info.EquippedItems.Add(i);
                _info.EquippedItemDictionary[ItemType.Armor] = i;

                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.EquippedItemDictionary[ItemType.Armor].battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Armor]);
                    _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Armor]);

                    var i = item.Clone();

                    UpdateEquipmentStat(_info, i, true);
                    _info.EquippedItems.Add(i);
                    _info.EquippedItemDictionary[ItemType.Armor] = i;

                    GameDataManager.instance.RemoveItem(item);
                }
            }
            UpdateArmor();
        }
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Leggings))
        {
            var item = FindBestItem(GameDataManager.instance.itemDictionary[ItemType.Leggings]);
            if (!_info.EquippedItemDictionary.ContainsKey(ItemType.Leggings))
            {
                var i = item.Clone();

                UpdateEquipmentStat(_info, i, true);
                _info.EquippedItems.Add(i);
                _info.EquippedItemDictionary[ItemType.Leggings] = i;

                GameDataManager.instance.RemoveItem(item);
            }
            else
            {
                if (_info.EquippedItemDictionary[ItemType.Leggings].battlePoint < item.battlePoint)
                {
                    GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Leggings]);
                    _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Leggings]);

                    var i = item.Clone();

                    UpdateEquipmentStat(_info, i, true);
                    _info.EquippedItems.Add(i);
                    _info.EquippedItemDictionary[ItemType.Leggings] = i;

                    GameDataManager.instance.RemoveItem(item);
                }
            }
            UpdateLeggings();
        }

    }

    private void UnEquip()
    {
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Weapon))
        {
            GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Weapon].Clone());

            UpdateEquipmentStat(_info, _info.EquippedItemDictionary[ItemType.Weapon], false);
            _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Weapon]);
            _info.EquippedItemDictionary.Remove(ItemType.Weapon);
            

            UpdateWeapon();
        }
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Helmet))
        {
            GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Helmet].Clone());

            UpdateEquipmentStat(_info, _info.EquippedItemDictionary[ItemType.Helmet], false);
            _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Helmet]);
            _info.EquippedItemDictionary.Remove(ItemType.Helmet);

            UpdateHelmet();
        }
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Armor))
        {
            GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Armor].Clone());

            UpdateEquipmentStat(_info, _info.EquippedItemDictionary[ItemType.Armor], false);
            _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Armor]);
            _info.EquippedItemDictionary.Remove(ItemType.Armor);

            UpdateArmor();
        }
        if (_info.EquippedItemDictionary.ContainsKey(ItemType.Leggings))
        {
            GameDataManager.instance.AddItem(_info.EquippedItemDictionary[ItemType.Leggings].Clone());

            UpdateEquipmentStat(_info, _info.EquippedItemDictionary[ItemType.Leggings], false);
            _info.EquippedItems.Remove(_info.EquippedItemDictionary[ItemType.Leggings]);
            _info.EquippedItemDictionary.Remove(ItemType.Leggings);

            UpdateLeggings();
        }
    }

    private Item FindBestItem(List<Item> items)
    {
        int max = 0;
        Item result = null;
        foreach(Item item in items)
        {
            if(max < item.battlePoint)
            {
                max = item.battlePoint;
                result = item;
            }
        }
        return result;
    }

    private int CalculateBattlePoint(HeroInfo hero)
    {
        // �� ������ ���� �뷱���� ���� �����ؾ� �� �� �ֽ��ϴ�.
        return hero.hp * 2 + hero.attackDamage * 2 + hero.defense * 3 + hero.magicResistance * 3 + hero.level * 5 + hero.damageBlock * 5 + hero.strength * 2 + hero.agility * 2 + hero.intelligence * 2;
    }

    private void UpdateEquipmentStat(HeroInfo info, Item equipment, bool equip)
    {
        int b = equip ? 1 : -1;

        var stat = equipment.stat;

        if (stat.basic.Count != 0)
        {
            foreach (Basic basic in stat.basic)
            {
                switch (basic.id)
                {
                    case BasicStat.AttackPower:
                        info.attackDamage += basic.value * b;
                        break;
                    case BasicStat.Strength:
                        info.strength += basic.value * b;
                        break;
                    case BasicStat.Agillity:
                        info.agility += basic.value * b;
                        break;
                    case BasicStat.Intelligence:
                        info.intelligence += basic.value * b;
                        break;
                    case BasicStat.Defense:
                        info.defense += basic.value * b;
                        break;
                    case BasicStat.MagicResistance:
                        info.magicResistance += basic.value * b;
                        break;
                    case BasicStat.Health:
                        info.hp += basic.value * b;
                        break;

                }

            }
        }
        if (stat.magic.Count != 0)
        {
            foreach (Magic magic in stat.magic)
            {
                switch (magic.id)
                {
                    case MagicStat.AttackPower:
                        info.attackDamage += magic.value * b;
                        break;
                    case MagicStat.Strength:
                        info.strength += magic.value * b;
                        break;
                    case MagicStat.Agillity:
                        info.agility += magic.value * b;
                        break;
                    case MagicStat.Intelligence:
                        info.intelligence += magic.value * b;
                        break;
                    case MagicStat.Defense:
                        info.defense += magic.value * b;
                        break;
                    case MagicStat.MagicResistance:
                        info.magicResistance += magic.value * b;
                        break;
                    case MagicStat.Health:
                        info.hp += magic.value * b;
                        break;
                    case MagicStat.DamageBlock:
                        info.damageBlock += magic.value * b;
                        break;
                    case MagicStat.HealthRegeneration:
                        info.healthRegen += magic.value * b;
                        break;
                    case MagicStat.EnergyRegeneration:
                        info.energyRegen += magic.value * b;
                        break;
                    case MagicStat.AttackSpeed:
                        info.attackSpeed += magic.value * b;
                        break;
                    case MagicStat.TrueDamage:
                        info.trueDamage += magic.value * b;
                        break;
                }
            }
        }

        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
        UpdateUITexts();
    }
}
