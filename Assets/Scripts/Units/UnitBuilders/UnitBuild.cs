using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "build_unit", menuName = "Scriptables/UnitBuild")]
public class UnitBuild : ScriptableObject
{
    public List<UnitBuildStep> buildSteps = new();
}
