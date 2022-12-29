using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ExtensionMethods
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool HasPlaceholder(this string s)
    {
        return Regex.IsMatch(s, "{\\d+}");
    }

    public static string ToLocalNumber(this int val) => val.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);

}
