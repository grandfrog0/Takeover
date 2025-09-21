using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EditableUI : MonoBehaviour
{
    public TMP_Text titleText;
    public Image iconImage;
    public bool WasChanged { get; protected set; }
    public abstract void Init(string title, string value);
    public abstract string GetValue();
}
