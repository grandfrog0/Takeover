using System.Collections.Generic;
using UnityEngine;

public class ConstLayerRender : MonoBehaviour
{
    [SerializeField] string layer;
    [SerializeField] int addOrder;
    private Dictionary<SpriteRenderer, int> defOrders = new();
    [SerializeField] List<SpriteRenderer> ignore = new();

    private void Start()
    {
        ReloadOrders();
    }

    public void UpdateOrders()
    {
        foreach ((SpriteRenderer sr, int order) in defOrders)
        {
            sr.sortingOrder = order + addOrder;
            sr.sortingLayerName = layer;
        }
    }

    public void ReloadOrders()
    {
        LoadOrders();
        UpdateOrders();
    }

    private void LoadOrders()
    {
        defOrders.Clear();
        LoadOrder(transform);
    }
    private void LoadOrder(Transform tr)
    {
        if (tr.TryGetComponent(out SpriteRenderer sr) && !ignore.Contains(sr))
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
