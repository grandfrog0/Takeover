using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChoosePositionStep : UnitBuildStep
{
    public V2 Position => choicePoint ? choicePoint.GetPoint() : startPos;

    [SerializeField] GameObject choicePrefab;
    [SerializeField] BuildChoicePoint choicePoint;
    private V2 startPos;
    private UnityAction<V2> action;
    private bool withoutNotify = false;

    public GameObject Model { get; set; }

    public override bool IsReady => !GridUnit.HasUnit(Position);


    public void LoadWithoutNotify(V2 position)
    {
        withoutNotify = true;

        startPos = position;

        choicePoint = Instantiate(choicePrefab, startPos, Quaternion.identity).GetComponent<BuildChoicePoint>();
        choicePoint.SetPoint(position);

        Model = Instantiate(Model, choicePoint.transform.position + Vector3.up * 0.125f, Quaternion.identity, choicePoint.transform);

        choicePoint.PlaceType = PlaceType.EmptyOnly;
        choicePoint.GetComponent<ConstLayerRender>().ReloadOrders();
        Destroy(choicePoint.gameObject);
    }

    public override void Load(V2 position, Dictionary<string, string> _)
    {
        if (withoutNotify) return;

        startPos = position;

        choicePoint = Instantiate(choicePrefab, startPos, Quaternion.identity).GetComponent<BuildChoicePoint>();
        choicePoint.SetPoint(position);

        Model = Instantiate(Model, choicePoint.transform.position + Vector3.up * 0.125f, Quaternion.identity, choicePoint.transform);

        choicePoint.PlaceType = PlaceType.EmptyOnly;
        choicePoint.GetComponent<ConstLayerRender>().ReloadOrders();

        action = choicePoint.AddPoint;
        Inputs.AddMoveDownListener(action);
    }

    public override void Deload()
    {
        if (withoutNotify) return;

        Inputs.RemoveMoveDownListener(action);
        startPos = Position;
        if (choicePoint) Destroy(choicePoint.gameObject);
    }

    public override object GetResult() => Position;
    public override string GetName() => "position";
}