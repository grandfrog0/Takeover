using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppDataLoader : MonoBehaviour, IInitializable
{
    public static GameData LoadedData { get; private set; }
    public static SettingsInfo LoadedSettings { get; private set; }

    public static string SettingsPath => Path.Combine(Application.persistentDataPath, "settings.json");
    public static string DataPath => Path.Combine(Application.persistentDataPath, "save.json");

    public InitializeOrder Order => InitializeOrder.DataLoader;
    public void Initialize()
    {
        LoadSettings();
        LoadData();
    }

    public static void LoadSettings()
    {
        try
        {
            string json = File.ReadAllText(SettingsPath);
            LoadedSettings = JsonUtility.FromJson<SettingsInfo>(json);
            //Debug.Log("settings loaded.");
        }
        catch (Exception e)
        {
            Debug.Log("error occured on load settings: " + e);
        }
    }
    public static void LoadData()
    {
        try
        {
            string json = File.ReadAllText(DataPath);
            LoadedData = JsonUtility.FromJson<GameData>(json);
            //Debug.Log("data loaded.");
        }
        catch (Exception e)
        {
            Debug.Log("error occured on load: " + e);
        }
    }

    public static void SaveData()
    {
        // game data
        GameData data = new GameData();
        data.grid = InfoLoader.GetGrid();
        data.inventory = InfoLoader.GetInventory();
        data.coins = InfoLoader.GetCoins();
        data.camera = InfoLoader.GetCameraInfo();
        data.items = InfoLoader.GetItems();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(DataPath, json);

        // settings
        SettingsInfo info = InfoLoader.GetSettings();

        json = JsonUtility.ToJson(info, true);
        File.WriteAllText(SettingsPath, json);

        //Debug.Log($"data saved.");
    }
}
