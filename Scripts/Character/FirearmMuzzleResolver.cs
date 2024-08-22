using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public static class FirearmMuzzleResolver
{
    private static Texture2D _texture;

    public static void Resolve(ItemSprite sprite)
    {
        sprite.MetaDict = new Dictionary<string, string> { { "Muzzle", ResolveMuzzle(sprite.path).ToString() } };
    }

    public static int ResolveMuzzle(string path)
    {
        path = Path.Combine(Environment.CurrentDirectory, path);

        _texture ??= new Texture2D(2, 2);

        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);

            _texture.LoadImage(bytes);

            var pixels = _texture.GetPixels32();
            var x = _texture.width / 2;
            var height = _texture.height - 64;

            for (var y = height - 1; y >= 0; y--)
            {
                if (pixels[x + y * _texture.width].a > 0)
                {
                    return 100 * y / height;
                }
            }
        }

        return 0;
    }
}
