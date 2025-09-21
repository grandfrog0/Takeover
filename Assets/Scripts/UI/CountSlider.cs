using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void Increase() => slider.value++;
    public void Decrease() => slider.value--;
}
