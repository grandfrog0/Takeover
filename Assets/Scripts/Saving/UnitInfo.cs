using UnityEngine;

[CreateAssetMenu(fileName = "info_unit", menuName = "Scriptables/UnitInfo")]
public class UnitInfo : ScriptableObject
{
    public GameObject prefab;
    public GameObject model;

    public UnitBuild build;
    public UnitConfig config;
    public UnitEditables editables;
    public UnitInfoAnimation infoAnimation;

    public string unitName;
    public string unitDescription;
}