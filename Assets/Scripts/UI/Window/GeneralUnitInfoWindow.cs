using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUnitInfoWindow : PopupWindow
{
    [SerializeField] InfoLabel health, energy, power, cost, energyCost;
    [SerializeField] TMPro.TMP_Text unitName, description;
    [SerializeField] RawImage image;
    private UnitInfo info;

    public void Init(UnitInfo info)
    {
        this.info = info;

        health.Text = info.config.maxHealth.ToString();
        energy.Text = info.config.maxEnergy.ToString();
        power.Text = info.config.maxSpeed.ToString();
        cost.Text = info.config.buildCost.ToString();
        energyCost.Text = info.config.fillEnergy.ToString();

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

    public override bool Close()
    {
        if (info && UnitImageManager.AnimImages.TryGetValue(info, out RawUnitImage im))
        {
            im.spawnedModel.GetComponent<UnitInfoAnimation>().End();
            im.SetActive(false);
            info = null;
        }

        return base.Close();
    }
}
