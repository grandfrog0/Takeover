using System.Collections.Generic;
using UnityEngine;

public class InfoLoader : MonoBehaviour
{
    public static GridInfo GetGrid() => new GridInfo(GridUnit.DeloadUnits());
    public static SerializableInventory GetInventory() => new SerializableInventory(InventoryManager.GetCopy());
    public static int GetCoins() => (int)Score.Coins;
    public static CameraInfo GetCameraInfo() => new CameraInfo(Camera.main);
    public static SettingsInfo GetSettings() => SettingsManager.CurrentSettings;
    public static ItemsInfo GetItems() => new ItemsInfo();

    private void OnApplicationQuit()
    {
        AppDataLoader.SaveData();
    }
    private void OnApplicationPause(bool pause)
    {
        AppDataLoader.SaveData();
    }
}
