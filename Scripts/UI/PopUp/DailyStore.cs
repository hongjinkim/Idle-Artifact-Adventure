using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DailyStore : PopUp
{
    [Header("Buttons")]
    public Button backButton;

    [Header("Texts")]
    public TextMeshProUGUI gold;
    public TextMeshProUGUI ironMaterial;
    public TextMeshProUGUI silverMaterial;
    public TextMeshProUGUI goldMaterial;


    private PlayerInfo playerInfo;
    List<Item> Materials = new List<Item>();

    private void OnEnable()
    {
        EventManager.StartListening(EventType.FundsUpdated, OnGoldUpdated);
        EventManager.StartListening(EventType.ItemUpdated, OnItemUpdate);
    }
    private void OnDisable()
    {
        EventManager.StopListening(EventType.FundsUpdated, OnGoldUpdated);
        EventManager.StopListening(EventType.ItemUpdated, OnItemUpdate);
    }

    protected override void Start()
    {
        base.Start();
        playerInfo = GameDataManager.instance.playerInfo;

        OnGoldUpdated(null);
        OnItemUpdate(null);

        backButton.onClick.AddListener(_UIManager.DailyStorePopUp.HideScreen);
    }

    private void OnGoldUpdated(Dictionary<string, object> message)
    {
        gold.text = NumberFormatter.FormatNumber(playerInfo.gold);
    }


    public void OnItemUpdate(Dictionary<string, object> message)
    {
        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Material))
        {
            Materials = GameDataManager.instance.itemDictionary[ItemType.Material];
        }

        foreach (Item item in Materials)
        {

            if (item.Params.Id == "Material_Iron")
            {
                ironMaterial.text = item.count.ToString();
            }
            else if (item.Params.Id == "Material_Silver")
            {
                silverMaterial.text = item.count.ToString();
            }
            else if (item.Params.Id == "Material_Gold")
            {
                goldMaterial.text = item.count.ToString();
            }

        }
    }
}
