using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitImageManager : MonoBehaviour, IInitializable
{
    public static Dictionary<UnitInfo, RawUnitImage> Images { get; private set; } = new();
    public static Dictionary<UnitInfo, RawUnitImage> AnimImages { get; private set; } = new();

    [SerializeField] GameObject rawPrefab;
    [SerializeField] Transform parent;
    [SerializeField] Transform animParent;

    public InitializeOrder Order => InitializeOrder.UnitImageManager;
    public void Initialize()
    {
        int i = 0;
        int a = 0;
        foreach (UnitInfo info in UnitsManager.RegistredUnits.unitInfo)
        {
            Images[info] = Instantiate(rawPrefab, parent).GetComponent<RawUnitImage>();
            Images[info].Init(info.model, new Vector3(i++ * 3, 0), parent);

            if (info.infoAnimation)
            {
                RawUnitImage im = AnimImages[info] = Instantiate(rawPrefab, animParent).GetComponent<RawUnitImage>();
                im.Init(info.infoAnimation.gameObject, new Vector3(a++ * 15, 10), animParent, 1.5f, new(800, 320));
                im.SetActive(false);
            }
        }
    }
}
