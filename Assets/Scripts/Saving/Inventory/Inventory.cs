using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Inventory
{
    public Dictionary<Item, int> items;
    public Inventory() => items = new();
    public Inventory(Dictionary<Item, int> i) => items = new(i);
}
