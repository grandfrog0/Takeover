
using System;
using System.Collections.Generic;

[Serializable]
public class SerializableItem
{
    public string unitName;
    public Dict<string, string> editedParams;

    public SerializableItem(Item item)
    {
        unitName = item.unit.unitName;
        editedParams = new (item.editedParams);
    }

    public Item ToItem()
        => new Item(UnitsManager.GetInfo(unitName), MyMath.ToDictionary(editedParams));

    public override string ToString() => $"{unitName}: [{string.Join(", ", editedParams)}]";
}

