using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseDirectionBStep : ChooseDirectionStep
{
    [SerializeField] string targetKey = "direction";
    private V2 ignoreDir;
    public override bool IsReady => choicePoint.GetPoint() - startPos != ignoreDir;
    public override void Load(V2 position, Dictionary<string, string> info)
    {
        ignoreDir = info.ContainsKey(targetKey) ? (V2)Enum.Parse<Direction>(info[targetKey]) : (0, 0);
        base.Load(position, info); 

        choicePoint.PlaceType = PlaceType.Special;
        choicePoint.specialMethod = () => IsReady;
    }
}
