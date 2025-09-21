using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditableTriggerButton: EditableUI
{
    public Button button;
    public void Init(string title, UnityAction action)
    {
        titleText.text = title;
        button.onClick.AddListener(action);
    }

    public override void Init(string _, string __) { }
    public override string GetValue() => null;
}
