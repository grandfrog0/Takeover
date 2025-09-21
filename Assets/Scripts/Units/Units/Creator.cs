using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Creator : UnitBot
{
    public UnityEvent<int> OnTimerChanged = new();

    private CreateInfo createInfo;
    private UnitInfo targetUnit;
    public bool isCreating = false;
    private float _timeLeft;
    public float TimeLeft
    {   
        get => _timeLeft;
        private set
        {
            _timeLeft = value > 0 ? value : 0;
            OnTimerChanged.Invoke((int)value);
        }
    }
    public float TimeNormalized => TimeLeft / createInfo.time;

    [SerializeField] AnimationRequest pumpAnim;

    public override void InitUnit(Dictionary<string, string> args)
    {
        base.InitUnit(args);
        if (TimeLeft > 0) StartBuild(createInfo, false);
    }

    public void StartBuild(CreateInfo createInfo, bool resetTimer = true)
    {
        if (isCreating) return;

        this.createInfo = createInfo;
        try
        {
            targetUnit = UnitsManager.GetInfo(createInfo.unitName);

            if (resetTimer) 
                TimeLeft = createInfo.time;

            pumpAnim.Play();

            isCreating = true;
        }
        catch (Exception e)
        {
            isCreating = false;
            Debug.Log($"Cant create unit from exception: {e}");
        }
    }

    private void EndBuild()
    {
        if (--createInfo.count <= 0)
        {
            TimeLeft = 0;
            isCreating = false;

            pumpAnim.Stop();
        }
        else
        {
            TimeLeft = createInfo.time;
        }

        UnitItem item = UnitItem.Create();
        item.transform.position = transform.position;
        item.Init(targetUnit, 1, createInfo.energyCost);

        Score.Coins -= createInfo.cost;
    }

    protected override void SpeedUpdate()
    {
        base.SpeedUpdate();
        if (isCreating)
        {
            if (Energy > 0 && TimeLeft > 0)
            {
                Energy -= createInfo.energyCost / createInfo.time * deltaTime;
                TimeLeft -= deltaTime;
            }
            if (TimeLeft <= 0)
            {
                if (Score.Coins >= createInfo.cost)
                    EndBuild();
            }
        }
    }

    public override void OnSelected()
    {
        if (SettingsManager.CurrentSettings.autoOpenCreateWindow && !isCreating)
            OpenCreateWindow();
    }
    public override Dictionary<string, string> Deload()
    {
        return new(base.Deload())
        {
            ["create_info"] = createInfo.ToString(),
            ["time_left"] = TimeLeft.ToString(),
        };
    }
    public override string GetValue(string valueName) 
        => valueName switch
        {
            "create_info" => createInfo.ToString(),
            "time_left" => TimeLeft.ToString(),
            _ => base.GetValue(valueName),
        };
    public override void SetValue(string valueName, string value)
    {
        switch (valueName)
        {
            case "create_info":
                createInfo = CreateInfo.Parse(value);
                break;

            case "time_left":
                TimeLeft = float.Parse(value);
                break;

            default:
                base.SetValue(valueName, value);
                break;
        }
    }
    public void OpenCreateWindow()
    {
        ((CreateUnitWindow)WindowManager.CreateOpenGet(PrefabBuffer.CreateUnitWindow)).Init(this);
    }
}
