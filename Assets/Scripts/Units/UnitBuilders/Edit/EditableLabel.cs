using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditableLabel : EditableUI
{
    public TMP_Text valueText;
    public Button button;
    private string value;

    public override void Init(string title, string value)
    {
        this.value = value;

        titleText.text = title;
        valueText.text = value.ToString();
    }
    public override string GetValue()
    {
        return value;
    }
}
