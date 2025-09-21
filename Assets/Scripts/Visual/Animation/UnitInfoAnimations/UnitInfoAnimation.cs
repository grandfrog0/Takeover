using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitInfoAnimation : MonoBehaviour
{
    public virtual void Play()
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimRepeat());
    }
    public virtual IEnumerator AnimRepeat()
    {
        while (true)
        {
            yield return StartCoroutine(Animation());
            yield return new WaitForSeconds(4);
        }
    }
    public virtual void End()
    {
        gameObject.SetActive(false);
    }
    public abstract IEnumerator Animation();
}
