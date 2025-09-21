using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    public UnityEvent<bool> onChanged { get; } = new();

    [SerializeField] Image image;
    [SerializeField] Sprite on, off;
    private bool _isOn = true;
    public bool IsOn
    { 
        get => _isOn; 
        set
        {
            _isOn = value;
            image.sprite = _isOn ? on : off;
            onChanged.Invoke(value);
        }
    }
    public void Switch() => IsOn = !IsOn;
}
