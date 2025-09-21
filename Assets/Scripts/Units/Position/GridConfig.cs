using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "grid", menuName = "Scriptables/GridConfig")]
public class GridConfig : ScriptableObject
{
    public Dict<UnitInfo, V2> units;
}
