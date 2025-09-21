using UnityEngine;

public class DirectionedSprite : MonoBehaviour, IRotatable, IInitializable
{
    private Direction _rotationDirection = Direction.Down;
    public Direction RotationDirection
    {
        get => _rotationDirection;
        set
        {
            _rotationDirection = value;
            UpdateSprite();
        }
    }

    protected SpriteRenderer sr;
    [SerializeField] protected Dict<Direction, Sprite> sprites = new();

    public InitializeOrder Order => InitializeOrder.DirectionedSprite;
    public virtual void Initialize()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void UpdateSprite()
    {
        sr.sprite = sprites.ContainsKey(RotationDirection) ? sprites[RotationDirection] : null;
    }
}
