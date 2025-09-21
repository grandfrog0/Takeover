using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintSeeker : MonoBehaviour
{
    public string text;
    public void SelectHint() => Hint.SelectHint(text);
    public void DeselectHint() => Hint.DeselectHint();
}
