using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAnim : AnimationRequest
{
    [SerializeField] float strength = 1.25f;
    [SerializeField] float time = 0.5f;
    Vector3 startPos, startScale;

    protected override IEnumerator Animation()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;

        Vector3 vertical = new Vector3(startScale.x / strength, startScale.y * strength, 1);

        for (float t = 0; t <= 1; t += Time.fixedDeltaTime / time)
        {
            transform.localScale = Vector3.Lerp(startScale, vertical, t);
            transform.localPosition = Vector3.Lerp(startPos, startPos + Vector3.up * (vertical.y - startScale.y) / 2, t);
            yield return null;
        }
        Vector3 pos = transform.localPosition;

        Vector3 horizontal = new Vector3(startScale.x * strength, startScale.y / strength, 1);

        for (float t = 0; t <= 1; t += Time.fixedDeltaTime / time)
        {
            transform.localScale = Vector3.Lerp(vertical, horizontal, t);
            transform.localPosition = Vector3.Lerp(pos, startPos - Vector3.up * (vertical.y - startScale.y) / 2, t);
            yield return null;
        }
        pos = transform.localPosition;

        for (float t = 0; t <= 1; t += Time.fixedDeltaTime / time)
        {
            transform.localScale = Vector3.Lerp(horizontal, startScale, t);
            transform.localPosition = Vector3.Lerp(pos, startPos, t);
            yield return null;
        }

        EndWithCheck();
    }

    protected override void End()
    {
        base.End();
        transform.localPosition = startPos;
        transform.localScale = startScale;
    }
}
