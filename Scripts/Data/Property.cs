using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

[Serializable]
public class Property
{
    public BasicStat id;
    public string value;

    [HideInInspector][NonSerialized] public int valueInt;
    [HideInInspector][NonSerialized] public int min;
    [HideInInspector][NonSerialized] public int max;
    [HideInInspector][NonSerialized] public int duration;
    [HideInInspector][NonSerialized] public ElementId element;
    [HideInInspector][NonSerialized] public bool percentage;

    public Property()
    {
    }

    public Property(BasicStat id, object value)
    {
        this.id = id;
        this.value = value.ToString();
        ParseValue();
    }

    public void ParseValue()
    {
        var parts = value.Split('/');

        //if (id == BasicStat.Damage || id == BasicStat.Resistance)
        //{
        //    switch (parts.Length)
        //    {
        //        case 2:
        //            element = parts[1].ToEnum<ElementId>();
        //            break;
        //        case 3:
        //            element = parts[1].ToEnum<ElementId>();
        //            duration = int.Parse(parts[2]);
        //            break;
        //        default:
        //            element = ElementId.Physic;
        //            break;
        //    }
        //}

        if (Regex.IsMatch(parts[0], @"^\d+-\d+$"))
        {
            parts = parts[0].Split('-');
            min = int.Parse(parts[0]);
            max = int.Parse(parts[1]);
        }
        else if (parts[0].EndsWith("%"))
        {
            valueInt = int.Parse(parts[0].Replace("%", null));
            percentage = true;
        }
        else
        {
            if (int.TryParse(parts[0], out var valueInt))
            {
                this.valueInt = valueInt;
            }
        }
    }

    public void ReplaceValue(string value)
    {
        this.value = value;
        ParseValue();
    }

    public void ReplaceValue(float value)
    {
        ReplaceValue(Mathf.RoundToInt(value));
    }

    public void ReplaceValue(int value)
    {
        this.value = value.ToString();
        ParseValue();
    }

    public void Add(float value)
    {
        Add(Mathf.RoundToInt(value));
    }

    public void Add(int value)
    {
        if (min > 0)
        {
            min += value;
            max += value;
            this.value = $"{min}-{max}" + (element == ElementId.Physic ? null : "/" + element);
        }
        else
        {
            valueInt += value;
            this.value = valueInt + (element == ElementId.Physic ? null : "/" + element);
        }
    }

    public void AddInPercentage(float value)
    {
        if (min > 0)
        {
            min = Mathf.RoundToInt(min * (1 + value / 100f));
            max = Mathf.RoundToInt(max * (1 + value / 100f));
            this.value = $"{min}-{max}" + (element == ElementId.Physic ? null : "/" + element);
        }
        else
        {
            valueInt = Mathf.RoundToInt(valueInt * (1 + value / 100f));
            this.value = valueInt + (element == ElementId.Physic ? null : "/" + element);
        }
    }

    public static Property Parse(string value)
    {
        var parts = value.Split('=');
        var property = new Property
        {
            id = parts[0].ToEnum<BasicStat>(),
            value = parts[1]
        };

        property.ParseValue();

        return property;
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        ParseValue();
    }

    public Property Copy()
    {
        return new Property(id, value);
    }
}