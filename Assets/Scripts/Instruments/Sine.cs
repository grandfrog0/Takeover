using UnityEngine;

public class Sine
{
    public float Speed { get; set; }
    public float Magnitude { get; set; }
    public float Value => Mathf.Sin(Time.time * Speed) * Magnitude;
    public float NormalizedValue => (Mathf.Sin(Time.time * Speed) + 1) / 2;
    public Sine(float speed = 1, float magnitude = 1)
    {
        Speed = speed;
        Magnitude = magnitude;
    }
}