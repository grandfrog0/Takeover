using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Events;

public class CreatorInfoAnimation : UnitInfoAnimation
{
    [SerializeField] CreatorBar bar;
    [SerializeField] UnitItem item;
    [SerializeField] UnitInfo targetUnit;
    public float timeMax;
    private float _timeLeft;
    public float TimeLeft
    {
        get => _timeLeft;
        private set
        {
            _timeLeft = value > 0 ? value : 0;
            bar.TimerValue = (int)value;
        }
    }
    public float TimeNormalized => TimeLeft / timeMax;

    [SerializeField] AnimationRequest pumpAnim;

    public void StartBuild()
    {
        timeMax = targetUnit.config.buildTime;
        item.gameObject.SetActive(false);

        pumpAnim.Play();
    }

    private void EndBuild()
    {
        TimeLeft = 0;

        pumpAnim.Stop();

        item.gameObject.SetActive(true);
        item.transform.position = transform.position;
        item.Init(targetUnit, 1, 0);
    }


    public override void End()
    {
        base.End();
        EndBuild();
    }

    public override IEnumerator Animation()
    {
        StartBuild();

        for (TimeLeft = 3; TimeLeft > 0; TimeLeft -= Creator.deltaTime)
            yield return new WaitForSeconds(Creator.deltaTime);

        EndBuild();
    }
}
