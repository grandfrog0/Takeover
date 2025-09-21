using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseDropItem : MonoBehaviour
{
    protected virtual float MergeRadius => image.localScale.x / 2;
    public bool IsCollected { get; protected set; }
    public bool IsGot { get; protected set; }
    public bool IsCombining { get; protected set; }
    [SerializeField] protected Transform to;
    [SerializeField] protected Transform image;
    private float dropRange = 100f;
    protected Coroutine coroutine;

    protected virtual void Initialize()
    {
        if (gameObject.activeInHierarchy) coroutine = StartCoroutine(DropAnim());
    }
    protected virtual IEnumerator MergeAnim(Transform tr)
    {
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, tr.position, t);
            yield return null;
        }
    }

    protected IEnumerator DropAnim()
    {
        Vector3 start = transform.localPosition;
        Vector3 need = start + (Vector3)Random.insideUnitCircle * dropRange;
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * 2)
        {
            transform.localPosition = Vector3.Lerp(start, need + Vector3.up * Mathf.Sin(Mathf.PI * t) * dropRange,t);
            yield return null;
        }
        OnDrop();
    }

    protected virtual void OnDrop() { }

    public void OnMouseDown()
    {
        if (!IsCollected && !IsCombining)
            Clicked();
    }
    public void OnMouseEnter()
    {
        if (Inputs.IsMousePressed() && !IsCollected && !IsCombining)
            Clicked();
    }

    public virtual void Clicked()
    {
        if (!coroutine.IsUnityNull()) StopCoroutine(coroutine);
        IsCollected = true;
        transform.SetParent(SceneBuffer.Canvas.transform);
        StartCoroutine(CollectionAnimation());
    }

    protected virtual IEnumerator CollectionAnimation()
    {
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, to.localPosition, t);
            yield return null;
        }

        GotItem();
    }

    public virtual void GotItem()
    {
        IsGot = true;
        Destroy(gameObject);
    }
}
