using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionedHands : DirectionedPositioned
{
    [SerializeField] bool IsReady;

    [SerializeField] LayeredRender lr;
    [SerializeField] Dict<Direction, Vector3> readyPoses;

    public override void UpdateSprite()
    {
        base.UpdateSprite();
        transform.localPosition = IsReady ? 
            (readyPoses.ContainsKey(RotationDirection) ? readyPoses[RotationDirection] : Vector3.zero) :
            (poses.ContainsKey(RotationDirection) ? poses[RotationDirection] : Vector3.zero);
    }

    public void SetReady(bool value)
    {
        IsReady = value;

        transform.localPosition = IsReady ?
            (readyPoses.ContainsKey(RotationDirection) ? readyPoses[RotationDirection] : Vector3.zero) :
            (poses.ContainsKey(RotationDirection) ? poses[RotationDirection] : Vector3.zero);

        lr.SetOrder(sr, value ? LayeredRender.OrdersPerUnit + 1 : -1);
    }
}
