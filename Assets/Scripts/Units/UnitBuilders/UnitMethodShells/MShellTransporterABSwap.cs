using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MShellTransporterABSwap : UnitMethodShell
{
    public override void Method(Unit unit)
    {
        if (unit is Transporter transporter)
        {
            Direction temp = transporter.DragPoint;
            transporter.DragPoint = transporter.DropPoint;
            transporter.DropPoint = temp;
        }
        else Debug.Log("This method is only for transporter unit!");
    }
}
