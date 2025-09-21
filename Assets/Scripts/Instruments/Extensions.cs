using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public static class Extensions
{
    public static List<T> FindObjectsOfInterface<T>(this MonoBehaviour obj) where T : class
    {
        List<T> list = new List<T>();
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            T[] components = go.GetComponents<T>();
            if (components != null && components.Length > 0)
            {
                list.AddRange(components);
            }
        }

        return list;
    }
    public static SortedSet<T> ToSortedSet<T>(this List<T> list, IComparer<T> comparer)
    {
        SortedSet<T> set = new(comparer);
        foreach (T t in list)
            set.Add(t);
        return set;
    }
    public static bool TryParse(this Direction dir, V2 v2, out Direction direction)
    {
        if (v2 == (0, 1))
            direction = Direction.Up;
        else if (v2 == (0, -1))
            direction = Direction.Down;
        else if (v2 == (1, 0))
            direction = Direction.Right;
        else if (v2 == (-1, 0))
            direction = Direction.Left;
        else
        {
            direction = default;
            return false;
        }

        return true;
    }
    public static Vector3 ZeroZ(this Vector3 v3) => new Vector3(v3.x, v3.y);
    public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue another = default)
        => dict.TryGetValue(key, out TValue value) ? value : another;
    public static TValue GetOrDefault<TKey, TValue>(this Dict<TKey, TValue> dict, TKey key, TValue another = default)
        => dict.TryGetValue(key, out TValue value) ? value : another;
    public static bool DictEquals<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other)
        => dict.Count == other.Count && dict.All(kv => kv.Value.Equals(other[kv.Key]));
    public static string ToDictString<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        => new Dict<TKey, TValue>(dict).ToJson();
}
