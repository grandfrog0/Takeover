using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class FormattedText : MonoBehaviour
{
    public string format = "{0}";
    [SerializeField] TMPro.TMP_Text text;
    private void OnValidate() => text ??= GetComponent<TMPro.TMP_Text>();
    public void SetText(string value) => text.text = string.Format(format, value);
    public void SetValue(float value) => text.text = string.Format(format, value);
}
