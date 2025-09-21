using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public static class MyMath
{
    public static float RandRange(Vector2 v) => Random.Range(v.x, v.y);
    public static List<V2> FindTransporterPath(V2 start, V2 end, HashSet<V2> obstacles)
    {
        var queue = new Queue<List<V2>>();
        var visited = new HashSet<V2>();

        queue.Enqueue(new List<V2> { start });
        visited.Add(start);

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var current = path[^1]; // Последняя точка пути

            if (current == end)
                return path;

            foreach (var dir in V2.Directions)
            {
                var next = current + dir;

                // Проверка на выход за границы 3x3 (-1..1)
                if (next.x < -1 || next.x > 1 || next.y < -1 || next.y > 1)
                    continue;

                // Пропускаем центр (0, 0)
                if (next == new V2(0, 0))
                    continue;

                // Пропускаем препятствия
                if (obstacles.Contains(next))
                    continue;

                // Если точка не посещена, добавляем в очередь
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    var newPath = new List<V2>(path) { next };
                    queue.Enqueue(newPath);
                }
            }
        }

        return null; // Путь не найден
    }
    public static bool IsInsideRect(Vector2 v, int l, int r, int d, int u)
        => v.x >= l && v.x <= r && v.y >= d && v.y <= u;

    /*
    public static Dict<string, object> ReplaceDictionariesToDicts(Dictionary<string, object> d)
    {
        Dict<string, object> res = new();
        foreach (var x in d)
        {
            if (x.Value is Dictionary<string, object> dict)
            {
                res.Add(x.Key, ReplaceDictionariesToDicts(dict));
            }
            else
            {
                res.Add(x);
            }
        }
        return res;
    }
    public static Dictionary<string, object> ReplaceDictsToDictionaries(Dict<string, object> d)
    {
        Dictionary<string, object> res = new();
        foreach (var x in d)
        {
            if (x.Value is Dict<string, object> dict)
            {
                res.Add(x.Key, ReplaceDictsToDictionaries(dict));
            }
            else
            {
                res.Add(x.Key, x.Value);
            }
        }
        return res;
    }
    */
    public static Dictionary<TKey, TValue> Parse<TKey, TValue>(string jsonString)
        => Dict<TKey, TValue>.FromJson(jsonString).ToDictionary();
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this Dict<TKey, TValue> dict)
    {
        Dictionary<TKey, TValue> dictionary = new();
        foreach (var x in dict)
            dictionary.Add(x.Key, x.Value);
        return dictionary;
    }
}
