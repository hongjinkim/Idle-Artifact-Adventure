using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationPanel : MonoBehaviour, ITraitPanel
{
    [SerializeField]
    List<GameObject> level10 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level20 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level30 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level40 = new List<GameObject>();
    //public TextMeshProUGUI levelText;
    //public Image heroImage;
    public Button[] traitButtons; // 8개의 버튼 (레벨당 2개씩, 4개 레벨)
    public TextMeshProUGUI[] traitDescriptions; // 8개의 설명 텍스트
    public Image[] traitIcons; // 8개의 특성 아이콘

    private HeroInfo currentHeroInfo;
    private ConcentrationTrait concentrationTrait;

    public void Initialize(HeroInfo heroInfo)
    {
        if (heroInfo == null)
        {
            Debug.LogError("Received null heroInfo in ConcentrationPanel.Initialize");
            return;
        }
        currentHeroInfo = heroInfo;
        concentrationTrait = heroInfo.traits.Find(t => t is ConcentrationTrait) as ConcentrationTrait;
        if (concentrationTrait == null)
        {
            Debug.LogError("ConcentrationTrait not found for this hero.");
            return;
        }
        Debug.Log($"Initialized ConcentrationPanel with hero: {heroInfo.heroName}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentHeroInfo == null)
        {
            Debug.LogError("currentHeroInfo is null in ConcentrationPanel");
            return;
        }

        //if (heroNameText != null)
        //    heroNameText.text = currentHeroInfo.heroName;
        //else
        //    Debug.LogWarning("heroNameText is not assigned in ConcentrationPanel");

        //if (levelText != null)
        //    levelText.text = "Level: " + currentHeroInfo.level.ToString();
        //else
        //    Debug.LogWarning("levelText is not assigned in ConcentrationPanel");

        //if (heroImage != null && currentHeroInfo.Sprite != null)
        //    heroImage.sprite = currentHeroInfo.Sprite;
        //else
        //    Debug.LogWarning("heroImage or currentHeroInfo.Sprite is not assigned in ConcentrationPanel");

        UpdateTraitButtons();
    }

    private void UpdateTraitButtons()
    {
        if (currentHeroInfo == null || concentrationTrait == null)
        {
            Debug.LogError("currentHeroInfo or concentrationTrait is null in UpdateTraitButtons");
            return;
        }

        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "막을 수 없는 힘", "잔인한 힘",
            "가죽 벗기기", "무자비",
            "분쇄", "영혼의 수확",
            "전투 능력", "폭력적인 성격"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;
            //노드 다등록한다음에 빼주는 방법
            //노드를 하나 선택했을때 같은레벨 선택할 수 있는 리스트에서 노드를 다 빼주면 됨 노드를 취소하면 다시 넣어주고
            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, !isLeftTrait);

            // 수정된 부분: 영웅 레벨, 현재 특성 선택 여부, 반대 특성 선택 여부를 모두 고려하여 버튼 활성화 상태 결정
            bool canSelectTrait = currentHeroInfo.level >= level && !isSelected && !isOppositeSelected;
            traitButtons[i].interactable = canSelectTrait;

            // 수정된 부분: 선택된 특성에 대한 시각적 피드백 개선
            Color buttonColor = isSelected ? Color.yellow : (canSelectTrait ? Color.white : Color.gray);
            traitButtons[i].GetComponent<Image>().color = buttonColor;

            if (traitDescriptions != null && i < traitDescriptions.Length)
                traitDescriptions[i].text = traitNames[i];

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        if (!currentHeroInfo.IsTraitApplied(TraitType.Concentration, level, isLeftTrait))
        {
            concentrationTrait.ChooseTrait(level, isLeftTrait);
            
            if (currentHeroInfo.seungsoo == null) currentHeroInfo.seungsoo = isLeftTrait;
            else
            {
                if (currentHeroInfo.seungsoo != isLeftTrait) return;
            }
            if (currentHeroInfo.character != null)
            {
                
                
                concentrationTrait.ApplyEffect(currentHeroInfo.character);
            }
           

            // 특성 적용 후 UI 갱신
            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}