using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberFormatter : MonoBehaviour
{
    public static string FormatNumber(float number)
    {
        return Format(number);
    }

    private static string Format(float number)
    {
        if (number >= 1000000000)  // 1B 이상
        {
            return (number / 1000000000).ToString("0.0") + "B";
        }
        else if (number >= 1000000)  // 1M 이상
        {
            return (number / 1000000).ToString("0.0") + "M";
        }
        else if (number >= 1000)  // 1K 이상
        {
            return (number / 1000).ToString("0.0") + "K";
        }
        else
        {
            return number.ToString("0");  // 1K 미만
        }
    }
}
