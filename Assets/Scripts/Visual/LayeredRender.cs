using System.Collections.Generic;
using UnityEngine;

public class LayeredRender : MonoBehaviour
{
    public static int OrdersPerUnit => 8;

    private IPositionable positionable;
    private Dictionary<SpriteRenderer, int> defOrders = new();

    private void Start()
    {
        LoadOrders();

        positionable ??= GetComponent<IPositionable>();
        positionable.AddMovedListener((_, _) => UpdateOrders());

        UpdateOrders();
    }

    public void UpdateOrders()
    {
        int delta = positionable.Position.y * OrdersPerUnit;

        foreach ((SpriteRenderer sr, int order) in defOrders)
            sr.sortingOrder = order - delta;
    }

    private void LoadOrders()
    {
        defOrders.Clear();
        LoadOrder(transform);
    }
    private void LoadOrder(Transform tr)
    {
        if (tr.TryGetComponent(out SpriteRenderer sr))
            defOrders[sr] = sr.sortingOrder;

        for (int i = 0; i < tr.childCount; i++)
            LoadOrder(tr.GetChild(i));
    }

    public void SetOrder(SpriteRenderer sr, int value)
    {
        defOrders[sr] = value;
        UpdateOrders();
    }
}
