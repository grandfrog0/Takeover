using System;
using System.Collections;
using UnityEngine;

public class BuildChoicePoint : ChosenEffect
{
    [SerializeField] GridTransform grid;
    [SerializeField] SpriteRenderer imageSr;
    public PlaceType PlaceType { get; set; } = PlaceType.All;
    [SerializeField] Gradient _badGradient;
    protected override Gradient Gradient { get => HasPoint ? _gradient : _badGradient; }
    public bool HasPoint => PlaceType switch
    {
        PlaceType.None => false,
        PlaceType.EmptyOnly => !GridUnit.HasUnit(grid.Position),
        PlaceType.CanPlaceUnit => GridUnit.CanPlaceUnit(grid.Position),
        PlaceType.UnitsOnly => GridUnit.HasUnit(grid.Position),
        PlaceType.All => true,
        PlaceType.Special => specialMethod(),
        _ => true
    };
    public Func<bool> specialMethod;

    protected void Start()
    {
        StartColorCoroutine();
        Instantiate(rippleEffect, transform.position, transform.localRotation);
    }

    public void SetPoint(V2 point)
    {
        if (grid)
        grid.Position = point;
    }
    public void AddPoint(V2 point)
    {
        if (grid)
        grid.Position += point;
    }

    public void SetSprite(Sprite sprite)
    {
        imageSr.sprite = sprite;
        StartColorCoroutine();
    }

    public V2 GetPoint() => grid.Position;

    public void ClearPoint()
    {
        sr.color = imageSr.color = Gradient.Evaluate(0);
    }

    protected virtual IEnumerator SineColorCoroutine()
    {
        Sine sine = new(speed * 5);
        while (true)
        {
            sr.color = imageSr.color = Gradient.Evaluate(sine.NormalizedValue);
            yield return null;
        }
    }

    protected override IEnumerator ColorCoroutine()
    {
        for (float t = 0; t <= 1; t += Time.fixedDeltaTime * speed)
        {
            sr.color = imageSr.color = Gradient.Evaluate(t);
            yield return null;
        }
        sr.color = imageSr.color = Gradient.Evaluate(1);
        StartCoroutine(SineColorCoroutine());
    }
}