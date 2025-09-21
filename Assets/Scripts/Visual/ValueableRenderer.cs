using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValueableRenderer : MonoBehaviour
{
    // from - enable
    [SerializeField] Dict<float, GameObject> sprites;
    private float value = 1;
    public void SetValue(float value)
    {
        List<float> keys = sprites.Keys.ToList();
        keys.Sort();

        foreach (float key in keys)
        {
            if (key >= value)
            {
                sprites[this.value].SetActive(false);
                this.value = key;
                sprites[key].SetActive(true);
                Initializator.InitObject(sprites[key]);
                break;
            }
        }
    }
    private void Start()
    {
        foreach (GameObject obj in sprites.Values)
            obj.SetActive(false);
        SetValue(1);
    }
}
