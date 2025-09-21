using System;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Вызывает событие у подписчиков по очереди
/// </summary>
public class KeyCodeChain
{
    public KeyCode KeyCode { get; }
    public bool HasSubsribers => downSubscribers.Count != 0 || upSubscribers.Count != 0 || holdSubscribers.Count != 0;
    public List<UnityAction> downSubscribers;
    public List<UnityAction> upSubscribers;
    public List<UnityAction> holdSubscribers;

    public UnityAction NextDownSubscriber => downSubscribers[^1];
    public UnityAction NextUpSubscriber => upSubscribers[^1];
    public UnityAction NextHoldSubscriber => holdSubscribers[^1];

    public KeyCodeChain(KeyCode keyCode)
    {
        KeyCode = keyCode;

        Inputs.AddDownListener(KeyCode, OnDown);
        Inputs.AddUpListener(KeyCode, OnUp);
        Inputs.AddHoldListener(KeyCode, OnHold);

        downSubscribers = new();
        upSubscribers = new();
        holdSubscribers = new();
    }

    public void AddDownListener(UnityAction action) => downSubscribers.Add(action);
    public void AddUpListener(UnityAction action) => upSubscribers.Add(action);
    public void AddHoldListener(UnityAction action) => holdSubscribers.Add(action);
    public void RemoveDownListener(UnityAction action) => downSubscribers.Remove(action);
    public void RemoveUpListener(UnityAction action) => upSubscribers.Remove(action);
    public void RemoveHoldListener(UnityAction action) => holdSubscribers.Remove(action);

    public void OnDown()
    {
        if (downSubscribers.Count != 0) 
            NextDownSubscriber();
    }
    public void OnUp()
    {
        if (upSubscribers.Count != 0) 
            NextUpSubscriber();
    }
    public void OnHold()
    {
        if (holdSubscribers.Count != 0) 
            NextHoldSubscriber();
    }
}
