using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class ItemSprite
{
    public string name;
    public string id;
    public string edition;
    public string collection;
    public string path;
    public Sprite sprite;
    public List<Sprite> sprites;
    public List<string> tags = new List<string>();
    public string meta;

    public Dictionary<string, string> MetaDict
    {
        get => meta == "" ? new Dictionary<string, string>() : JsonConvert.DeserializeObject<Dictionary<string, string>>(meta);
        set => meta = JsonConvert.SerializeObject(value);
    }

    public ItemSprite(string edition, string collection, string type, string name, string path, Sprite sprite, List<Sprite> sprites)
    {
        this.id = $"{edition}.{collection}.{type}.{name}";

        if (sprites == null || sprites.Count == 0)
        {
            throw new Exception($"Please set [Texture Type = Sprite] for [{this.id}] from Import Settings!");
        }

        this.name = name;
        this.collection = collection;
        this.edition = edition;
        this.path = path;
        this.sprite = sprite;
        this.sprites = sprites.OrderBy(i => i.name).ToList();
    }

    public Sprite GetSprite(string name)
    {
        return sprites.Single(j => j.name == name);
    }
}
