using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : ItemContainer
{
    public Action onRefresh;

    /// <summary>
    /// Defines what kinds of items can be equipped.
    /// </summary>
    public List<ItemSlot> slots;

    /// <summary>
    /// Equipped items will be instantiated in front of equipment slots.
    /// </summary>
    public GameObject itemPrefab;

    /// <summary>
    /// Character preview.
    /// </summary>
    public CharacterBase preview;

    public Transform scheme;
    //public int bagSize;

    //public readonly List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public void OnValidate()
    {
        if (Application.isPlaying) return;

        slots = GetComponentsInChildren<ItemSlot>(true).ToList();

        //if (Character == null)
        //{
        //    Character = FindObjectOfType<Character>();
        //}
    }

    public void Start()
    {
        //Character.Animator.SetBool("Ready", false);
    }

    //public void SetBagSize(int size)
    //{
    //    bagSize = size;

    //    var supplies = GetComponentsInChildren<ItemSlot>(true).Where(i => i.types.Contains(ItemType.Supply)).ToList();

    //    for (var i = 0; i < supplies.Count; i++)
    //    {
    //        supplies[i].Locked = i >= size;
    //    }
    //}

    public override void Refresh(Item selected)
    {

        onRefresh?.Invoke();
    }

    private void Reset()
    {
        
    }
}
