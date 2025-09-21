using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmPopup : PopupWindow
{
    public UnityEvent OnCancelled { get; } = new();
    public UnityEvent OnConfirmed { get; } = new();
    public bool IsConfirmed { get; private set; }
    [SerializeField] TMPro.TMP_Text text;

    public void SetConfirmed(bool value)
    {
        IsConfirmed = value;

        if (value)
            OnConfirmed.Invoke();
        else
            OnCancelled.Invoke();

        Close();
    }
    public void Confirm() => SetConfirmed(true);
    public void Cancel() => SetConfirmed(false);

    public void Init(string description)
    {
        text.text = description;
    }

    public override bool Open()
    {
        if (base.Open())
        {
            KeyChains.AddDown(KeyCode.Return, Confirm, "Confirm");
            KeyChains.AddDown(KeyCode.Escape, Cancel, "Cancel");
            return true;
        }
        return false;
    }
    public override bool Close()
    {
        KeyChains.RemoveDown(KeyCode.Return, Confirm);
        KeyChains.RemoveDown(KeyCode.Escape, Cancel);
        return base.Close();
    }
    protected override IEnumerator CloseAnimaton()
    {
        yield return base.CloseAnimaton();
        Destroy(gameObject);
    }
}
