using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAnim : AnimationRequest
{
    [SerializeField] float speed = 2;
    [SerializeField] float strength = 1;
    private Vector3 startSize;
    private Vector3 needSize;

    protected override IEnumerator Animation()
    {
        startSize = transform.localScale;
        needSize = startSize * 1.5f * strength;

        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * speed)
        {
            transform.localScale = Vector3.Slerp(startSize, needSize, t);
            yield return null;
        }

        for (float t = 1; t >= 0.5f; t -= Time.fixedDeltaTime * speed)
        {
            transform.localScale = Vector3.Slerp(startSize, needSize, t);
            yield return null;
        }

        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * speed)
        {
            transform.localScale = Vector3.Slerp(startSize, needSize, t);
            yield return null;
        }

        for (float t = 1; t >= 0; t -= Time.fixedDeltaTime * speed)
        {
            transform.localScale = Vector3.Slerp(startSize, needSize, t);
            yield return null;
        }

        EndWithCheck();
    }

    protected override void End()
    {
        base.End();
        transform.localScale = startSize;
    }
}
