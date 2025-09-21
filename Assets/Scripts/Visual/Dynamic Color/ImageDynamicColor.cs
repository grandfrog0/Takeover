using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDynamicColor : UIDynamicColor
{
    private Image image;
    public override void PointerEnter() => image.color = anotherColor;
    public override void PointerExit() => image.color = defaultColor;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = defaultColor;
    }
}
