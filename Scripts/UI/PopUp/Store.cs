using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    //public Transform _SalesListTransform;
    [Header("Transform")]
    public Transform goldPrizeListTransform;
    public Transform materialPrizeListTransform;


    public List<Prize> goldPrizeList;
    public List<Prize> materialPrizeList;
    //public List<Prize> _currentPrizeList;

    [Header("Button")]
    public Button goldPageBtn;
    public Button materialPageBtn;

    [SerializeField] private string currentPage;

    void Start()
    {
        //_currentPrizeList = goldPrizeList;
        DisplayShopItems();
        OnGoldPageButtonClicked();

        goldPageBtn.onClick.AddListener(() => OnGoldPageButtonClicked());
        materialPageBtn.onClick.AddListener(() => OnMaterialPageButtonClicked());
    }

    void DisplayShopItems()
    {
        //foreach (Transform child in _SalesListTransform)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach (Prize i in goldPrizeList)
        {
            GameObject GO = Instantiate(i.prize, goldPrizeListTransform);

            var item = new Item(i.id);

            SetItemDetails(GO, i, item);
        }
        foreach (Prize i in materialPrizeList)
        {
            GameObject GO = Instantiate(i.prize, materialPrizeListTransform);

            var item = new Item(i.id);

            SetItemDetails(GO, i, item);
        }
    }

    public void SetItemDetails(GameObject GO, Prize prize, Item item)
    {
        GO.transform.GetChild(0).GetComponent<Image>().sprite = ItemCollection.active.GetItemIcon(item).sprite;
        GO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = prize.price.ToString();
        GO.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = ItemCollection.active.GetItemIcon(prize.needmaterial).sprite;
        GO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnBuyButtonClicked(prize, item));
    }
    
    void OnBuyButtonClicked(Prize prize, Item itemToPurchase)
    {
        if (currentPage == "gold")
        {
            PurchaseItemWithGold(prize, itemToPurchase);
        }
        else if (currentPage == "material")
        {
            PurchaseItemWithMaterial(prize, itemToPurchase);
        }
    }

    public void PurchaseItemWithGold(Prize prize, Item itemToPurchase)
    {
        if (GameDataManager.instance.playerInfo.gold >= prize.price)
        {
            GameDataManager.instance.playerInfo.gold -= prize.price;
            GameDataManager.instance.AddItem(itemToPurchase);

            ToastMsg.instance.ShowMessage(itemToPurchase.Params.Name + "을(를) 구매했습니다.", 0.5f);
            EventManager.TriggerEvent(EventType.FundsUpdated, null);
        }
        else
        {
            ToastMsg.instance.ShowMessage("골드가 부족합니다.", 0.5f);
        }
    }

    public void PurchaseItemWithMaterial(Prize prize, Item itemToPurchase)
    {
        int currentQuantity = GameDataManager.instance.GetItemQuantity(prize.needmaterial);

        if (currentQuantity >= prize.price)
        {
            GameDataManager.instance.RemoveItem(prize.needmaterial, prize.price);
            GameDataManager.instance.AddItem(itemToPurchase);

            ToastMsg.instance.ShowMessage(itemToPurchase.Params.Name + "을(를) 구매했습니다.", 0.5f);
        }
        else
        {
            ToastMsg.instance.ShowMessage("재료가 부족합니다.", 0.5f);
        }
    }

    void OnGoldPageButtonClicked()
    {
        goldPrizeListTransform.SetActive(true);
        materialPrizeListTransform.SetActive(false);

        currentPage = "gold";
    }

    void OnMaterialPageButtonClicked()
    {
        goldPrizeListTransform.SetActive(false);
        materialPrizeListTransform.SetActive(true);

        currentPage = "material";
    }

}
