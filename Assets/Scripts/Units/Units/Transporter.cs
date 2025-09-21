using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.CanvasScaler;

[RequireComponent(typeof(GridTransform))]
public class Transporter : UnitBot
{
    public UnityEvent onDrag = new();
    public UnityEvent onDrop = new();

    private DirectionedRenderer dr;

    [SerializeField] bool _canDrag = true;
    public bool CanDrag { get => _canDrag; private set => _canDrag = value; }

    [SerializeField] Direction _dragPoint;
    [SerializeField] Direction _dropPoint;
    public Direction DragPoint { get => _dragPoint; set => _dragPoint = value; }
    public Direction DropPoint { get => _dropPoint; set => _dropPoint = value; }

    // drag coroutine
    private Coroutine _dragCoroutine;
    private DragInfo _dragInfo;

    protected override void InitDefaults()
    {
        base.InitDefaults();
        CanDrag = true;
    }
    public override string GetValue(string valueName)
    {
        return valueName switch
        {
            "direction" => DragPoint.ToString(),
            "direction_1" => DropPoint.ToString(),
            "is_active" => CanDrag.ToString(),
            _ => base.GetValue(valueName)
        };
    }
    public override void SetValue(string valueName, string value)
    {
        switch (valueName)
        {
            case "direction":
                DragPoint = Enum.Parse<Direction>(value);
                break;

            case "direction_1":
                DropPoint = Enum.Parse<Direction>(value);
                break;

            case "is_active":
                CanDrag = bool.Parse(value);
                break;

            default:
                base.SetValue(valueName, value);
                break;
        }
    }
    public override Dictionary<string, string> Deload()
    {
        return new(base.Deload())
        {
            ["direction"] = DragPoint.ToString(),
            ["direction_1"] = DropPoint.ToString(),
            ["is_active"] = CanDrag.ToString(),
        };
    }

    public override void Initialize()
    {
        base.Initialize();
        dr = GetComponent<DirectionedRenderer>();
    }

    private void TryDrag()
    {
        if (!CanDrag || EnergyPercent <= 0) return;
        if (HasNeighbor((V2)DragPoint, out Unit unit))
            _dragCoroutine = StartCoroutine(DraggingCoroutine(unit));
    }
    private IEnumerator DraggingCoroutine(Unit unit)
    {
        if (!unit.CanPlace(gridTransform.Position + (V2)DropPoint, out _))
            yield break;

        HashSet<V2> obstacles = new();
        for (int y = -1; y <= 1; y++)
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;

                V2 pos = gridTransform.Position + (x, y);
                if (GridUnit.HasUnit(pos))
                    obstacles.Add((x, y));
            }

        List<V2> points = MyMath.FindTransporterPath((V2)DragPoint, (V2)DropPoint, obstacles);
        if (points == null || points.Count == 0)
            yield break;

        onDrag.Invoke();

        unit.gridTransform.IsMoving = true;

        _dragInfo = new(unit);

        _dragInfo.startPos = unit.gridTransform.Position;

        dr.RotationDirection = DragPoint;
        yield return new WaitForSeconds(0.5f / SpeedModifier);

        _dragInfo.lastPos = _dragInfo.startPos;
        foreach (V2 dir in points)
        {
            if (Direction.Down.TryParse(dir, out Direction rotDir))
                dr.RotationDirection = rotDir;

            for (float t = 0; t < 1; t += Time.fixedDeltaTime * 2 * SpeedModifier)
            {
                unit.transform.position = Vector3.Slerp(_dragInfo.lastPos, gridTransform.Position + dir, t);
                yield return null;
            }
            _dragInfo.lastPos = gridTransform.Position + dir;
        }

        _dragInfo.hasObstanceOnThePath = unit.HasObstacle(gridTransform.Position + (V2)DropPoint, out _);
        if (_dragInfo.hasObstanceOnThePath)
        {
            points.Reverse();
            foreach (V2 dir in points)
            {
                if (Direction.Down.TryParse(dir, out Direction rotDir))
                    dr.RotationDirection = rotDir;

                for (float t = 0; t < 1; t += Time.fixedDeltaTime * 2 * SpeedModifier)
                {
                    unit.transform.position = Vector3.Slerp(_dragInfo.lastPos, gridTransform.Position + dir, t);
                    yield return null;
                }

                _dragInfo.lastPos = gridTransform.Position + dir;
            }
        }
        unit.gridTransform.IsMoving = false;
        unit.gridTransform.Position = _dragInfo.TargetPos;

        onDrop.Invoke();

        yield return new WaitForSeconds(0.5f / SpeedModifier);

        dr.RotationDirection = Direction.Down;

        _dragInfo = null;
    }
    protected override void OnDestroy()
    {
        if (!_dragCoroutine.IsUnityNull()) 
        {
            StopCoroutine(_dragCoroutine);
            _dragCoroutine = null;
        }
        if (_dragInfo != null)
        {
            _dragInfo.unit.gridTransform.IsMoving = false;
            _dragInfo.unit.gridTransform.Position = _dragInfo.TargetPos;

            onDrop.Invoke();
            dr.RotationDirection = Direction.Down;

            _dragInfo = null;
        }
        base.OnDestroy();
    }
    protected override void SpeedUpdate()
    {
        base.SpeedUpdate();
        TryDrag();
    }
}
