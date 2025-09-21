using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateUnitImage : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] RawImage image;
    [SerializeField] HintSeeker seeker;
    public UnitInfo info;

    public void Load(UnitInfo info, Action<UnitInfo> action)
    {
        this.info = info;
        image.texture = UnitImageManager.Images[info].renderTexture;
        button.onClick.AddListener(() => action(info));
        seeker.text = info.unitName;
    }
}
