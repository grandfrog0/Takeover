using System;
using System.Collections.Generic;

[Serializable]
public class ItemsInfo
{
    public List<SerializableCoinItem> coins;
    public List<SerializableUnitItem> units;

    public ItemsInfo()
    {
        coins = CoinItem.DeloadCoins();
        units = UnitItem.DeloadUnits();
    }
}
