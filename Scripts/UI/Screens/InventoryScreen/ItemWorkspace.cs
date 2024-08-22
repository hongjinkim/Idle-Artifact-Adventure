using System;
using System.Linq;
using UnityEngine;
public abstract class ItemWorkspace : MonoBehaviour
{
    public static float SfxVolume = 1;

    public Item SelectedItem { get; protected set; }

    public abstract void Refresh();

    protected void Reset()
    {
        SelectedItem = null;
        //ItemInfo.Reset();
    }

    //protected void MoveItem(Item item, ItemContainer from, ItemContainer to, int amount = 1, string currencyId = null)
    //{
    //    MoveItemSilent(item, from, to, amount);

    //    var moved = to.Items.Last(i => i.Hash == item.Hash);

    //    if (from.Stacked)
    //    {
    //        if (item.count == 0)
    //        {
    //            SelectedItem = currencyId == null ? moved : from.Items.Single(i => i.id == currencyId);
    //        }
    //    }
    //    else
    //    {
    //        SelectedItem = from.Items.LastOrDefault(i => i.Hash == item.Hash) ?? moved;
    //    }

    //    Refresh();
    //    from.Refresh(SelectedItem);
    //    to.Refresh(SelectedItem);
    //}

    //public void MoveItemSilent(Item item, ItemContainer from, ItemContainer to, int amount = 1)
    //{
    //    if (item.count <= 0) throw new ArgumentException("item.Count <= 0");
    //    if (amount <= 0) throw new ArgumentException("amount <= 0");
    //    if (item.count < amount) throw new ArgumentException("item.Count < amount");

    //    if (to.Stacked)
    //    {
    //        var targets = to.Items.Where(i => i.Hash == item.Hash).ToList();

    //        switch (targets.Count)
    //        {
    //            case 0:
    //                to.Items.Add(new Item(item.id, item.modifier, amount));
    //                break;
    //            case 1:
    //                targets[0].count += amount;
    //                break;
    //            default:
    //                throw new Exception($"Unable to move item silently: {item.id} ({item.Hash}).");
    //        }
    //    }
    //    else
    //    {
    //        to.Items.Add(new Item(item.id, item.modifier, amount));
    //    }

    //    var moved = to.Items.Last(i => i.Hash == item.Hash);

    //    if (from.Stacked)
    //    {
    //        item.count -= amount;

    //        if (item.count == 0)
    //        {
    //            from.Items.Remove(item);
    //        }
    //    }
    //    else
    //    {
    //        from.Items.Remove(item);
    //    }
    //}
}
