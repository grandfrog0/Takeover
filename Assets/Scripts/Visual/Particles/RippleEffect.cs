using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] float speed;
    [SerializeField] bool destroy = true;
    private Color startColor;
    private Vector3 startScale;

    private void FixedUpdate()
    {
        transform.localScale += Vector3.one * speed * Time.fixedDeltaTime;
        sr.color -= Color.black * speed * Time.fixedDeltaTime;

        if (destroy && sr.color.a <= 0)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (!sr)
        {
            sr = GetComponent<SpriteRenderer>();
            startColor = sr.color;
            startScale = transform.localScale;
        }
        else
        {
            sr.color = startColor;
            transform.localScale = startScale;
        }
    }
}
