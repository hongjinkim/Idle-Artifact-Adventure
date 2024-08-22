using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ItemInfo : PopUp
{
    [Header("Head")]
    public Image iconBackground;
    public Image icon;
    public Image iconFrame;
    public TextMeshProUGUI level;
    public TextMeshProUGUI battlePoint;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI rarity;
    public TextMeshProUGUI type;

    [Header("Basic Stat")]
    public TextMeshProUGUI luckyPoint;
    public List<TextMeshProUGUI> basicStats;
    public List<TextMeshProUGUI> magicStats;


    [Header("Lock")]
    [SerializeField] private Button lockButton;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Item currentItem;
    private List<Item> currentItems;

    [Header("View")]
    public Button nextViewBtn;
    public Button prevViewBtn;

    [Header("Function")]
    public Button disassemblyBtn;
    public Button equipBtn;

    public bool isLocked = false;
    public Item Item { get; protected set; }
    public int currentIndex; // 새로 추가된 필드

    //public GearBase gearBase;
    public Disassembly disassembly;

    protected override void Start()
    {
        base.Start();
        lockButton.onClick.AddListener(ToggleItemLock);
        nextViewBtn.onClick.AddListener(SelectNextItem);
        prevViewBtn.onClick.AddListener(SelectPreviousItem);
        disassemblyBtn.onClick.AddListener(SelectedDisassemblyButtonClikced);
        equipBtn.onClick.AddListener(EquipButtonClicked);

        UpdateButtonUI();
    }

    public void OnEnable()
    {
        if (Item == null)
        {
            Reset();
        }
    }

    public void Reset()
    {

    }

    public virtual void Initialize(Item item, List<Item> items)
    {
        currentItem = item;
        currentItems = items;

        Item = item;
        currentIndex = items.IndexOf(currentItem);

        ShowScreen();

        icon.sprite = ItemCollection.active.GetItemIcon(item).sprite;
        iconBackground.sprite = ItemCollection.active.GetBackground(item) ?? ItemCollection.active.backgroundBrown;
        iconBackground.color = Color.white;
        iconFrame.raycastTarget = true;
        level.text = "Lv. " + item.Params.Level.ToString("D2");
        itemName.text = item.Params.Name;
        rarity.text = "품질 : " + item.Params.Rarity.ToString();
        type.text = item.Params.Type.ToString();
        battlePoint.text = item.battlePoint.ToString();
        if (item.IsEquipment)
        {
            luckyPoint.text = "럭키포인트: " + item.luckyPoint.ToString() + "(" +item.luckyPercent.ToString()+ "%)";

            for(int i = 0; i < basicStats.Count; i++)
            {
                if (item.stat.basic.Count > i)
                    basicStats[i].text = item.stat.basic[i].value <= 0 ? null : item.stat.basic[i].id.ToString() + "  " + "+" + item.stat.basic[i].value.ToString() + "    (+" + (item.stat.basic[i].value - item.stat.basic[i].minValue).ToString() + ")";
                else
                    basicStats[i].text = null;
            }
            for (int i = 0; i < magicStats.Count; i++)
            {
                if (item.stat.magic.Count > i)
                    magicStats[i].text = item.stat.magic[i].value <= 0 ? null : item.stat.magic[i].id.ToString() + "  " + "+" + item.stat.magic[i].value.ToString();
                else
                    magicStats[i].text = null;
            }

        }
        else
        {
            // 장비 아이템이 아닐 경우 스탯 미표기 후 설명 표시
            luckyPoint.text = null;
            foreach(TextMeshProUGUI text in basicStats)
            {
                text.text = null;
            }
            foreach (TextMeshProUGUI text in magicStats)
            {
                text.text = null;
            }
        }

        UpdateButtonUI();
    }

    public void OnBackButtonClicked()
    {
        HideScreen();
        Item.isSelected = false;
    }

    private void ToggleItemLock()
    {
        if (currentItem != null)
        {
            currentItem.isLocked = !currentItem.isLocked;
            UpdateButtonUI();
            Debug.Log($"Item {currentItem.Params.Name} is now {(currentItem.isLocked ? "locked" : "unlocked")}");
        }
    }

    private void UpdateButtonUI()
    {
        if (currentItem != null)
        {
            buttonText.text = currentItem.isLocked ? "Unlock" : "Lock";
        }
    }

    private void SelectedDisassemblyButtonClikced()
    {
        disassembly.SelectedItemDisassembly(Item);
        OnBackButtonClicked();
    }

    private void EquipButtonClicked()
    {
        Debug.Log("장착");
    }

    public void SelectPreviousItem()
    {
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }
        if (currentIndex == -1)
        {
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 마지막 아이템 선택
            Initialize(currentItems[currentItems.Count - 1], currentItems);
        }
        else
        {
            int previousIndex = (currentIndex - 1 + currentItems.Count) % currentItems.Count;
            Initialize(currentItems[previousIndex], currentItems);
        }
    }
    public void SelectNextItem()
    {
        if (currentItems.Count == 0)
        {
            Debug.Log("Current inventory is empty.");
            return;
        }
        if (currentIndex == -1)
        {
            // 현재 선택된 아이템이 없거나 현재 인벤토리에 없는 경우, 첫 번째 아이템 선택
            Initialize(currentItems[0], currentItems);
        }
        else
        {
            int nextIndex = (currentIndex + 1) % currentItems.Count;
            Initialize(currentItems[nextIndex], currentItems);
        }
    }
}