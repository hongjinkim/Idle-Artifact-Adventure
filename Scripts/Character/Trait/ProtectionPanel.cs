using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProtectionPanel : MonoBehaviour, ITraitPanel
{
    //public TextMeshProUGUI heroNameText;
    //public TextMeshProUGUI levelText;
    //public Image heroImage;
    public Button[] traitButtons; // 8개의 버튼 (레벨당 2개씩, 4개 레벨)
    public TextMeshProUGUI[] traitDescriptions; // 8개의 설명 텍스트
    public Image[] traitIcons; // 8개의 특성 아이콘

    private HeroInfo currentHeroInfo;
    private ProtectionTrait protectionTrait;

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        protectionTrait = heroInfo.traits.Find(t => t is ProtectionTrait) as ProtectionTrait;
        if (protectionTrait == null)
        {
            //Debug.LogError("ProtectionTrait not found for this hero.");
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        //heroNameText.text = currentHeroInfo.heroName;
        //levelText.text = "Level: " + currentHeroInfo.level.ToString();
        //heroImage.sprite = currentHeroInfo.Sprite;
        UpdateTraitButtons();
    }

    private void UpdateTraitButtons()
    {
        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "거대한 혈통", "강화된 피부",
            "무한한 활력", "강인함",
            "파괴불가", "마법 저항",
            "방패 들기", "견고한 지원"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;

            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, !isLeftTrait);

            traitButtons[i].interactable = currentHeroInfo.level >= level && !isSelected && !isOppositeSelected;

            if (isSelected)
            {
                traitButtons[i].GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                traitButtons[i].GetComponent<Image>().color = Color.white;
            }

            if (traitDescriptions != null && i < traitDescriptions.Length)
                traitDescriptions[i].text = traitNames[i];

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        if (!currentHeroInfo.IsTraitApplied(TraitType.Protection, level, isLeftTrait))
        {
            protectionTrait.ChooseTrait(level, isLeftTrait);
            if (currentHeroInfo.seungsoo == null) currentHeroInfo.seungsoo = isLeftTrait;
            else
            {
                if (currentHeroInfo.seungsoo != isLeftTrait) return;
            }
            // Character가 있으면 즉시 적용
            if (currentHeroInfo.character != null)
            {
                protectionTrait.ApplyEffect(currentHeroInfo.character);
            }
            else
            {
                // Character가 없으면 HeroInfo에만 기록
                currentHeroInfo.AddAppliedTrait(TraitType.Protection, level, isLeftTrait);
            }

            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}