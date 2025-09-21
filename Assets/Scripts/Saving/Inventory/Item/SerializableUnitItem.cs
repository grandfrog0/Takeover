using System;
using UnityEngine;

[Serializable]
public class SerializableUnitItem
{
    public Vector2 pos;
    public int value;
    public int energy;
    public string unitName;

    public SerializableUnitItem(UnitItem unitItem)
    {
        pos = unitItem.transform.position;
        unitName = unitItem.UnitInfo.unitName;
        value = unitItem.Value;
        energy = (int)unitItem.Energy;
    }
}

