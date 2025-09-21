using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class ChosenUnitEffect : ChosenEffect
{
    [SerializeField] SpriteRenderer arrowRenderer;
    [SerializeField] Transform arrowTr;
    [SerializeField] Transform infoPopup;

    private Unit curUnit;

    public void SetUnit(Unit unit)
    {
        if (curUnit) curUnit.gridTransform.onMoved.RemoveListener(OnMoved);
        curUnit = unit;
        curUnit.gridTransform.onMoved.AddListener(OnMoved);

        transform.position = unit.transform.position;
        arrowTr.position = unit.transform.position + Vector3.up * (unit.gridTransform.AddHeight + 0.9f * unit.transform.localScale.y);

        infoPopup.gameObject.SetActive(true);
        infoPopup.transform.position = curUnit.transform.position;

        StartColorCoroutine();

        Instantiate(rippleEffect, transform.position, transform.localRotation);
    }

    public void ClearUnit()
    {
        if (curUnit) curUnit.gridTransform.onMoved.RemoveListener(OnMoved);
        curUnit = null;
        StopColorCoroutine();
        sr.color = arrowRenderer.color = Color.clear;
        infoPopup.gameObject.SetActive(false);
    }

    private void OnMoved(V2 old, V2 x)
    {
        transform.position = infoPopup.transform.position = x;
        arrowTr.position = (Vector3)x + Vector3.up * (curUnit.gridTransform.AddHeight + 0.9f * curUnit.transform.localScale.y);
    }

    protected override IEnumerator ColorCoroutine()
    {
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * speed)
        {
            sr.color = arrowRenderer.color = Gradient.Evaluate(t);
            infoPopup.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t * 2);
            yield return null;
        }
        sr.color = arrowRenderer.color = Gradient.Evaluate(1);
    }
}