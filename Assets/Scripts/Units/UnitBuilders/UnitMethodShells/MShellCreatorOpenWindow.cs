using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MShellCreatorOpenWindow : UnitMethodShell
{
    public override void Method(Unit unit)
    {
        if (unit is Creator creator)
        {
            ((CreateUnitWindow)WindowManager.CreateOpenGet(PrefabBuffer.CreateUnitWindow)).Init(creator);
        }
        else Debug.Log("This method is only for creator unit!");
    }
}
