using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBotBar : UnitBar
{
    [SerializeField] ValueBar energyBar;
    [SerializeField] ValueBar powerBar;
    [SerializeField] ShakeAnim energyLow;
    public float EnergyPercent
    {
        get => energyBar.Value;
        set
        {
            energyBar.Value = value;

            if (value < 0.5f)
                energyLow.Play();
        }
    }
    public float PowerPercent
    {
        get => powerBar.Value;
        set => powerBar.Value = value;
    }
}
