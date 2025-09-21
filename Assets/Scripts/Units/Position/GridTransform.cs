using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GridTransform : MonoBehaviour, IPositionable
{
    private V2 validatePosition;

    [SerializeField] float _height = 1;
    public float Height => _height * transform.localScale.y;
    public float AddHeight => (_height - 0.5f) * transform.localScale.y;

    public UnityEvent<V2, V2> onMoved { get; private set; } = new();

    private V2 _oldPosition;
    public V2 Position
    {
        get => !IsMoving ? transform.position : _oldPosition;
        set
        {
            transform.position = value;

            if (value != _oldPosition) onMoved?.Invoke(_oldPosition, value);

            _oldPosition = value;
        }
    }

    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            if (value) _oldPosition = Position;
            else if (_oldPosition != Position) onMoved.Invoke(_oldPosition, Position);
            _isMoving = value;
        }
    }

    public bool HasAnimation => !moveCoroutine.IsUnityNull();
    private Coroutine moveCoroutine;
    /// <summary>
    /// Set position with animation
    /// </summary>
    public void MoveToAsync(V2 pos)
    {
        if (!moveCoroutine.IsUnityNull()) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine(Position, pos));
    }
    /// <summary>
    /// Add vector to position with animation
    /// </summary>
    public void MoveAsync(V2 add)
    {
        if (!moveCoroutine.IsUnityNull()) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine(Position, Position + add));
    }

    private IEnumerator MoveCoroutine(V2 start, V2 end, float time = 0.5f)
    {
        for (float t = 0; t < 1; t += Time.fixedDeltaTime / time)
        {
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        Position = end;
    }

    public void AddMovedListener(UnityAction<V2, V2> action)
        => onMoved.AddListener(action);
    public void RemoveMovedListener(UnityAction<V2, V2> action)
        => onMoved.RemoveListener(action);

    private void OnValidate()
    {
        if (Position != validatePosition)
        {
            onMoved?.Invoke(validatePosition, Position);

            validatePosition = Position;
        }
    }
}
