using System;

[Serializable]
public class SerializableInventory
{
    public Dict<SerializableItem, int> items;
    public SerializableInventory(Inventory inventory)
    {
        items = new();
        foreach (var x in inventory.items)
            items.Add(new SerializableItem(x.Key), x.Value);
    }
    public Inventory ToInventory()
    {
        Inventory inv = new();
        foreach (var x in items)
            inv.items.Add(x.Key.ToItem(), x.Value);
        return inv;
    }
}
