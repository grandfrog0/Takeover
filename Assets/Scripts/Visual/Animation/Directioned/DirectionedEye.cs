using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionedEye : DirectionedPositioned
{
    [SerializeField] Dict<Direction, Sprite> blinks;
    [SerializeField] Vector2 timerRange = new Vector2(3, 10);
    private Coroutine blink;

    public override void Initialize()
    {
        base.Initialize();

        if (!blink.IsUnityNull()) StopCoroutine(blink);
        blink = StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(timerRange.x, timerRange.y));

            sr.sprite = sprites.ContainsKey(RotationDirection) ? sprites[RotationDirection] : null;

            yield return new WaitForSeconds(0.2f);

            sr.sprite = blinks.ContainsKey(RotationDirection) ? blinks[RotationDirection] : null;

            yield return new WaitForSeconds(0.2f);

            sr.sprite = sprites.ContainsKey(RotationDirection) ? sprites[RotationDirection] : null;
        }
    }
}
