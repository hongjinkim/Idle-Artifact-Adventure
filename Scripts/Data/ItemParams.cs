using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using static DB;

[Serializable]
public class ItemParams
{
    public int Index;
    public string Name;
    public string Id;
    public int Level;
    public ItemRarity Rarity;
    public ItemType Type;
    public ItemClass Class;
    public List<ItemTag> Tags = new List<ItemTag>();
    //public List<Property> Properties = new List<Property>();
    public int Price;
    public string IconId;
    public string SpriteId;
    public string Meta;

    [JsonIgnore, NonSerialized] public List<LocalizedValue> Localization = new List<LocalizedValue>();

    public char Grade => (char)(65 + Level);

    //public Property FindProperty(BasicStat id)
    //{
    //    var target = Properties.SingleOrDefault(i => i.id == id && i.element == ElementId.Physic);

    //    return target;
    //}

    //public Property FindProperty(BasicStat id, ElementId element)
    //{
    //    var target = Properties.SingleOrDefault(i => i.id == id && i.element == element);

    //    return target;
    //}

    public string GetLocalizedName(string language)
    {
        var localized = Localization.SingleOrDefault(i => i.Language == language) ?? Localization.SingleOrDefault(i => i.Language == "English");

        return localized == null ? Id : localized.Value;
    }

    public List<string> MetaToList()
    {
        return Meta.IsEmpty() ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(Meta);
    }

    public ItemParams Copy()
    {
        return new ItemParams
        {
            Index = Index,
            Id = Id,
            Level = Level,
            Rarity = Rarity,
            Type = Type,
            Class = Class,
            Tags = Tags.ToList(),
            //Properties = Properties.Select(i => i.Copy()).ToList(),
            Price = Price,
            IconId = IconId,
            SpriteId = SpriteId,
            Meta = Meta
        };
    }
}
