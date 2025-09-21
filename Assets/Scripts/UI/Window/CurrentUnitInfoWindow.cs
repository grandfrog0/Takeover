using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CurrentUnitInfoWindow : PopupWindow
{
    [SerializeField] InfoLabel health, energy, power;
    [SerializeField] TMPro.TMP_Text unitName, description;
    [SerializeField] RawImage image;
    private UnitInfo info;
    private Unit unit;
    private UnitBot unitBot;
    private UnityAction deleteAction;

    public void Init()
    {
        unit.OnHealthChanged.AddListener(UpdateHealth);

        health.Text = $"{unit.Health:F0}/{info.config.maxHealth}";
        if (unit is UnitBot bot)
        {
            unitBot = bot;
            bot.OnEnergyChanged.AddListener(UpdateEnergy);
            bot.OnPowerChanged.AddListener(UpdatePower);

            energy.Text = $"{bot.Energy:F0}/{info.config.maxEnergy}";
            power.Text = $"{bot.SpeedModifier:F2}/{info.config.maxSpeed}";
        }
        else energy.Text = power.Text = "-";

        unitName.text = info.unitName;
        description.text = info.unitDescription;

        if (UnitImageManager.AnimImages.TryGetValue(info, out RawUnitImage im))
        {
            im.SetActive(true);
            im.spawnedModel.GetComponent<UnitInfoAnimation>().Play();
            image.texture = im.renderTexture;
            image.SetNativeSize();
        }
        else
        {
            image.texture = UnitImageManager.Images[info].renderTexture;
            image.SetNativeSize();
        }
    }

    private void UpdateHealth(float _)
    {
        if (unit && info)
            health.Text = $"{unit.Health:F0}/{info.config.maxHealth}";
    }
    private void UpdateEnergy(float _)
    {
        if (unitBot && info)
            energy.Text = $"{unitBot.Energy:F0}/{info.config.maxEnergy}";
    }
    private void UpdatePower(float _)
    {
        if (unitBot && info)
            power.Text = $"{unitBot.SpeedModifier:F2}/{info.config.maxSpeed}";
    }

    public override bool Open()
    {
        if (base.Open())
        {
            unit = UnitsManager.CurUnit;
            if (unit)
            {
                info = UnitsManager.GetInfo(unit.UnitName);
                if (info) Init();
            }
            return true;
        }
        return false;
    }

    public void EditButtonClicked() => WindowManager.CreateOpen(PrefabBuffer.EditUnitWindow);
    public void DeleteButtonClicked()
    {
        ConfirmPopup popup = WindowManager.CreateOpenGet(PrefabBuffer.ConfirmPopup) as ConfirmPopup;
        deleteAction = () =>
        {
            popup.OnClosed.RemoveListener(deleteAction);
            if (popup.IsConfirmed)
            {
                Close();
                GridUnit.DeleteUnit(UnitsManager.CurUnit);
                UnitsManager.ClearCurUnit();
            }
        };
        popup.OnClosed.AddListener(deleteAction);
    }

    public override bool Close()
    {
        if (info && UnitImageManager.AnimImages.TryGetValue(info, out RawUnitImage im))
        {
            im.spawnedModel.GetComponent<UnitInfoAnimation>().End();
            im.SetActive(false);
            info = null;
        }

        if (unit)
        {
            unit.OnHealthChanged.RemoveListener(UpdateHealth);
            if (unitBot)
            {
                unitBot.OnEnergyChanged.RemoveListener(UpdateEnergy);
                unitBot.OnPowerChanged.RemoveListener(UpdatePower);
            }
        }

        return base.Close();
    }
}
