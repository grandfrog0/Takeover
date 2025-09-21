using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image image;
    [SerializeField] Sprite one, zero;

    public void Start()
    {
        if (image)
        {
            Validate(slider.value);
            slider.onValueChanged.AddListener(Validate);
        }
    }

    public void Validate(float x) => image.sprite = x != 0 ? one : zero;
}
