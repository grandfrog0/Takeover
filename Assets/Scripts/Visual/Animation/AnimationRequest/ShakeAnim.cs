using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnim : AnimationRequest
{
    [SerializeField] float strength = 1;
    [SerializeField] float time = 0.5f;
    Vector3 start;

    protected override IEnumerator Animation()
    {
        start = transform.localPosition;


        for (int i = 0; i < time / 0.125f; i++)
        {
            Vector3 need = (Vector2)transform.localPosition + Random.insideUnitCircle * strength;
            for (float t = 0; t <= 0.125f; t += Time.fixedDeltaTime)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, need, t);
                yield return null;
            }
        }

        EndWithCheck();
    }

    protected override void End()
    {
        base.End();
        transform.localPosition = start;
    }
}
