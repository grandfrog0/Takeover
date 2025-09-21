
using UnityEngine;

public class ItemsManager : MonoBehaviour, IInitializable
{
    public InitializeOrder Order => InitializeOrder.ItemsManager;
    public void Initialize()
    {
        ItemsInfo items = AppDataLoader.LoadedData?.items;
        if (items != null)
        {
            if (items.coins != null)
                CoinItem.LoadCoins(items.coins);
            if (items.units != null)
                UnitItem.LoadUnits(items.units);
        }
    }
}
