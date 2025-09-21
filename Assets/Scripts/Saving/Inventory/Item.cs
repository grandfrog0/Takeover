using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    public UnitInfo unit;
    public Dictionary<string, string> editedParams;

    public Item(UnitInfo unit, Dictionary<string, string> editedParams = null)
    {
        this.unit = unit;
        this.editedParams = editedParams ?? new();
    }

    public override int GetHashCode()
    {
        string paramsText = string.Join(";", editedParams.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}"));
        return HashCode.Combine(unit, paramsText);
    }
    public override bool Equals(object obj) => GetHashCode().Equals(obj.GetHashCode());
    public override string ToString() => $"{unit.unitName}: {string.Join("; ", editedParams)}";
}
