using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains important objects from scene
/// </summary>
public class PrefabBuffer : MonoBehaviour, IInitializable
{
    private static PrefabBuffer inst;
    public static GameObject ConfirmPopup => inst._confirmPopup;
    public static GameObject CurUnitInfoWindow => inst._curUnitInfoWindow;
    public static GameObject EditUnitWindow => inst._editUnitWindow;
    public static GameObject GenUnitInfoWindow => inst._genUnitInfoWindow;
    public static GameObject CreateUnitWindow => inst._createUnitWindow;
    public static GameObject CoinItem => inst._coinItem;
    public static GameObject UnitItem => inst._unitItem;
    public static GameObject PauseWindow => inst._pauseWindow;
    public static GameObject SettingsWindow => inst._settingsWindow;
    public static GameObject SettingsSaveAndLoadWindow => inst._settingsSaveAndLoadWindow;


    [Header("Windows")]
    [SerializeField] GameObject _confirmPopup;
    [SerializeField] GameObject _curUnitInfoWindow, _editUnitWindow, _genUnitInfoWindow, _createUnitWindow;
    [SerializeField] GameObject _pauseWindow, _settingsWindow, _settingsSaveAndLoadWindow;
    [Header("Misc")]
    [SerializeField] GameObject _coinItem, _unitItem;
    public InitializeOrder Order => InitializeOrder.Buffers;
    public void Initialize() => inst = this;
}
