using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridUnit : MonoBehaviour, IInitializable
{
    static private GridUnit inst;
    static public bool HasUnit(V2 pos, out Unit unit)
        => inst.UnitsPositions.TryGetValue(pos, out unit);
    static public bool HasUnit(V2 pos)
        => inst.UnitsPositions.ContainsKey(pos);
    static public void AddUnit(Unit unit)
    {
        inst.UnitsPositions.Add(unit.gridTransform.Position, unit);
        inst.AddMovedListener(unit);
    }
    static public bool CanPlaceUnit(V2 pos, out string reason)
    {
        reason = "";

        if (HasUnit(pos))
        {
            reason = "has_unit";
            return false;
        }

        for (int x = -2; x <= 2; x++)
            for (int y = 2; y >= -2; y--)
            {
                if (x == 0 && y == 0) continue;

                if (HasUnit(pos + new V2(x, y), out Unit unit) && (unit.UnitName == "aggregator" || unit.HasAnyNeighbor()))
                    return true;
            }

        reason = "no_units_nearby";
        return false;
    }
    static public bool CanPlaceUnit(V2 pos)
    {
        if (HasUnit(pos)) return false;

        for (int x = -2; x <= 2; x++)
            for (int y = 2; y >= -2; y--)
            {
                if (x == 0 && y == 0) continue;

                if (HasUnit(pos + new V2(x, y), out Unit unit) && (unit.UnitName == "aggregator" || unit.HasAnyNeighbor()))
                    return true;
            }
        return false;
    }
    static public void DeleteUnit(Unit unit)
    {
        if (unit == null) return;

        inst.UnitsPositions.Remove(unit.gridTransform.Position);
        Destroy(unit.gameObject);
    }
    static public List<Dictionary<string, string>> DeloadUnits()
        => inst.UnitsPositions.Select(x => x.Value.Deload()).ToList();

    public Dictionary<V2, Unit> UnitsPositions { get; private set; }
    private event Action<V2, V2, Unit> _onUnitMoved;
    [SerializeField] GridConfig emptyGridConfig;

    public InitializeOrder Order => InitializeOrder.UnitGrid;
    public void Initialize()
    {
        inst = this;
        UnitsPositions = new();

        GridInfo info = AppDataLoader.LoadedData?.grid;

        if (info != null)
        {
            LoadUnits(info.ToDictionaries());
        }
        else
        {
            foreach((UnitInfo i, V2 pos) in emptyGridConfig.units)
                UnitsManager.BuildQuiet(i, pos);
        }
    }
    private void LoadUnits(List<Dictionary<string, string>> list)
    {
        foreach (Dictionary<string, string> dict in list)
            UnitsManager.BuildQuiet(dict);
    }
    private void AddMovedListener(Unit unit)
    {
        unit.gridTransform.onMoved.AddListener(OnUnitMoved);

        void OnUnitMoved(V2 from, V2 to)
                => this.OnUnitMoved(from, to, unit);
    }
    private void OnUnitMoved(V2 from, V2 to, Unit unit)
    {
        UnitsPositions.Remove(from);
        UnitsPositions.Add(to, unit);

        _onUnitMoved?.Invoke(from, to, unit);
    }
}
