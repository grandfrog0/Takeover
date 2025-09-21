using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionedPositioned : DirectionedSprite
{
    [SerializeField] protected Dict<Direction, Vector3> poses;

    public override void UpdateSprite()
    {
        base.UpdateSprite();
        transform.localPosition = poses.ContainsKey(RotationDirection) ? poses[RotationDirection] : Vector3.zero;
    }
}
