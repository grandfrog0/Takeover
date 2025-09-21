using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void OpenSettings()
    {
        WindowManager.CreateOpen(PrefabBuffer.SettingsWindow);
    }
}
