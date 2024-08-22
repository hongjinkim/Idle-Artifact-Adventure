using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DB;

public static class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

    public static T LoadJson<T>(string name)
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath + "/Resources/Jsons", name));
        return JsonUtility.FromJson<T>(json);
    }
    public static T[] LoadArrayJson<T>(string name)
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath + "/Resources/Jsons", name));
        return getJsonArray<T>(json);
    }
}
