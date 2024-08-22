using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemPickupText : MonoBehaviour
{
    
    private TextMeshProUGUI textUGUI;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        textUGUI = GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.SetParent(GameManager.instance.pickUpGrid);
    }
    public void SetText(string text)
    {
        textUGUI.text = text;
    }
    public void SetText(string text, ItemRarity rarity)
    {
        string temp = "";
        switch (rarity)
        {
            case (ItemRarity.Basic):
                temp = $"<color=grey>{text}</color>";
                break;
            case (ItemRarity.Common):
                temp = $"<color=green>{text}</color>";
                break;
            case (ItemRarity.Rare):
                temp = $"<color=blue>{text}</color>";
                break;
            case (ItemRarity.Epic):
                temp = $"<color=purple>{text}</color>";
                break;
            case (ItemRarity.Legendary):
                temp = $"<color=red>{text}</color>";
                break;
        }


        textUGUI.text = temp + " 을(를) 획득했습니다.";
    }
}
