using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public GridInfo grid;
    public SerializableInventory inventory;
    public int coins;
    public CameraInfo camera;
    public ItemsInfo items;

    public override string ToString()
    { 
        string inventoryText = inventory == null ? "" : string.Join("; ", inventory.items);
        return $"Grid: [{grid}], Inventory: {inventoryText}, Coins: {coins}, Camera: [{camera}]";
    }
}
