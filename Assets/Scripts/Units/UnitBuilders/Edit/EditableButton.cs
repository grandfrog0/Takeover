using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableButton: EditableUI
{
    public Toggle toggle;
    private Action action;

    public void Init(string title, Action action)
    {
        titleText.text = title;
        toggle.IsOn = false;
        toggle.onChanged.AddListener(x => WasChanged = x);
        this.action = action;
    }
    public void Invoke() => action();

    public override void Init(string _, string __) { }
    public override string GetValue() => "";
}
