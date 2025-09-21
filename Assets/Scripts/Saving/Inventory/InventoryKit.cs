using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "inventory_kit", menuName = "Scriptables/InventoryKit")]
public class InventoryKit : ScriptableObject
{
    public Dict<SerializableItem, int> items;
}