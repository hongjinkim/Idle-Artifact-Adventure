using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "IconCollection", menuName = "Scriptable/IconCollection")]
public class IconCollection : ScriptableObject
{
    public string id;
    public List<UnityEngine.Object> iconFolders;
    public List<ItemIcon> icons;

    public Sprite GetIcon(string id)
    {
        var icon = id == null ? null : icons.SingleOrDefault(i => i.id == id);

        if (icon == null && id != null) Debug.LogWarning("Icon not found: " + id);

        return icon?.sprite;
    }

#if UNITY_EDITOR

    public void Refresh()
    {
        icons.Clear();

        foreach (var folder in iconFolders)
        {
            if (folder == null) continue;

            var root = AssetDatabase.GetAssetPath(folder);
            var files = Directory.GetFiles(root, "*.png", SearchOption.AllDirectories).ToList();

            foreach (var path in files.Select(i => i.Replace("\\", "/")))
            {
                var match = Regex.Match(path, @"Assets\/HeroEditor4D\/(?<Edition>\w+)\/(.+?\/)*Icons\/\w+\/(?<Type>\w+)\/(?<Collection>.+?)\/(.+\/)*(?<Name>.+?)\.png");

                if (!match.Success) throw new Exception($"Incorrect path: {path}");

                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                var edition = match.Groups["Edition"].Value;
                var collection = match.Groups["Collection"].Value;
                var type = match.Groups["Type"].Value;
                var iconName = match.Groups["Name"].Value;
                var icon = new ItemIcon(edition, collection, type, iconName, path, sprite);

                if (icons.Any(i => i.path == icon.path))
                {
                    Debug.LogErrorFormat($"Duplicated icon: {icon.path}");
                }
                else
                {
                    icons.Add(icon);
                }
            }
        }

        icons = icons.OrderBy(i => i.name).ToList();
        EditorUtility.SetDirty(this);
    }

#endif
}

