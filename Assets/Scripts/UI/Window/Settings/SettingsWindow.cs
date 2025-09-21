using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : PopupWindow
{
    [SerializeField] Slider soundSlider, musicSlider;
    [SerializeField] Toggle showHintsToggle, autoOpenCreateWindowToggle;

    public void Init()
    {
        SettingsInfo info = SettingsManager.CurrentSettings;
        InitValues();

        soundSlider.onValueChanged.AddListener(x => SettingsManager.CurrentSettings.soundVolume = (int)x);
        musicSlider.onValueChanged.AddListener(x => SettingsManager.CurrentSettings.musicVolume = (int)x);

        showHintsToggle.onChanged.AddListener(x => SettingsManager.CurrentSettings.showHints = x);
        autoOpenCreateWindowToggle.onChanged.AddListener(x => SettingsManager.CurrentSettings.autoOpenCreateWindow = x);
    }
    public void InitValues()
    {
        soundSlider.value = SettingsManager.CurrentSettings.soundVolume;
        musicSlider.value = SettingsManager.CurrentSettings.musicVolume;

        showHintsToggle.IsOn = SettingsManager.CurrentSettings.showHints;
        autoOpenCreateWindowToggle.IsOn = SettingsManager.CurrentSettings.autoOpenCreateWindow;
    }
    public void ResetSettingsButtonClicked()
    {
        SettingsManager.CurrentSettings = SettingsManager.DefaultSettings.Copy();
        InitValues();
    }
    public void OpenLoadAndSaveWindowButtonClicked()
    {
        WindowManager.CreateOpen(PrefabBuffer.SettingsSaveAndLoadWindow);
    }

    public override bool Open()
    {
        if (base.Open())
        {
            Init();
            return true;
        }
        return false;
    }
}
