using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitsManager : MonoBehaviour, IInitializable
{
    private static UnitsManager inst;

    private static Unit _curUnit;
    public static Unit CurUnit
    {
        get => _curUnit;
        set
        {
            if (inst.State == IngameState.BuildUnit ||
                inst.State == IngameState.SearchUnitPosition)
                return;

            if (inst.State != IngameState.ChooseUnit || _curUnit != value)
            {
                if (_curUnit == null && value != null)
                    KeyChains.AddDown(KeyCode.Escape, ClearCurUnit, "Deselect");

                inst.State = IngameState.ChooseUnit;

                if (_curUnit) _curUnit.OnDeselected();

                _curUnit = value;
                inst.chosenEffect.SetUnit(value);

                _curUnit.OnSelected();
            }
        }
    }
    public static bool IsCurrent(Unit unit) => CurUnit == unit;
    public static RegistredUnits RegistredUnits => inst.units;
    public static UnitInfo GetInfo(string unitName) => inst.unitsNames.ContainsKey(unitName) ? inst.unitsNames[unitName] : null;
    public static void ClearCurUnit()
    {
        inst.State = IngameState.None;
        _curUnit = null;
        inst.chosenEffect.ClearUnit();

        KeyChains.RemoveDown(KeyCode.Escape, ClearCurUnit);
    }
    public static void StartBuild(UnitInfo info, V2 pos, Dictionary<string, string> param = null)
    {
        if (GridUnit.CanPlaceUnit(pos, out string reason))
            inst.Build(info, pos, param);
        else
            Debug.Log($"Can't place unit here by reason: " + reason);
    }
    public static void StartEdit(Unit unit, UnitBuildStep step, string valueName, V2 pos)
    {
        inst.Edit(unit, step, valueName, pos);
    }
    public static void BuildQuiet(UnitInfo unitInfo, V2 position)
    {
        Dictionary<string, string> result = unitInfo.config?.CreateInitializators() ?? new();

        result["name"] = unitInfo.unitName;
        result["position"] = position.ToString();

        Unit unit = Instantiate(unitInfo.prefab, position, Quaternion.identity).GetComponent<Unit>();
        unit.InitUnit(result);
        GridUnit.AddUnit(unit);
        Initializator.InitObject(unit.gameObject, new() { unit });
        UnitUIManager.ConnectUI(unit);
    }
    public static void BuildQuiet(Dictionary<string, string> info)
    {
        Unit unit = Instantiate(GetInfo(info["name"]).prefab, V2.Parse(info["position"]), Quaternion.identity).GetComponent<Unit>();
        unit.InitUnit(info);
        GridUnit.AddUnit(unit);
        Initializator.InitObject(unit.gameObject, new() { unit });
        UnitUIManager.ConnectUI(unit);
    }

    private static UnityAction deleteAction;
    public static void DeleteCurrent()
    {
        ConfirmPopup popup = WindowManager.CreateOpenGet(PrefabBuffer.ConfirmPopup) as ConfirmPopup;
        deleteAction = () =>
        {
            popup.OnClosed.RemoveListener(deleteAction);
            if (popup.IsConfirmed)
            {
                GridUnit.DeleteUnit(CurUnit);
                ClearCurUnit();
            }
        };
        popup.OnClosed.AddListener(deleteAction);
    }

    [Header("Main")]
    [SerializeField] RegistredUnits units;
    private Dict<string, UnitInfo> unitsNames = new();

    [Header("Unit choosing")]
    [SerializeField] ChosenUnitEffect chosenEffect;

    [Header("Unit building")]
    private IBuilder builder;
    private Coroutine buildCoroutine;

    [Header("State")]
    [SerializeField] IngameState State;

    public InitializeOrder Order => InitializeOrder.UnitsManager;
    public void Initialize()
    {
        inst = this;

        foreach (UnitInfo info in units.unitInfo)
            unitsNames[info.unitName] = info;
    }

    public void CancelBuild()
    {
        if (builder != null)
        {
            builder?.Deload();
            builder.DestroyModel();
            builder = null;
        }

        if (!buildCoroutine.IsUnityNull())
        {
            StopCoroutine(buildCoroutine);
            buildCoroutine = null;
        }

        if (State == IngameState.BuildUnit)
            State = IngameState.None;

        KeyChains.RemoveDown(KeyCode.Return, GoNextBuildStep);
        KeyChains.RemoveDown(KeyCode.Escape, CancelBuild);
    }
    public void Build(UnitInfo unitInfo, V2? pos = null, Dictionary<string, string> param = null)
    {
        if (builder != null)
            CancelBuild();

        switch (State)
        {
            case IngameState.ChooseUnit:
                ClearCurUnit();
                break;

            case IngameState.SearchUnitPosition:
                ItemsSpawner.CancelSearch();
                break;
        }

        State = IngameState.BuildUnit;

        if (!buildCoroutine.IsUnityNull()) StopCoroutine(buildCoroutine);
        buildCoroutine = StartCoroutine(BuildCoroutine(unitInfo, pos, param ?? new()));
    }
    private bool NextBuildStep()
    {
        if (builder == null)
            return false;

        if (builder.CanNext() && !builder.IsFinished)
            return builder.NextStep();

        return false;
    }
    private void GoNextBuildStep() => NextBuildStep();
    private IEnumerator BuildCoroutine(UnitInfo unitInfo, V2? pos, Dictionary<string, string> startParams)
    {
        KeyChains.AddDown(KeyCode.Escape, CancelBuild, "Cancel building the unit");
        KeyChains.AddDown(KeyCode.Return, GoNextBuildStep, "Next step of building");

        builder = new UnitBuilder(unitInfo, pos, startParams);

        NextBuildStep();
        yield return new WaitUntil(() => builder.IsFinished);

        Dictionary<string, string> info = builder.GetResult();

        Unit unit = null;
        try
        {
            unit = Instantiate(unitInfo.prefab, V2.Parse(info["position"]), Quaternion.identity).GetComponent<Unit>();
        }
        catch (Exception e)
        {
            Debug.Log("Initialization failed: " + e);
        }

        if (unit != null)
        {
            unit.InitUnit(info);
            GridUnit.AddUnit(unit);

            Initializator.InitObject(unit.gameObject, new() { unit });
            InventoryManager.AddUnitCount(new Item(unitInfo, startParams), -1);

            UnitUIManager.ConnectUI(unit);
        }

        State = IngameState.None;
        CurUnit = unit;

        builder.End();

        KeyChains.RemoveDown(KeyCode.Return, GoNextBuildStep);
        KeyChains.RemoveDown(KeyCode.Escape, CancelBuild);
    }

    public void Edit(Unit unit, UnitBuildStep step, string valueName, V2 pos)
    {
        if (builder != null)
            CancelBuild();

        switch (State)
        {
            case IngameState.ChooseUnit:
                ClearCurUnit();
                break;

            case IngameState.SearchUnitPosition:
                ItemsSpawner.CancelSearch();
                break;
        }

        State = IngameState.BuildUnit;

        if (!buildCoroutine.IsUnityNull()) StopCoroutine(buildCoroutine);
        buildCoroutine = StartCoroutine(EditStepCoroutine(unit, step, valueName, pos));
    }
    private IEnumerator EditStepCoroutine(Unit unit, UnitBuildStep step, string valueName, V2 pos)
    {
        KeyChains.AddDown(KeyCode.Escape, CancelBuild, "Cancel edit");
        KeyChains.AddDown(KeyCode.Return, GoNextBuildStep, "Confirm edit");

        builder = new UnitEditBuilder(step, pos, unit.Deload());

        NextBuildStep();
        yield return new WaitUntil(() => builder.IsFinished);

        unit.SetValue(valueName, builder.GetResult()[""]);

        State = IngameState.None;
        CurUnit = unit;

        builder.End();

        KeyChains.RemoveDown(KeyCode.Return, GoNextBuildStep);
        KeyChains.RemoveDown(KeyCode.Escape, CancelBuild);
    }
}
