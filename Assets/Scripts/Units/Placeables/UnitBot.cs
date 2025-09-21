using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitBot : Unit
{
    public const float deltaTime = 0.5f;

    // Events
    public UnityEvent<float> OnPowerChanged = new();
    public UnityEvent<float> OnEnergyChanged = new();

    // Parameters
    // Power
    [SerializeField] float maxSpeedModifier;
    [SerializeField] float speedIncrease;
    private float _speedModifier = 1;
    public float SpeedModifier
    {
        get => _speedModifier;
        set
        {
            _speedModifier = Mathf.Clamp(value, 1, maxSpeedModifier);
            OnPowerChanged.Invoke(PowerPercent);
        }
    }
    public float PowerPercent => maxSpeedModifier != 1 ? (SpeedModifier - 1) / (maxSpeedModifier - 1) : 1;
    // Energy
    [SerializeField] bool _energyReduction = true;
    public bool EnergyReduction { get => _energyReduction; private set => _energyReduction = value; }
    private float maxEnergy = 1;
    private float fillEnergy = 1;
    [SerializeField] float _energy;
    public float Energy
    {
        get => _energy;
        protected set
        {
            _energy = value;
            if (maxEnergy != 0 && _energy > maxEnergy)
            {
                _energy = maxEnergy;
                Debug.Log("It is the limit!!");
                // TODO: return cost back
            }
            if (_energy < 0) _energy = 0;
            OnEnergyChanged.Invoke(EnergyPercent);
        }
    }
    public float EnergyPercent => _energy / fillEnergy;

    // Hidden
    private Coroutine _speedUpdate, _myUpdate;

    public override void Initialize()
    {
        base.Initialize();
        _speedUpdate = StartCoroutine(SpeedUpdateCoroutine());
        _myUpdate = StartCoroutine(MyUpdateCoroutine());
    }
    public override void InitUnit(Dictionary<string, string> args)
    {
        base.InitUnit(args); // initialize, INITDEFAULTS, load, ...
        _energy = fillEnergy;
    }
    protected override void InitDefaults()
    {
        base.InitDefaults();

        speedIncrease = 0.1f;
        maxSpeedModifier = 2f;
        maxEnergy = 1f;
        fillEnergy = 1f;
    }
    private IEnumerator SpeedUpdateCoroutine(float interval = deltaTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval / SpeedModifier);
            SpeedUpdate();
        }
    }
    private IEnumerator MyUpdateCoroutine(float interval = deltaTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            MyUpdate();
        }
    }
    protected virtual void SpeedUpdate()
    {
        if (EnergyReduction)
        {
            if (Energy > 0) Energy -= deltaTime;
        }
    }
    protected virtual void MyUpdate()
    {
        SpeedModifier -= speedIncrease / 5f;
    }

    public override bool OnClicked()
    {
        bool res = base.OnClicked();

        if (!res)
        {
            SpeedModifier += speedIncrease;
            return true;
        }

        return false;
    }

    protected override void OnDead()
    {
        base.OnDead();

        if (!_myUpdate.IsUnityNull()) StopCoroutine(_myUpdate);
        if (!_speedUpdate.IsUnityNull()) StopCoroutine(_speedUpdate);
    }

    public override string GetValue(string valueName)
    {
        return valueName switch
        {
            "speed_increase" => speedIncrease.ToString(),
            "max_speed" => maxSpeedModifier.ToString(),
            "max_energy" => maxEnergy.ToString(),
            "fill_energy" => fillEnergy.ToString(),
            "speed_modifier" => SpeedModifier.ToString(),
            "energy" => Energy.ToString(),
            "energy_reduction" => EnergyReduction.ToString(),
            _ => base.GetValue(valueName)
        };
    }
    public override void SetValue(string valueName, string value)
    {
        switch(valueName)
        {
            case "speed_increase":
                speedIncrease = float.Parse(value);
                break;

            case "max_speed":
                maxSpeedModifier = float.Parse(value);
                break;

            case "max_energy":
                maxEnergy = float.Parse(value);
                break;

            case "fill_energy":
                fillEnergy = float.Parse(value);
                break;

            case "speed_modifier":
                SpeedModifier = float.Parse(value);
                break;

            case "energy":
                Energy = float.Parse(value);
                break;

            case "energy_reduction":
                EnergyReduction = bool.Parse(value);
                break;

            default:
                base.SetValue(valueName, value);
                break;
        }
    }
    public override Dictionary<string, string> Deload()
    {
        return new(base.Deload())
        {
            ["speed_increase"] = speedIncrease.ToString(),
            ["max_speed"] = maxSpeedModifier.ToString(),
            ["speed_modifier"] = SpeedModifier.ToString(),
            ["max_energy"] = maxEnergy.ToString(),
            ["fill_energy"] = fillEnergy.ToString(),
            ["energy"] = Energy.ToString(),
            ["energy_reduction"] = EnergyReduction.ToString(),
        };
    }
}
