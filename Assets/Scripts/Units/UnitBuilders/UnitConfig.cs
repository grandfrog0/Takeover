using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unit_config", menuName = "Scriptables/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    public float maxHealth;
    public float fillEnergy;
    public float maxEnergy;
    public float maxSpeed;
    public int buildCost;
    public int buildTime;

    public Dict<string, string> additional = new();

    public Dictionary<string, string> CreateInitializators()
    {
        Dictionary<string, string> dict = new(additional)
        {
            ["max_health"] = maxHealth.ToString(),
            ["fill_energy"] = fillEnergy.ToString(),
            ["max_energy"] = maxEnergy.ToString(),
            ["max_speed"] = maxSpeed.ToString(),
            ["build_cost"] = buildCost.ToString(),
            ["build_time"] = buildTime.ToString(),
        };
        return dict;
    }
}
