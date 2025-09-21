using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlayedUI : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> _onChanged = new();
    public UnityEvent<bool> OnChanged => _onChanged;

    [SerializeField] Transform target;
    public Vector3 StartPosition { get; set; }

    public void OnTargetChanged(bool value)
    {
        if (value)          transform.localPosition = StartPosition;
        else if (target)    transform.position      = target.position;

        OnChanged.Invoke(value);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        OnChanged.Invoke(value);
    }

    private void Start()
    {
        StartPosition = transform.localPosition;
    }
}
