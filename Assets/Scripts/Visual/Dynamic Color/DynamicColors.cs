using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicColors : MonoBehaviour
{
    List<UIDynamicColor> colors;

    public void PointerEnter()
    {
        foreach (var c in colors)
            c.PointerEnter();
    }
    public void PointerExit()
    {
        foreach (var c in colors)
            c.PointerExit();
    }

    private void Start()
    {
        colors = new();
        GetChilds(transform);
    }
    private void GetChilds(Transform tr)
    {
        if (tr.TryGetComponent(out UIDynamicColor color))
            colors.Add(color);

        for (int i = 0; i < tr.childCount; i++)
            GetChilds(tr.GetChild(i));
    }
}
