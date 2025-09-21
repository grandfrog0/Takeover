using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePerTime : MonoBehaviour
{
    private SpriteRenderer sr;

    public Dict<Sprite, Vector2> spriteRanges;

    private float curTime;
    private float timer;
    private int cur;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    protected void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= curTime)
        {
            timer -= curTime;

            cur = ++cur % spriteRanges.Count;
            curTime = MyMath.RandRange(spriteRanges[cur].value);
            sr.sprite = spriteRanges[cur].key;
        }
    }
}
