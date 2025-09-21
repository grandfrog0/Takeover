using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLabel : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;
    public string Text
    {
        get => text.text;
        set => text.text = value;
    }
}
