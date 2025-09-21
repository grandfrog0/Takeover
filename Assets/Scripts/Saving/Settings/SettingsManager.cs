using UnityEngine;

public class SettingsManager : MonoBehaviour, IInitializable
{
    public static SettingsInfo CurrentSettings { get; set; }
    public static SettingsInfo DefaultSettings { get; private set; }

    [SerializeField] SettingsPreset defaultSettingsPreset;

    public InitializeOrder Order => InitializeOrder.SettingsManager;
    public void Initialize()
    {
        DefaultSettings = defaultSettingsPreset.settings;
        CurrentSettings = AppDataLoader.LoadedSettings ?? DefaultSettings.Copy();
    }
}
