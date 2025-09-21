using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class TextDynamicColor : UIDynamicColor
{
    private TMPro.TMP_Text text;
    public override void PointerEnter() => text.color = anotherColor;
    public override void PointerExit() => text.color = defaultColor;

    private void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
        text.color = defaultColor;
    }
}
