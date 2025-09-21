using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseDirectionStep : UnitBuildStep
{
    [SerializeField] BuildChoicePoint choicePrefab;
    [SerializeField] Sprite sprite;
    protected BuildChoicePoint choicePoint;
    protected V2 startPos;
    private UnityAction<V2> action;

    public override bool IsReady => true;

    public override void Load(V2 position, Dictionary<string, string> _)
    {
        startPos = position;

        choicePoint = Instantiate(choicePrefab, position + (0, -1), Quaternion.identity).GetComponent<BuildChoicePoint>();
        choicePoint.SetSprite(sprite);
        choicePoint.SetPoint(position + (0, -1));
        choicePoint.PlaceType = PlaceType.All;

        action = x => choicePoint.SetPoint(x + position);
        Inputs.AddMoveDownListener(action);
    }

    public override void Deload()
    {
        Inputs.RemoveMoveDownListener(action);
        if (choicePoint) Destroy(choicePoint.gameObject);
    }

    public override object GetResult() => (Direction)(choicePoint.GetPoint() - startPos);
    public override string GetName() => "direction";
}
