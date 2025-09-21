using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AnimationRequest : MonoBehaviour
{
    public bool OncePerTime = true;
    public bool IsRepeating = false;
    public bool IsPlaying { get; private set; }
    private Coroutine animationCoroutine;
    public void Play()
    {
        if (OncePerTime && IsPlaying) 
            return;

        Stop();
        if (gameObject.activeInHierarchy)
        {
            IsPlaying = true;
            animationCoroutine = StartCoroutine(Animation());
        }
    }
    protected abstract IEnumerator Animation();
    protected virtual void End()
    { 
        IsPlaying = false;
    }
    protected virtual void EndWithCheck()
    {
        End();
        if (IsRepeating) Play();
    }
    public void Stop()
    {
        if (!animationCoroutine.IsUnityNull())
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
            End();
        }
    }
}
