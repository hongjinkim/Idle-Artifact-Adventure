using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "Scriptables/ItemCollection")]
public class ItemCollection : ScriptableObject
{
    [Header("Main")]
    public List<SpriteCollection> spriteCollections = new();
    public List<IconCollection> iconCollections = new();
    public List<ItemParams> items = new();

    [Header("Extra")]
    public Sprite backgroundBlue;
    public Sprite backgroundBrown;
    public Sprite backgroundGreen;
    public Sprite backgroundGrey;
    public Sprite backgroundPurple;
    public Sprite backgroundRed;

    public static ItemCollection active;

    private Dictionary<string, ItemSprite> _itemSprites;
    private Dictionary<string, ItemIcon> _itemIcons;

    public Func<Item, ItemParams> getItemParamsOverride;

    public void OnEnable()
    {
        //items.ForEach(i => i.Properties.ForEach(p => p.ParseValue()));
    }

    public ItemParams GetItemParams(Item item)
    {
        if (getItemParamsOverride != null) return getItemParamsOverride(item);

        var itemParams = items.SingleOrDefault(i => i.Id == item.id);

        if (itemParams == null)
        {
            throw new Exception($"Item params not found: {item.id}");
        }

        return itemParams;
    }
    public ItemParams GetItemParams(string id)
    {
        var itemParams = items.SingleOrDefault(i => i.Id == id);

        if (itemParams == null)
        {
            throw new Exception($"Item params not found: {id}");
        }

        return itemParams;
    }

    public ItemSprite GetItemSprite(Item item)
    {
        _itemSprites ??= CacheSprites();

        var itemParams = GetItemParams(item);

        if (itemParams.SpriteId == null) return null;

        if (_itemSprites.ContainsKey(itemParams.SpriteId))
        {
            return _itemSprites[itemParams.SpriteId];
        }

        Debug.LogWarning($"Sprite not found: {itemParams.SpriteId}");

        return null;
    }

    public ItemIcon GetItemIcon(Item item)
    {
        _itemIcons ??= CacheIcons();

        var itemParams = GetItemParams(item);

        if (itemParams.IconId == null) return null;

        if (_itemIcons.ContainsKey(itemParams.IconId))
        {
            return _itemIcons[itemParams.IconId];
        }

        Debug.LogWarning($"Icon not found: {itemParams.IconId}");

        return null;
    }
    public ItemIcon GetItemIcon(string id)
    {
        _itemIcons ??= CacheIcons();

        var itemParams = GetItemParams(id);

        if (itemParams.IconId == null) return null;

        if (_itemIcons.ContainsKey(itemParams.IconId))
        {
            return _itemIcons[itemParams.IconId];
        }

        Debug.LogWarning($"Icon not found: {itemParams.IconId}");

        return null;
    }

    /// <summary>
    /// Can be used to find sprites directly, with no existing items needed.
    /// </summary>
    public Sprite FindSprite(string spriteId)
    {
        _itemSprites ??= CacheSprites();

        if (spriteId != null && _itemSprites.ContainsKey(spriteId))
        {
            return _itemSprites[spriteId].sprite;
        }

        Debug.LogWarning($"Sprite not found: {spriteId}");

        return null;
    }

    /// <summary>
    /// Can be used to find icons directly, with no existing items needed.
    /// </summary>
    public Sprite FindIcon(string iconId)
    {
        _itemIcons ??= CacheIcons();

        if (iconId != null && _itemIcons.ContainsKey(iconId))
        {
            return _itemIcons[iconId].sprite;
        }

        Debug.LogWarning($"Icon not found: {iconId}");

        return null;
    }

    public Func<Item, Sprite> GetBackgroundCustom;

    public Sprite GetBackground(Item item)
    {
        if (GetBackgroundCustom != null) return GetBackgroundCustom(item) ?? backgroundBrown;

        switch (item.Params.Rarity)
        {
            case ItemRarity.Legacy: return backgroundGrey;
            case ItemRarity.Basic: return backgroundGrey;
            case ItemRarity.Common: return backgroundGreen;
            case ItemRarity.Rare: return backgroundBlue;
            case ItemRarity.Epic: return backgroundPurple;
            case ItemRarity.Legendary: return backgroundRed;
            default: throw new NotImplementedException();
        }
    }
    public Sprite GetBackground(string id)
    {
        switch (GetItemParams(id).Rarity)
        {
            case ItemRarity.Legacy: return backgroundGrey;
            case ItemRarity.Basic: return backgroundGrey;
            case ItemRarity.Common: return backgroundGreen;
            case ItemRarity.Rare: return backgroundBlue;
            case ItemRarity.Epic: return backgroundPurple;
            case ItemRarity.Legendary: return backgroundRed;
            default: throw new NotImplementedException();
        }
    }

    public void Reset()
    {
        _itemSprites = null;
        _itemIcons = null;
    }

    private Dictionary<string, ItemSprite> CacheSprites()
    {
        var dict = new Dictionary<string, ItemSprite>();

        foreach (var sprite in spriteCollections.SelectMany(i => i.GetAllSprites()))
        {
            if (!dict.ContainsKey(sprite.id))
            {
                dict.Add(sprite.id, sprite);
            }
        }

        return dict;
    }

    private Dictionary<string, ItemIcon> CacheIcons()
    {
        var dict = new Dictionary<string, ItemIcon>();

        foreach (var icon in iconCollections.SelectMany(i => i.icons))
        {
            if (!dict.ContainsKey(icon.id))
            {
                dict.Add(icon.id, icon);
            }
        }

        return dict;
    }
}