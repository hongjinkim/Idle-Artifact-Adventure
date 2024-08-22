using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpScroll : MonoBehaviour
{
    public static ExpScroll Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI[] countTxts;
    //private string[] expScrollIds = { "Exp_Basic", "Exp_Common", "Exp_Rare", "Exp_Epic", "Exp_Legendary" };

    List<Item> ExpScrolls = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateExpScrollCount()
    {
        for (int i = 0; i < countTxts.Length; i++)
        {
            countTxts[i].text = string.Empty;
        }


        if(GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Exp))
        {
            ExpScrolls = GameDataManager.instance.itemDictionary[ItemType.Exp];
        }

        foreach (Item item in ExpScrolls)
        {            
            int Count;

            if (item.Params.Id == "Exp_Basic")
            {
                Count = item.count;
                countTxts[0].text = Count.ToString();
            }
            else if (item.Params.Id == "Exp_Common")
            {
                Count = item.count;
                countTxts[1].text = Count.ToString();
            }
            else if (item.Params.Id == "Exp_Rare")
            {
                Count = item.count;
                countTxts[2].text = Count.ToString();
            }
            else if (item.Params.Id == "Exp_Epic")
            {
                Count = item.count;
                countTxts[3].text = Count.ToString();
            }
            else if (item.Params.Id == "Exp_Legendary")
            {
                Count = item.count;
                countTxts[4].text = Count.ToString();
            }
        }
    }

    //public void UpdateExpScrollCount()
    //{
    //    var playerInfo = GameDataManager.instance.playerInfo;

    //    for (int i = 0; i < countTxts.Length; i++)
    //    {
    //        string expScrollId = expScrollIds[i];
    //        if (playerInfo.trackedItems.TryGetValue(expScrollId, out int count))
    //        {
    //            countTxts[i].text = count.ToString();
    //        }
    //        else
    //        {
    //            countTxts[i].text = string.Empty;
    //        }
    //    }
    //}
}