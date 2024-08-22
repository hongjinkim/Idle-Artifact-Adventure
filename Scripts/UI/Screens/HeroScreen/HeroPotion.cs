using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPotion : MonoBehaviour
{
    public HeroPage heropage;
    public Potion potionPopup;

    [Header("Slider")]
    public Slider slider;

    [Header("PotionInfos")]
    public PotionInfo potionGreenS;
    public PotionInfo potionGreenM;
    public PotionInfo potionYellowS;
    public PotionInfo potionYellowM;
    public PotionInfo potionRedS;
    public PotionInfo potionRedM;
    public PotionInfo selectedPotionInfo;
    [SerializeField]private List<PotionInfo> _potionInfos;

    [Header("Button")]
    [SerializeField] private Button swapButton;


    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _potionName;
    [SerializeField] private TextMeshProUGUI _potionDescription;
    //public TextMeshProUGUI potionSelectedCount;
    public TextMeshProUGUI sliderValue;
    //[SerializeField] private PotionInfo[] potionInfos;

    //[SerializeField] private Image selectedSlotImage;


    //[SerializeField] private Color normalColor = Color.white;
    //[SerializeField] private Color selectedColor = Color.yellow;
    [Header("Sprite")]
    public Sprite baseSprite;
    

    private int currentSelectedIndex = -1;

    private HeroInfo currentHero;
    //private Dictionary<int, int> heroToPotionIndex = new Dictionary<int, int>();

    public static HeroPotion Instance { get; private set; }

    private void OnValidate()
    {
        _potionInfos.Add(potionGreenS);
        _potionInfos.Add(potionGreenM);
        _potionInfos.Add(potionYellowS);
        _potionInfos.Add(potionYellowM);
        _potionInfos.Add(potionRedS);
        _potionInfos.Add(potionRedM);
    }

    private void Start()
    {
        potionGreenS.button.onClick.AddListener(() => SelectPotion(potionGreenS));
        potionGreenM.button.onClick.AddListener(() => SelectPotion(potionGreenM));
        potionYellowS.button.onClick.AddListener(() => SelectPotion(potionYellowS));
        potionYellowM.button.onClick.AddListener(() => SelectPotion(potionYellowM));
        potionRedS.button.onClick.AddListener(() => SelectPotion(potionRedS));
        potionRedM.button.onClick.AddListener(() => SelectPotion(potionRedM));

        

        EventManager.StartListening(EventType.ItemUpdated, UpdatePotionInfo);

        if (swapButton != null)
        {
            swapButton.onClick.AddListener(SwapPotion);
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        UpdatePotionInfo(null);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventType.ItemUpdated, UpdatePotionInfo);
    }

    public void UpdatePotionInfo(Dictionary<string, object> message)
    {
        bool temp = false;
        if (message == null)
            temp = true;
        else if (message.ContainsKey("type"))
        {
            if ((ItemType)message["type"] == ItemType.Usable)
            {
                temp = true;
            }
        }

        if (temp)
        {
            foreach (PotionInfo info in _potionInfos)
            {
                var item = GameDataManager.instance.FindItem(info.id, ItemType.Usable);
                if (item == null)
                {
                    info.icon.color = Color.grey;
                    info.background.color = Color.grey;
                    info.count.text = "0";
                }
                else
                {
                    info.icon.color = Color.white;
                    info.background.color = Color.white;
                    info.count.text = item.count.ToString();
                }
            }
        }
    }

    private void SelectPotion(PotionInfo info)
    {
        selectedPotionInfo.id = info.id;
        selectedPotionInfo.icon.sprite = info.icon.sprite;
        selectedPotionInfo.count.text = info.count.text;
        selectedPotionInfo.icon.color = Color.white;
        selectedPotionInfo.background.sprite = ItemCollection.active.GetBackground(new Item(info.id)) ?? ItemCollection.active.backgroundBrown;
        selectedPotionInfo.background.color = Color.white;

        _potionName.text = info.potionName;
        _potionDescription.text = info.description;
    }


    private void SwapPotion()
    {
        if(currentHero.potionId != selectedPotionInfo.id)
        {
            currentHero.potionId = selectedPotionInfo.id;
        }
        heropage.UpdatePotion();
        potionPopup.HideScreen();
    }

    public void UpdateCurrentHero(HeroInfo hero)
    {
        currentHero = hero;
        //ResetPotionUI();

        //UpdatePotionSlot(); // 현재 영웅 변경 시 포션 슬롯 업데이트
        slider.value = hero.potionUseHp;
        sliderValue.text = (slider.value * 100).ToString() + "%";

    }
    public void UpdateHeroPotionItem(string id)
    {
        foreach(PotionInfo info in _potionInfos)
        {
            if(info.id == id)
            {
                SelectPotion(info);
                return;
            }
        }
        ResetPotionUI();
    }
    //private void UpdatePotionEffect(int index)
    //{
    //    if (index < 0 || index >= potionInfos.Length) return;

    //    // 영웅의 정보에 따라 포션 효과를 계산하고 설명을 업데이트합니다.
    //    // 이는 게임의 구체적인 규칙에 따라 달라질 수 있습니다.
    //    switch (index)
    //    {
    //        case 0: // Green S
    //        case 1: // Green M
    //            int hpRestore = (index == 0) ? 10 : 20;
    //            potionInfos[index].description = $"HP를 {hpRestore + 5} 회복합니다.";
    //            break;
    //        case 2: // Yellow S
    //        case 3: // Yellow M
    //            int mpRestore = (index == 2) ? 30 : 60;
    //            potionInfos[index].description = $"HP를 {mpRestore + 10} 회복합니다.";
    //            break;
    //        case 4: // Red S
    //        case 5: // Red M
    //            int atkBoost = (index == 4) ? 80 : 100;
    //            potionInfos[index].description = $"HP을 {atkBoost + 15} 회복합니다.";
    //            break;
    //    }
    //}

    private void ResetPotionUI()
    {
        selectedPotionInfo.id = "";
        selectedPotionInfo.icon.sprite = baseSprite;
        selectedPotionInfo.icon.color = new Color32(0, 0, 0, 100);
        selectedPotionInfo.background.sprite = ItemCollection.active.backgroundBrown;
        selectedPotionInfo.count.text = "";

        _potionName.text = "";
        _potionDescription.text = "";
    }

    //public void UpdatePotionSlot()
    //{
    //    try
    //    {
    //        if (currentHero != null && currentHero.PotionItem != null && currentHero.PotionItem.Params != null)
    //        {
    //            Debug.Log($"Updating potion slot for hero: {currentHero.heroName}, Potion: {currentHero.PotionItem.Params.Id}");

    //            Sprite potionSprite = null;
    //            try
    //            {
    //                potionSprite = ItemCollection.active.GetItemIcon(currentHero.PotionItem)?.sprite;
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.LogError($"Error getting potion icon: {e.Message}");
    //            }

    //            if (potionSprite != null)
    //            {
    //                if (potionSelected != null)
    //                {
    //                    potionSelected.sprite = potionSprite;
    //                    potionSelected.color = Color.white;
    //                }
    //                if (selectedSlotImage != null)
    //                {
    //                    selectedSlotImage.sprite = potionSprite;
    //                    selectedSlotImage.color = Color.white;
    //                }
    //            }
    //            else
    //            {
    //                Debug.LogWarning($"No sprite found for potion: {currentHero.PotionItem.Params.Id}");
    //            }

    //            if (potionSelectedCount != null)
    //            {
    //                potionSelectedCount.text = currentHero.PotionItem.count.ToString();
    //                potionSelectedCount.gameObject.SetActive(true);
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("No potion equipped or invalid potion data");
    //            ResetPotionSlot();
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"Error in UpdatePotionSlot: {e.Message}");
    //        ResetPotionSlot();
    //    }
    //}

    private void ResetPotionSlot()
    {
        selectedPotionInfo = null;
    }

    private void OnSliderValueChanged(float value)
    {
        currentHero.potionUseHp = MathF.Round(value * 10f) / 10f;
        slider.value = currentHero.potionUseHp;
        sliderValue.text = (slider.value * 100).ToString() + "%";
    }
}