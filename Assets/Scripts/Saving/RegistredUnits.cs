using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "registred_units", menuName = "Scriptables/RegistredUnits")]
public class RegistredUnits : ScriptableObject
{
    public List<UnitInfo> unitInfo;
}
