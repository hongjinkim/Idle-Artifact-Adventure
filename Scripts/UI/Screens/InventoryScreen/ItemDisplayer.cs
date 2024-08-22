using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    //[System.Serializable]
    //public class ItemData
    //{
    //    public int index;
    //    public string itemId;
    //    public string itemName;
    //    public string itemQuality;
    //    [TextArea(3, 10)]
    //    public string itemDescription;
    //}

    //public ItemData item;
    //public int itemIndex;
    private Text itemNameText;
    private Text itemQualityText;
    private Text itemDescriptionText;

    //List<CsvLoader.ItemData> itemDatas;


    

        //public void DisplayItem()
        //{
        //    if (item == null)
        //    {
        //        return;
        //    }
        //    itemNameText.text = item.itemName;
        //    itemQualityText.text = item.itemQuality;
        //    itemDescriptionText.text = item.itemDescription;
        //
        //}
        //public void LoadFromCSV(int index)
        //{
        //    if (CsvLoader.Instance == null || CsvLoader.Instance.itemDataList == null)
        //    {
        //        return;
        //    }        

        //    itemDatas = CsvLoader.Instance.itemDataList;
        //    var csvItem = CsvLoader.Instance.itemDataList.Find(x => x.index == index);
        //    if (csvItem != null)
        //    {
        //        item = new ItemData
        //        {
        //            index = csvItem.index,
        //            itemId = csvItem.itemId,
        //            itemName = csvItem.itemName,
        //            itemQuality = csvItem.itemQuality,
        //            itemDescription = csvItem.itemDescription,
        //        };
        //        ImgLoader();
        //        DisplayItem();
        //    }
        //}

    private void ImgLoader()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("ItemImg/5");
    }

}
