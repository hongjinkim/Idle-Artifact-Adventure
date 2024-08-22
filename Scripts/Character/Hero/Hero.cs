using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hero : Character
{
    private const float MAX_ENERGY = 100f;


    private const float REGEN_INTERVAL = 0.5f;
    private const float REGEN_PERCENT = 0.05f;
    private bool isRegenerating = false;
    private bool isTraitSelectionPending = false;
    private int pendingTraitLevel = 0;
    [SerializeField] private TraitManager traitManager;
    private Coroutine energyRegenerationCoroutine;

    float healthPercentage => (float)currentHealth / maxHealth;
    Item potion;
    int healAmount;


    public Image energyBar;
    protected virtual void OnEnable()
    {
        InstantFadeIn();
        if (isRegenerating)
        {
            CancelInvoke("RegenerateHealth");
            isRegenerating = false;
        }
        if (energyRegenerationCoroutine == null)
        {
            energyRegenerationCoroutine = StartCoroutine(RegenerateEnergy());
        }
    }


    protected virtual void OnDisable()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            InvokeRepeating("RegenerateHealth", 0f, REGEN_INTERVAL);
        }

        if (_unitState == Character.UnitState.death)
        {
            Revive();
        }
        if (energyRegenerationCoroutine != null)
        {
            StopCoroutine(energyRegenerationCoroutine);
            energyRegenerationCoroutine = null;
        }
    }


    protected override void Start()
    {
        base.Start();
        // HeroInfo에서 Character로 참조 설정
        if (info.character == null)
        info.character = this;
        
        // 저장된 특성 효과 적용
        info.ApplyTraitEffects(this);
        //info.SetCharacter(this);
        //SetStat();
       
        SetupHeroInfoEvents();
        if (traitManager != null)
        {
            foreach (var appliedTrait in info.appliedTraits)
            {
                Trait trait = traitManager.GetTrait(appliedTrait.Type);
                if (trait != null)
                {
                    trait.ChooseTrait(appliedTrait.Level, appliedTrait.IsLeft);
                    trait.ApplyEffect(this);
                }
            }
        }
        else
        {
            Debug.LogWarning("TraitManager is not assigned to Hero");
        }
        
    }
    protected override void Update()
    {
        base.Update();
        CheckAndUseSkill();
    }
    private void EnergyBarUpdate()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = info.energy / 100f; // 에너지의 최대값이 100이라고 가정
        }
    }
    private IEnumerator RegenerateEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (info != null)
            {
                info.energy = Mathf.Min(info.energy + info.energyRegen, MAX_ENERGY);
                
            }
        }
    }
    
    
    private void SetupHeroInfoEvents()
    {
        info.OnLevelUp += OnHeroLevelUp;
        info.OnTraitSelectionAvailable += OnTraitSelectionAvailable;
    }
    private void OnHeroLevelUp()
    {
        // 레벨업 시 수행할 작업...
        info.ApplyTraits(this);
        EventManager.TriggerEvent(EventType.BattlePointUpdated, null);
    }
    private void OnTraitSelectionAvailable(int level)
    {
        isTraitSelectionPending = true;
        pendingTraitLevel = level;
        // UI를 통해 사용자에게 특성 선택을 요청
        RequestTraitSelectionFromUser();
    }
    private void RequestTraitSelectionFromUser()
    {
        // 이 메서드는 UI 시스템을 통해 사용자에게 특성 선택을 요청합니다.
        // 예를 들어, 특성 선택 팝업을 표시할 수 있습니다.
        // 실제 구현은 게임의 UI 시스템에 따라 달라집니다.
        
    }
    public void OnTraitSelected(bool isLeftTrait)
    {
        if (isTraitSelectionPending)
        {
            info.SelectTrait(pendingTraitLevel, isLeftTrait);
            info.ApplyTraits(this);
            isTraitSelectionPending = false;
            pendingTraitLevel = 0;
        }
    }
   
    protected virtual void ApplyPassiveSkill1() { }
    protected virtual void ApplyPassiveSkill2() { }
    protected virtual void ApplyPassiveSkill3() { }
   
    private void RegenerateHealth()
    {
        if (!gameObject.activeInHierarchy && currentHealth < maxHealth)
        {
            currentHealth = (int)Mathf.Min(currentHealth + (maxHealth * REGEN_PERCENT), maxHealth);
            if (currentHealth >= maxHealth)
            {
                CancelInvoke("RegenerateHealth");
                isRegenerating = false;
            }
        }
    }


    //private void ActivePotion()
    //{

    //    float healthPercentage = (float)currentHealth / maxHealth;
    //    if (info.PotionItem != null && healthPercentage <= 0.9f)
    //    {
    //        Item inventoryPotion = GameDataManager.instance.playerInfo.items.Find(item =>
    //                  item != null && item.Params != null && item.Params.Id == info.PotionItem.Params.Id);

    //        if (inventoryPotion != null)
    //        {
    //            inventoryPotion.count--;


    //            if (inventoryPotion.count <= 0)
    //            {
    //                GameDataManager.instance.playerInfo.items.Remove(inventoryPotion);
    //                info.PotionItem = null; // 캐릭터의 포션 아이템 참조 제거
    //            }

    //        }
    //    }
    //}

    public override void TakeDamage(Character attacker, float damage)
    {
        base.TakeDamage(attacker, damage);
        ActivePotion();
    }

    private void ActivePotion()
    {
        if (info.potionUseHp >= healthPercentage)
        {
            if (potion != info.potionItem)
                potion = info.potionItem;
            // 포션 사용
            if(potion != null)
            {
                GameDataManager.instance.RemoveItem(potion);

                // 여기에 포션 효과 적용 로직 추가 (예: 체력 회복)
                
                switch (potion.id)
                {
                    case ("Potion_Green_S"):
                        healAmount = 100;
                        break;
                    case ("Potion_Green_M"):
                        healAmount = 200;
                        break;
                    case ("Potion_Yellow_S"):
                        healAmount = 300;
                        break;
                    case ("Potion_Yellow_M"):
                        healAmount = 400;
                        break;
                    case ("Potion_Red_S"):
                        healAmount = 500;
                        break;
                    case ("Potion_Red_M"):
                        healAmount = 600;
                        break;
                    default:
                        healAmount = 0;
                        break;

                }
                currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            }
        }
            
    }




}


//    protected void IncreaseIntelligence(float amount)
//    {
//        info.intelligence += (int)amount;
//        info.energyRegen += (int)(0.1f * amount);
//        info.magicResistance += (int)(0.1f * amount);

//        if (info.characteristicType == CharacteristicType.Intellect)
//        {
//            info.attackDamage += (int)(0.9f * amount);
//        }
//        SetStat();
//    }

//    protected void IncreaseStamina(float amount)
//    {
//        info.stamina += (int)amount;
//        info.hp += (int)(10f * amount);
//        SetStat();
//    }

//    private void RegenerateHealth()
//    {
//        if (!gameObject.activeInHierarchy && currentHealth < maxHealth)
//        {
//            currentHealth = Mathf.Min(currentHealth + (maxHealth * REGEN_PERCENT), maxHealth);
//            if (currentHealth >= maxHealth)
//            {
//                CancelInvoke("RegenerateHealth");
//                isRegenerating = false;
//            }
//        }
//    }
//}

