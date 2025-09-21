using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitBar : MonoBehaviour
{
    [SerializeField] ValueBar healthBar;
    [SerializeField] HeartbeatAnim heartbeat;
    [SerializeField] List<Image> images;

    public float HealthPercent
    {
        get => healthBar.Value;
        set
        {
            if (healthBar.Value > value)
                heartbeat.Play();

            healthBar.Value = value;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual void SetOpacity(float value)
    {
        foreach (Image im in images)
            im.color = new Color(im.color.r, im.color.g, im.color.b, value);
    }
    public void SetTransparent() => SetOpacity(5);
    public void SetUntransparent() => SetOpacity(40);
}
