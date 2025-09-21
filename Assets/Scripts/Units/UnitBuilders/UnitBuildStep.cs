using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class UnitBuildStep : MonoBehaviour
{
    public abstract bool IsReady { get; }
    public abstract void Load(V2 position, Dictionary<string, string> info);
    public abstract void Deload();
    public abstract object GetResult();
    public abstract string GetName();
}