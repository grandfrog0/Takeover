using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChosenEffect : MonoBehaviour
{
    [SerializeField] protected GameObject rippleEffect;
    [SerializeField] protected Gradient _gradient;
    protected virtual Gradient Gradient { get => _gradient; set => _gradient = value; }
    [SerializeField] protected float speed = 2;

    protected Coroutine coroutine;
    [SerializeField] protected SpriteRenderer sr;

    public void StartColorCoroutine()
    {
        StopColorCoroutine();
        coroutine = StartCoroutine(ColorCoroutine());
    }
    public void StopColorCoroutine()
    {
        if (!coroutine.IsUnityNull()) 
            StopCoroutine(coroutine);
    }

    protected virtual IEnumerator ColorCoroutine()
    {
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * speed)
        {
            sr.color = Gradient.Evaluate(t);
            yield return null;
        }
        sr.color = Gradient.Evaluate(1);
    }
}