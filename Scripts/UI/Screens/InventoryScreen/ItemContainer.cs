using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public abstract class ItemContainer : MonoBehaviour
{
    public List<Item> Items { get; protected set; } = new List<Item>();

    [Header("Settings")]
    [Tooltip("Stack identical inventory items to a single UI element.")]
    public bool Stacked = true;
    public bool AutoSelect = true;

    protected GameObject _container;

    public abstract void Refresh(Item selected);

    protected virtual void Awake()
    {
        _container = this.gameObject;
    }

    public void Initialize(ref List<Item> items, Item selected = null)
    {
        Items = items;
        Refresh(selected);
    }
    public virtual void ShowContainer()
    {
        SetObjectActivity(_container, true);
    }
    public virtual void HideContainer()
    {
        SetObjectActivity(_container, false);
    }
    public static void SetObjectActivity(GameObject GO, bool state)
    {
        if (GO == null)
            return;

        GO.SetActive(state);
    }
}
