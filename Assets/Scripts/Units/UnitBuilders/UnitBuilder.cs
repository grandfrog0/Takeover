using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class UnitBuilder : IBuilder
{
    private ChoosePositionStep choosePositionStep;
    private List<UnitBuildStep> steps;
    private List<Func<UnitBuildStep>> stepInits;
    private V2 Position => steps.Count > 0 ? choosePositionStep.Position : Camera.main.transform.position;

    private UnitInfo unitInfo;
    private int curIndex = -1;
    private Dictionary<string, string> result;

    public bool IsFinished { get; private set; }

    public UnitBuilder(UnitInfo unitInfo, V2? startPos = null, Dictionary<string, string> startParams = null)
    {
        this.unitInfo = unitInfo;

        choosePositionStep = BuildMaterialsContainer.Inst(BuildMaterialsContainer.ChoosePositionStep);
        choosePositionStep.Model = unitInfo.model;

        stepInits = new() { () => choosePositionStep };
        stepInits.AddRange(unitInfo.build.buildSteps.Select(x => (Func<UnitBuildStep>)(() => BuildMaterialsContainer.Inst(x))));

        steps = new();
        result = unitInfo.config?.CreateInitializators() ?? new();

        if (startParams != null)
            foreach((string str, string obj) in startParams)
                result[str] = obj;

        result["name"] = unitInfo.unitName;

        if (startPos != null)
        {
            choosePositionStep.LoadWithoutNotify((V2)startPos);
            NextStep();
        }
    }

    public void Deload()
    {
        for (; curIndex >= 0; curIndex--)
        {
            steps[curIndex].Deload();
            Object.Destroy(steps[curIndex].gameObject);
        }
        curIndex = -1;
    }

    public bool CanNext() => curIndex == -1 || (steps[curIndex].IsReady && GridUnit.CanPlaceUnit(Position));
    public bool HasNext() => curIndex < stepInits.Count - 1;

    public void DestroyModel()
    {
        if (choosePositionStep && choosePositionStep.Model)
            Object.Destroy(choosePositionStep.Model);
    }

    public bool NextStep()
    {
        if (!CanNext())
            return false;

        if (curIndex >= 0)
        {
            choosePositionStep.Model.transform.SetParent(null);

            int i = 0;
            string text = steps[curIndex].GetName();

            while (result.ContainsKey(text) && i++ < 1000)
                    text = $"{steps[curIndex].GetName()}_{i}"; // position_1

            if (i >= 1000) Debug.Log("error naming build step");
            else result[text] = steps[curIndex].GetResult().ToString();

            steps[curIndex].Deload();
        }

        if (!HasNext())
        {
            IsFinished = true;
            return false;
        }

        curIndex++;

        steps.Add(stepInits[curIndex]());
        steps[curIndex].Load(Position, result);

        return HasNext();
    }
    public Dictionary<string, string> GetResult() => new(result);

    public void End()
    {
        Object.Destroy(choosePositionStep.Model);
        Deload();
    }
}
