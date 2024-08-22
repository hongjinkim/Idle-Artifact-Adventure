using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public Image background;
    //public Sprite activeSprite;
    public Sprite BaseSprite;
    //public List<ItemType> types;
    //public List<ItemClass> classes;

    //public bool Locked
    //{
    //    get => icon.sprite == lockedSprite;
    //    set
    //    {
    //        //icon.sprite = value ? lockedSprite : activeSprite;
    //        background.color = value ? new Color32(150, 150, 150, 255) : new Color32(255, 255, 255, 255);
    //    }
    //}

    //public bool Supports(Item item)
    //{
    //    return types.Contains(item.Params.Type) && (classes.Count == 0 || classes.Contains(item.Params.Class)) && !Locked;
    //}
}
