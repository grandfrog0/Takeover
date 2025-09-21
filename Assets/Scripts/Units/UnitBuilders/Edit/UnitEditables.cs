using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "unit_editables", menuName = "Scriptables/UnitEditables")]
public class UnitEditables : ScriptableObject
{
    public List<Editable> editables = new();
}
