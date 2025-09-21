using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSaveAndLoadWindow : PopupWindow
{
    [SerializeField] Toggle saveWhenMenuOpeningToggle;
    [SerializeField] ChoiceBox autosaveChoice;
    public void Init()
    {
        SettingsInfo info = SettingsManager.CurrentSettings;

        saveWhenMenuOpeningToggle.IsOn = SettingsManager.CurrentSettings.saveWhenMenuOpening;

        autosaveChoice.ClearValues();
        List<ChoiceVariant> variants = new();
        foreach (object obj in Enum.GetValues(typeof(AutosavePeriod)))
        {
            variants.Add(new( ((AutosavePeriod) obj).Name(), obj ));
        }

        autosaveChoice.AddValues(variants);
        autosaveChoice.Value = (int)SettingsManager.CurrentSettings.autosavePeriod;

        saveWhenMenuOpeningToggle.onChanged.AddListener(x => SettingsManager.CurrentSettings.saveWhenMenuOpening = x);
        autosaveChoice.onValueChanged.AddListener(SetAutosavePeriod);
    }

    public void SetAutosavePeriod(int a)
    {
        SettingsManager.CurrentSettings.autosavePeriod =
            (AutosavePeriod)autosaveChoice.GetVariant(autosaveChoice.Value).value;
        PauseManager.RestartAutosaveTimer();
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
