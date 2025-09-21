using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Instruments;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreateUnitWindow : PopupWindow
{
    [SerializeField] RegistredUnits createableUnits;

    [SerializeField] TMP_Text coinsScore, energyScore;
    [SerializeField] TMP_Text startText;

    [SerializeField] RawImage unitImage;
    [SerializeField] TMP_Text unitNameText;

    [SerializeField] Slider countSlider;
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text shortTime, longTime, energyCost, coinsCost;

    [SerializeField] RectTransform choice;
    [SerializeField] Transform unitImagesParent;
    [SerializeField] GameObject unitImagePrefab;
    private List<(UnitInfo, CreateUnitImage)> units;

    public UnitInfo curUnit;
    public CreateInfo result;

    private Creator thisCreator;

    private int count = 1;
    private int cost = 0;
    private int enCost = 0;
    private int time;

    public void Init(Creator creator)
    {
        units = new();

        thisCreator = creator;
        energyScore.text = creator.Energy.ToString();
        creator.OnEnergyChanged.AddListener(UpdateEnergyScore);

        coinsScore.text = Score.Coins.ToString();
        Score.OnCoinsScoreChanged.AddListener(UpdateScore);

        foreach (UnitInfo info in createableUnits.unitInfo)
        {
            CreateUnitImage im = Instantiate(unitImagePrefab, unitImagesParent).GetComponent<CreateUnitImage>();
            im.Load(info, SetUnit);
            units.Add((info, im));
        }

        SetUnit(units[0].Item1);
        OnOpened.AddListener(() => SetUnit(units[0].Item1));

        result = new()
        {
            unitName = curUnit.unitName,
            count = count,
            energyCost = (int)energySlider.value,
            time = time,
            cost = curUnit.config.buildCost
        };
    }

    public void UpdateEnergyScore(float _) => energyScore.text = thisCreator.Energy.ToString();
    public void UpdateScore(int x) => coinsScore.text = x.ToString();

    public void UpdateCost()
    {
        count = (int)countSlider.value;

        cost = curUnit.config.buildCost * count;
        enCost = (int)energySlider.value * count;

        coinsCost.text = cost.ToString();
        energyCost.text = enCost.ToString();

        startText.color = cost <= Score.Coins ? Color.white : Color.red;
    }

    public void SetUnit(UnitInfo info)
    {
        curUnit = info;

        choice.SetParent(units.Where(x => x.Item1 == info).First().Item2.transform);
        choice.SetAsFirstSibling();
        choice.anchoredPosition = Vector3.zero;

        unitImage.texture = UnitImageManager.Images[info].renderTexture;
        unitNameText.text = info.unitName;

        if (info.config.maxEnergy != 0)
        {
            energySlider.minValue = info.config.fillEnergy;
            energySlider.maxValue = info.config.maxEnergy;
        }
        else
        {
            energySlider.minValue = 0;
            energySlider.maxValue = info.config.fillEnergy;
        }

        time = info.config.buildTime;
        shortTime.text = TimeValidator.ToShortText(time / 2);
        longTime.text = TimeValidator.ToShortText(time);

        UpdateCost();
    }

    public void InfoButtonClicked() => ((GeneralUnitInfoWindow)WindowManager.CreateOpenGet(PrefabBuffer.GenUnitInfoWindow)).Init(curUnit);

    public void StartButtonClicked()
    {
        if (cost <= Score.Coins)
        {
            result.unitName = curUnit.unitName;
            result.count = count;
            result.energyCost = (int)energySlider.value;
            result.time = time;
            result.cost = curUnit.config.buildCost;

            thisCreator.StartBuild(result);

            Close();
        }
        else
        {
            Debug.Log("Too expensive!");
        }
    }

    public override bool Close()
    {
        Score.OnCoinsScoreChanged.RemoveListener(UpdateScore);
        thisCreator.OnEnergyChanged.RemoveListener(UpdateEnergyScore);
        return base.Close();
    }
}
