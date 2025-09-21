using System;

[Serializable]
public class SettingsInfo
{
    public int soundVolume;
    public int musicVolume;

    public bool showHints;
    public bool autoOpenCreateWindow;

    // save and load settings
    public bool saveWhenMenuOpening;
    public AutosavePeriod autosavePeriod;

    public SettingsInfo Copy() => new SettingsInfo() 
    {
        soundVolume = soundVolume,
        musicVolume = musicVolume,
        showHints = showHints,
        autoOpenCreateWindow = autoOpenCreateWindow,
        saveWhenMenuOpening = saveWhenMenuOpening,
        autosavePeriod = autosavePeriod,
    };

    public override string ToString()
        => $"sound: {soundVolume}, music: {musicVolume}, showHints: {showHints}, autoOpenCreateWindow: {true}";
}
