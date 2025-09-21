using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Window : MonoBehaviour
{
    public UnityEvent OnOpened { get; protected set; } = new();
    public UnityEvent OnClosed { get; protected set; } = new();
    public UnityEvent OnBeginOpen { get; protected set; } = new();
    public UnityEvent OnBeginClose { get; protected set; } = new();

    public abstract bool IsOpened { get; protected set; }
    public void OpenAction() => Open();
    public void CloseAction() => Close();
    public abstract bool Open();
    public abstract bool Close();
}
