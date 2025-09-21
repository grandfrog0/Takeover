using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] float pixelPerUnit = 0.08f;
    [SerializeField] Image fillImage;

    private float _value;
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            fillImage.fillAmount = (int)(value / pixelPerUnit) * pixelPerUnit;
        }
    }
}
