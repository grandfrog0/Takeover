using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Instruments;
using TMPro;
using UnityEngine;

public class CreatorBar : UnitBotBar
{
    [SerializeField] ValueBar timerBar;
    [SerializeField] TMP_Text timerText;
    [SerializeField] ShakeAnim timerCall;
    [SerializeField] GameObject timerObj;

    private int _timerValue;
    public int TimerValue
    {
        get => _timerValue;
        set
        {
            _timerValue = value;
            timerText.text = TimeValidator.ToShortText(value);

            if (value < 15)
                timerCall.Play();

            timerObj.SetActive(value != 0);
        }
    }
    public float TimerNormalized
    {
        get => timerBar.Value;
        set => timerBar.Value = value;
    }

    public override void SetOpacity(float value)
    {
        base.SetOpacity(value);
        timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, value);
    }
}
