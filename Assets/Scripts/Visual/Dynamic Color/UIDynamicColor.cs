using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIDynamicColor : MonoBehaviour
{
    [SerializeField] protected Color anotherColor;
    [SerializeField] protected Color defaultColor;
    public abstract void PointerEnter();
    public abstract void PointerExit();
}
