using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TransporterInfoAnimation : UnitInfoAnimation
{
    [SerializeField] List<V2> points;
    [SerializeField] GridTransform gridTransform;
    [SerializeField] GridTransform tr;
    [SerializeField] DirectionedRenderer dr;
    [SerializeField] Direction DragPoint, DropPoint;
    private V2 startPos;

    public override IEnumerator Animation()
    {
        Initializator.InitObject(dr.gameObject);

        dr.RotationDirection = DragPoint;
        yield return new WaitForSeconds(0.5f);

        V2 lastPos = startPos;
        foreach (V2 dir in points)
        {
            if (Direction.Down.TryParse(dir, out Direction rotDir))
                dr.RotationDirection = rotDir;

            for (float t = 0; t < 1; t += Time.fixedDeltaTime * 2)
            {
                tr.transform.position = Vector3.Slerp(lastPos, gridTransform.Position + dir, t);
                yield return null;
            }
            lastPos = gridTransform.Position + dir;
        }

        tr.Position = lastPos;

        yield return new WaitForSeconds(0.5f);

        dr.RotationDirection = Direction.Down;
    }

    public override IEnumerator AnimRepeat()
    {
        startPos = tr.Position;
        while (true)
        {
            yield return StartCoroutine(Animation());
            yield return new WaitForSeconds(4);
            tr.Position = startPos;
        }
    }

    public override void End()
    {
        base.End();
        tr.Position = startPos;
    }
}
