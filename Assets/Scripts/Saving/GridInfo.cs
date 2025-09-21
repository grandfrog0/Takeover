using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GridInfo
{
    public List<Dict<string, string>> info;
    public GridInfo(List<Dictionary<string, string>> dicts)
    {
        info = new();
        foreach (var dict in dicts)
            info.Add(new Dict<string, string>(dict));
    }
    public List<Dictionary<string, string>> ToDictionaries()
    {
        List<Dictionary<string, string>> res = new();

        foreach (var dict in info)
            res.Add(MyMath.ToDictionary(dict));

        return res;
    }
    public override string ToString()
    {
        return string.Join("; ", string.Join(", ", info));
    }
}
