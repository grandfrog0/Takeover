using UnityEngine;
using UnityEngine.Events;

public class InputEvent
{
    public UnityEvent OnHold { get; private set; }
    public UnityEvent OnDown { get; private set; }
    public UnityEvent OnUp { get; private set; }

    public bool IsPressed { get; private set; }

    public float HoldDelay { get; set; }
    public float HoldPeriod { get; set; }
    public float HoldTime { get; private set; }
    public bool CanHold { get; private set; }

    public InputEvent(float holdDelay = 0.5f) : this(true, holdDelay) { }
    public InputEvent(bool canHold, float holdDelay = 0.5f)
    {
        CanHold = canHold;

        HoldDelay = holdDelay;
        HoldPeriod = 0.075f;

        OnHold = new();
        OnDown = new();
        OnUp = new();

        IsPressed = false;
    }

    public void OnKeyDown()
    {
        HoldTime = 0;
        IsPressed = true;
        OnDown.Invoke();
    }
    public void OnKeyUp()
    {
        HoldTime = 0;
        IsPressed = false;
        OnUp.Invoke();
    }
    public void OnKeyHold()
    {
        OnHold.Invoke();

        if (!CanHold) return;

        if (HoldTime > HoldDelay)
        {
            HoldTime -= HoldPeriod;
            OnDown.Invoke();
        }

        HoldTime += Time.deltaTime;
    }
}
