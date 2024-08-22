using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroMenuManager : MonoBehaviour
{
    
    public HeroInfo currentHeroInfo;
    public TraitManager traitManager;
    [Header("Menus")]
    public Transform EquipmentMenu;
    public Transform AttributeMenu;
    public Transform LevelUpMenu;
    public Transform SkillMenu;
    public Transform TraitMenu;

    [Header("Potion")]
    public Button PotionEquipmentButton;
    public Button PotionEquipmentCloseButton;

    [Header("Skill Panels")]
    public ArcherSkillPanel archerSkillPanel;
    public KnightSkillPanel knightSkillPanel;
    public WizardSkillPanel wizardSkillPanel;
    public PriestSkillPanel priestSkillPanel;


    private void Awake()
    {
       
        EquipmentMenu.SetAsLastSibling();
        //SetCurrentHeroToSkillPanel();
    }

    private void Start()
    {
        SetCurrentHeroToSkillPanel();
    }


    public void OnEquipmentButtonClicked()
    {
        traitManager.HideAllPanels();
        EquipmentMenu.SetAsLastSibling();
       
    }


    private void HideAllSkillPanels()
    {
        archerSkillPanel.gameObject.SetActive(false);
        knightSkillPanel.gameObject.SetActive(false);
        wizardSkillPanel.gameObject.SetActive(false);
        priestSkillPanel.gameObject.SetActive(false);
    }
    public void OnAttributeButtonClicked()
    {
        traitManager.HideAllPanels();
        AttributeMenu.SetAsLastSibling();
    }

    public void OnLevelUpButtonClicked()
    {
        traitManager.HideAllPanels();
        LevelUpMenu.SetAsLastSibling();
    }

    public void OnSkillButtonClicked()
    {
        
        traitManager.HideAllPanels();
        SkillMenu.SetAsLastSibling();
        
        HideAllSkillPanels();
        switch (currentHeroInfo.heroClass)
        {
            case HeroClass.Archer:
                archerSkillPanel.SetActive(true);
                break;
            case HeroClass.Knight:
                knightSkillPanel.SetActive(true);
                break;
            case HeroClass.Priest:
                priestSkillPanel.SetActive(true);
                break;
            case HeroClass.Wizard:
                wizardSkillPanel.SetActive(true);
                break;
        }
    }
    private void SetCurrentHeroToSkillPanel()
    {
        var heroes = GameDataManager.instance.playerInfo.heroes;

        for(int i = 0; i < heroes.Count; i++)
        {
            if (heroes[i].character == null)
                heroes[i].character = GameManager.instance.heroCharacterScript[i];
            switch (heroes[i].heroClass)
            {
                case HeroClass.Archer:
                    archerSkillPanel.SetCurrentArcher(heroes[i].character as Archer);
                    break;
                case HeroClass.Knight:
                    knightSkillPanel.SetCurrentKnight(heroes[i].character as Knight);
                    break;
                case HeroClass.Priest:
                    priestSkillPanel.SetCurrentPriest(heroes[i].character as Priest);
                    break;
                case HeroClass.Wizard:
                    wizardSkillPanel.SetCurrentWizard(heroes[i].character as Wizard);
                    break;
            }
        }

    }
    public void OnTalentButtonClicked()
    {
        TraitMenu.SetAsLastSibling();
        if (traitManager == null)
        {
            Debug.LogError("TraitManager is null in HeroMenuManager");
            return;
        }
        if (currentHeroInfo == null)
        {
            Debug.LogError("currentHeroInfo is null in HeroMenuManager");
            return;
        }
        Debug.Log($"Showing trait panel for hero: {currentHeroInfo.heroName}");
        traitManager.ShowTraitPanel(currentHeroInfo);
    }
    public void UpdateCurrentHero(HeroInfo heroInfo)
    {
        Debug.Log(heroInfo.heroName);
        currentHeroInfo = heroInfo;
    }

    public void OnPotionEquipmentClicked()
    {
        MainUIManager.instance.PotionPopUp.GetComponent<Potion>().UpdatePotion(currentHeroInfo.potionId);
    }

    public void OpPotionEquipmentClosed()
    {
        MainUIManager.instance.PotionPopUp.HideScreen();
    }
}
