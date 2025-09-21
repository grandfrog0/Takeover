using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionedRenderer : MonoBehaviour, IRotatable, IInitializable
{
    private List<DirectionedSprite> sprites = new();

    [SerializeField] Direction _rotationDirection = Direction.Down;
    public Direction RotationDirection
    {
        get => _rotationDirection;
        set
        {
            _rotationDirection = value;
            UpdateSprites();
        }
    }

    public InitializeOrder Order => InitializeOrder.DirectionedRenderer;
    public void Initialize()
    {
        LoadSprites();
        UpdateSprites();
    }

    private void LoadSprites()
    {
        sprites.Clear();
        LoadSprite(transform);
    }
    private void LoadSprite(Transform tr)
    {
        if (tr.TryGetComponent(out DirectionedSprite sprite))
            sprites.Add(sprite);

        for (int i = 0; i < tr.childCount; i++)
            LoadSprite(tr.GetChild(i));
    }

    private void UpdateSprites()
    {
        foreach (DirectionedSprite sprite in sprites)
            sprite.RotationDirection = RotationDirection;
    }
}
