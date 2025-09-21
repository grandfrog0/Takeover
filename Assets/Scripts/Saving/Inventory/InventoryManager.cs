using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Instruments;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour, IInitializable
{
    private static Inventory inventory;
    public static UnityEvent<Item, int> OnItemCountChanged { get; } = new();
    public static void SetItemCount(Item item, int value)
    {
        if (CountValidator.Validate(value, out int val))
        {
            inventory.items[item] = val;
            OnItemCountChanged.Invoke(item, val);
        }
    }
    public static void AddUnitCount(Item item, int value)
    {
        if (!inventory.items.ContainsKey(item))
        {
            // delete it
            Debug.Log("Not contains an item: " + item);
            Debug.Log("Current items list: ");

            foreach((Item i, _) in inventory.items)
            {
                Debug.Log($"{i == item}, {item.GetHashCode()}, {i.GetHashCode()}, {i}");
            }

            inventory.items[item] = CountValidator.Validate(value);
        }
        else
        {
            inventory.items[item] = CountValidator.Validate(inventory.items[item] + value);
        }

        int count = inventory.items[item];
        if (count <= 0)
            inventory.items.Remove(item);

        OnItemCountChanged.Invoke(item, count);
    }
    public static int GetCount(Item item) => inventory.items.TryGetValue(item, out int c) ? c : 0;
    public static Inventory GetCopy() => new Inventory(inventory.items);

    [SerializeField] InventoryKit startInventory;
    public InitializeOrder Order => InitializeOrder.InventoryManager;
    public void Initialize()
    {
        Inventory loadedInventory = AppDataLoader.LoadedData?.inventory?.ToInventory();
        if (loadedInventory != null)
        {
            inventory = loadedInventory;
        }
        else
        {
            inventory = new();

            if (startInventory)
                foreach ((SerializableItem item, int count) in startInventory.items)
                {
                    UnitInfo info = UnitsManager.GetInfo(item.unitName);
                    Dictionary<string, string> args = new(item.editedParams);

                    if (!args.ContainsKey("fill_energy"))
                        args.Add("fill_energy", info.config.fillEnergy.ToString());

                    inventory.items.Add(new(info, args), count);
                }
        }
    }
}
