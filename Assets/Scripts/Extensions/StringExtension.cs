using System.Linq;
using UnityEngine;

public static class StringExtension
{
    public static bool IsNullorEmpty(this string value)
    {
        return value == null || !value.Any();
    }
}
