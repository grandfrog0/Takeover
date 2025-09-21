using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PopupWindow : Window
{
    public override bool IsOpened { get; protected set; }
    private Coroutine openCoroutine, closeCoroutine;
    protected float scaleMultiplier = 1;

    public override bool Open()
    {
        if (IsOpened) return false;

        gameObject.SetActive(true);

        KeyChains.AddDown(KeyCode.Escape, CloseAction, "Close window");

        if (!closeCoroutine.IsUnityNull())
        {
            StopCoroutine(closeCoroutine);
            CloseAnimationEnd();
        }
        openCoroutine = StartCoroutine(OpenAnimation());

        IsOpened = true;
        WindowManager.OnWindowOpened(this);
        OnBeginOpen.Invoke();

        return true;
    }

    public override bool Close()
    {
        if (!IsOpened) return false;

        KeyChains.RemoveDown(KeyCode.Escape, CloseAction);

        if (!openCoroutine.IsUnityNull())
        {
            StopCoroutine(openCoroutine);
            OpenAnimationEnd();
        }
        closeCoroutine = StartCoroutine(CloseAnimaton());

        IsOpened = false;
        OnBeginClose.Invoke();

        return true;
    }

    protected virtual IEnumerator OpenAnimation()
    {
        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 5)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * scaleMultiplier, t);
            yield return null;
        }

        OpenAnimationEnd();
    }
    protected virtual void OpenAnimationEnd()
    {
        transform.localScale = Vector3.one;
        OnOpened.Invoke();
    }

    protected virtual IEnumerator CloseAnimaton()
    {
        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 5)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * scaleMultiplier, Vector3.zero, t);
            yield return null;
        }

        CloseAnimationEnd();
        gameObject.SetActive(false);
    }
    protected virtual void CloseAnimationEnd()
    {
        WindowManager.OnWindowClosed(this);
        OnClosed.Invoke();
        Destroy(gameObject);
    }
}
