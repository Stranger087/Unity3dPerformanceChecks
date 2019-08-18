using System;
using UnityEngine;

public static class ExtensionMethods
{
    public static T Next<T>(this T src) where T : struct {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[]) Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static void DestroyChildren(this Transform t) {
        for (int i = t.childCount - 1; i >= 0; i--) {
            GameObject.Destroy(t.GetChild(i).gameObject);
        }
    }
}