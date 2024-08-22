using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;

public class CSVToJson : MonoBehaviour
{
    [SerializeField] private Object[] _csvFiles;

    string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    char[] TRIM_CHARS = { '\"' };


    private void OnValidate()
    {
        MakeJson();
    }
    void MakeJson()
    {
        GetAllCSV();
        foreach (TextAsset data in _csvFiles)
        {
            string json = JsonConvert.SerializeObject(Read(data), Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/" + data.name + ".json", json);
        }
    }

    void GetAllCSV()
    {
        _csvFiles = Resources.LoadAll("CSVs", typeof(TextAsset));
    }

    private List<Dictionary<string, object>> Read(TextAsset data)
    {
        var list = new List<Dictionary<string, object>>();

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }

}

