using System;
using UnityEngine;

[Serializable]
public struct V2
{
    public static V2[] Directions => new V2[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
    public static V2 Parse(string a)
    {
        // Удаляем скобки и разбиваем по запятой
        string[] parts = a.Trim(' ', '(', ')').Split(',');

        if (parts.Length == 2 && 
            int.TryParse(parts[0].Trim(), out int x) && int.TryParse(parts[1].Trim(), out int y))
            return new V2(x, y);
        else throw new FormatException($"Cant format string {a} to V2 object.");
    }

    [SerializeField] public int x;
    [SerializeField] public int y;

    public V2(int x, int y) => (this.x, this.y) = (x, y);

    public static implicit operator Vector2(V2 v2) => new Vector2(v2.x, v2.y);
    public static implicit operator Vector2Int(V2 v2) => new Vector2Int(v2.x, v2.y);
    public static implicit operator Vector3(V2 v2) => new Vector3(v2.x, v2.y, 0);
    public static implicit operator V2(Vector2 v2) => new V2((int)v2.x, (int)v2.y);
    public static implicit operator V2(Vector2Int v2) => new V2(v2.x, v2.y);
    public static implicit operator V2(Vector3 v2) => new V2((int)v2.x, (int)v2.y);
    public static implicit operator V2((int, int) v2) => new V2(v2.Item1, v2.Item2);
    public static explicit operator Direction(V2 v2)
    {
        if (v2 == new V2(1, 0)) return Direction.Right;
        else if (v2 == new V2(-1, 0)) return Direction.Left;
        else if (v2 == new V2(0, 1)) return Direction.Up;
        else if (v2 == new V2(0, -1)) return Direction.Down;
        else return Direction.Down;
    }
    public static explicit operator V2(Direction dir)
        => dir switch
        {
            Direction.Up => (0, 1),
            Direction.Down => (0, -1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0, 0)
        };

    public static V2 operator +(V2 a, V2 b) => new(a.x + b.x, a.y + b.y);
    public static V2 operator -(V2 a, V2 b) => new(a.x - b.x, a.y - b.y);
    public static V2 operator *(V2 a, int b) => new(a.x * b, a.y * b);
    public static V2 operator /(V2 a, int b) => new(a.x / b, a.y / b);
    public static bool operator ==(V2 a, V2 b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(V2 a, V2 b) => !(a.x == b.x && a.y == b.y);
    public override bool Equals(object obj) => this == (V2)obj;
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => $"({x}, {y})";
}
