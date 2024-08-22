using UnityEngine;
using System.Collections.Generic;

public class TraitManager : MonoBehaviour
{
    public GameObject concentrationPanel;
    public GameObject magicPanel;
    public GameObject protectionPanel;
    //public GameObject PriestPanel;
    public ConcentrationTrait concentrationTrait;
    public MagicTrait magicTrait;
    public ProtectionTrait protectionTrait;
    // ... 다른 특성 패널들 ...
    private HeroInfo currentHeroInfo;

    private void Awake()
    {
        concentrationTrait = new ConcentrationTrait();
        magicTrait=new MagicTrait();
        protectionTrait=new ProtectionTrait();
        // 다른 trait들도 여기서 초기화
    }
    public void ShowTraitPanel(HeroInfo heroInfo)
    {
        if (heroInfo == null)
        {
            Debug.LogError("Received null heroInfo in TraitManager.ShowTraitPanel");
            return;
        }
        currentHeroInfo = heroInfo;
        HideAllPanels();

        GameObject panelToShow = GetPanelForHeroTrait(heroInfo);
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            panelToShow.transform.SetAsLastSibling();
            ITraitPanel traitPanel = panelToShow.GetComponent<ITraitPanel>();
            if (traitPanel != null)
            {
                traitPanel.Initialize(heroInfo);
                Debug.Log($"Initialized trait panel for hero: {heroInfo.heroName}");
            }
            else
            {
                Debug.LogError("Failed to get ITraitPanel component from panel");
            }
        }
        else
        {
            Debug.LogError("Failed to get panel for hero trait");
        }
    }

    public void HideAllPanels()
    {
        concentrationPanel.SetActive(false);
        magicPanel.SetActive(false);
        protectionPanel.SetActive(false);
    }
    private GameObject GetPanelForHeroTrait(HeroInfo heroInfo)
    {
        if (heroInfo == null || heroInfo.traits == null || heroInfo.traits.Count == 0)
        {
            Debug.LogError("HeroInfo or traits is null or empty");
            return null;
        }

        Trait firstTrait = heroInfo.traits[0];
        if (firstTrait == null)
        {
            Debug.LogError("First trait is null");
            return null;
        }

        switch (firstTrait)
        {
            case ConcentrationTrait _:
                return concentrationPanel;
            case MagicTrait _:
                return magicPanel;
            case ProtectionTrait _:
                return protectionPanel;
            default:
                Debug.LogError($"No panel found for trait type: {firstTrait.GetType()}");
                return null;
        }
    }
    private GameObject GetPanelForTraitType(TraitType traitType)
    {
        switch (traitType)
        {
            case TraitType.Concentration:
                return concentrationPanel;
            case TraitType.Magic:
                return magicPanel;
            case TraitType.Protection:
                return protectionPanel;
            // ... 다른 특성 타입에 대한 case 추가
            default:
                return null;
        }
    }
    public Trait GetTrait(TraitType traitType)
    {
        switch (traitType)
        {
            case TraitType.Concentration:
                return concentrationTrait;
            case TraitType.Magic:
                return magicTrait;
            case TraitType.Protection:
                return protectionTrait;
            default:
                Debug.LogError($"Unknown trait type: {traitType}");
                return null;
        }
    }
    public void SetCurrentHero(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
    }

    public void ApplyConcentrationTrait(int level, bool isLeftTrait)
    {
        if (concentrationTrait == null)
        {
            Debug.LogError("concentrationTrait is null in TraitManager");
            return;
        }
        Debug.Log(currentHeroInfo.seungsoo);
        if (currentHeroInfo.seungsoo == null) currentHeroInfo.seungsoo = isLeftTrait;
        else {
            if (currentHeroInfo.seungsoo != isLeftTrait)  return; }
        concentrationTrait.ChooseTrait(level, isLeftTrait);

        if (currentHeroInfo != null)
        {
            currentHeroInfo.ApplyTrait(concentrationTrait);
        }
        else
        {
            Debug.LogWarning("No current hero found to apply trait.");
        }
    }

    public void ApplyMagicTrait(int level, bool isLeftTrait)
    {
        if (magicTrait == null)
        {
            Debug.LogError("magicTrait is null in TraitManager");
            return;
        }

        magicTrait.ChooseTrait(level, isLeftTrait);

        if (currentHeroInfo != null)
        {
            currentHeroInfo.ApplyTrait(magicTrait);
        }
        else
        {
            Debug.LogWarning("No current hero found to apply trait.");
        }
    }

    public void ApplyProtectionTrait(int level, bool isLeftTrait)
    {
        if (protectionTrait == null)
        {
            Debug.LogError("protectionTrait is null in TraitManager");
            return;
        }

        protectionTrait.ChooseTrait(level, isLeftTrait);

        if (currentHeroInfo != null)
        {
            currentHeroInfo.ApplyTrait(protectionTrait);
        }
        else
        {
            Debug.LogWarning("No current hero found to apply trait.");
        }
    }

    private Character GetCurrentCharacter()
    {
        if (currentHeroInfo != null)
        {
            Debug.Log($"Current Hero Info: {currentHeroInfo.heroName}");
            if (currentHeroInfo.character != null)
            {
                return currentHeroInfo.character;
            }
            else
            {
                Debug.LogWarning("currentHeroInfo.character is null");
            }
        }
        else
        {
            Debug.LogWarning("currentHeroInfo is null");
        }
        return null;
    }
    // Concentration Trait
    public void ApplyConcentrationTraitLeft10() { ApplyConcentrationTrait(10, true); }
    public void ApplyConcentrationTraitRight10() { ApplyConcentrationTrait(10, false); }
    public void ApplyConcentrationTraitLeft20() { ApplyConcentrationTrait(20, true); }
    public void ApplyConcentrationTraitRight20() { ApplyConcentrationTrait(20, false); }
    public void ApplyConcentrationTraitLeft30() { ApplyConcentrationTrait(30, true); }
    public void ApplyConcentrationTraitRight30() { ApplyConcentrationTrait(30, false); }
    public void ApplyConcentrationTraitLeft40() { ApplyConcentrationTrait(40, true); }
    public void ApplyConcentrationTraitRight40() { ApplyConcentrationTrait(40, false); }

    // Magic Trait
    public void ApplyMagicTraitLeft10() { ApplyMagicTrait(10, true); }
    public void ApplyMagicTraitRight10() { ApplyMagicTrait(10, false); }
    public void ApplyMagicTraitLeft20() { ApplyMagicTrait(20, true); }
    public void ApplyMagicTraitRight20() { ApplyMagicTrait(20, false); }
    public void ApplyMagicTraitLeft30() { ApplyMagicTrait(30, true); }
    public void ApplyMagicTraitRight30() { ApplyMagicTrait(30, false); }
    public void ApplyMagicTraitLeft40() { ApplyMagicTrait(40, true); }
    public void ApplyMagicTraitRight40() { ApplyMagicTrait(40, false); }

    // Protection Trait
    public void ApplyProtectionTraitLeft10() { ApplyProtectionTrait(10, true); }
    public void ApplyProtectionTraitRight10() { ApplyProtectionTrait(10, false); }
    public void ApplyProtectionTraitLeft20() { ApplyProtectionTrait(20, true); }
    public void ApplyProtectionTraitRight20() { ApplyProtectionTrait(20, false); }
    public void ApplyProtectionTraitLeft30() { ApplyProtectionTrait(30, true); }
    public void ApplyProtectionTraitRight30() { ApplyProtectionTrait(30, false); }
    public void ApplyProtectionTraitLeft40() { ApplyProtectionTrait(40, true); }
    public void ApplyProtectionTraitRight40() { ApplyProtectionTrait(40, false); }
}

// 모든 특성 패널이 구현해야 할 인터페이스
public interface ITraitPanel
{
    void Initialize(HeroInfo heroInfo);
}