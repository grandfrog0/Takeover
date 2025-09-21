using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditableToggle : EditableUI
{
    public Toggle toggle;
    public override void Init(string title, string value)
    {
        titleText.text = title;

        if (bool.TryParse(value, out bool val))
        {
            toggle.IsOn = val.Equals(true);
        }
        else Debug.Log(title);

        toggle.onChanged.AddListener(_ => WasChanged = true);
    }
    public override string GetValue() => toggle.IsOn.ToString();
}
