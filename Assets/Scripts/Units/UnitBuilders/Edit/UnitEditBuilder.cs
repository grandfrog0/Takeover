using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnitEditBuilder : IBuilder
{
    private UnitBuildStep stepPrefab;
    private UnitBuildStep step;
    private V2 position;

    private object result;
    private Dictionary<string, string> oldRes;
    public bool IsFinished { get; private set; }

    public UnitEditBuilder(UnitBuildStep step, V2 startPos, Dictionary<string, string> res)
    {
        position = startPos;
        stepPrefab = step;
        oldRes = res;
    }

    public void Deload()
    {
        if (step)
        {
            step.Deload();
            Object.Destroy(step.gameObject);
        }
    }

    public bool NextStep()
    {
        if (!CanNext()) return false;

        if (step)
        {
            result = step.GetResult();
            step.Deload();
            IsFinished = true;
        }

        step = Object.Instantiate(stepPrefab);
        step.Load(position, oldRes);

        return true;
    }
    public Dictionary<string, string> GetResult() => new() { [""] = result.ToString() };

    public void End() => Deload();

    public bool CanNext() => step == null || step.IsReady;
    public bool HasNext() => false;
    public void DestroyModel() { }
}
