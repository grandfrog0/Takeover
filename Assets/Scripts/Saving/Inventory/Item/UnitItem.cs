using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Instruments;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitItem : BaseDropItem
{
    private static HashSet<UnitItem> units = new();
    public static List<SerializableUnitItem> DeloadUnits()
        => units.Select(x => new SerializableUnitItem(x)).ToList();
    public static void LoadUnits(List<SerializableUnitItem> list)
    {
        foreach(var x in list)
            Create().InitFromSerializable(x);
    }

    public static UnitItem Create()
        => Instantiate(PrefabBuffer.UnitItem, SceneBuffer.WorldCanvas.transform).GetComponent<UnitItem>();

    [SerializeField] Image energyImage;
    [SerializeField] RawImage rawImage;
    protected override float MergeRadius => transform.localScale.x / 2 * Mathf.Pow(2, 0.5f);
    public UnitInfo UnitInfo { get; set; }

    [SerializeField] TMP_Text energyText;
    [SerializeField] float _energy;
    public float Energy
    {
        get => _energy;
        set
        {
            _energy = value;
            if (_energy < 0)
                _energy = 0;
        }
    }

    [SerializeField] TMP_Text text;
    [SerializeField] int _value;
    public int Value 
    {
        get => _value; 
        set
        {
            _value = value;
            if (_value < 0) 
                _value = 0;
        } 
    }
    private UnitItem other;

    public void CheckMerge()
    {
        foreach(UnitItem unit in units)
            if (Vector3.Distance(transform.position, unit.transform.position) <= MergeRadius 
                && unit.UnitInfo == UnitInfo && unit.Energy == Energy && unit != this)
                unit.Merge(this);
    }
    public void Merge(UnitItem other)
    {
        if (IsCombining) return;
        this.other = other;
        IsCombining = true;
        coroutine = StartCoroutine(MergeAnim(other.transform));
    }
    protected override IEnumerator MergeAnim(Transform tr)
    {
        yield return base.MergeAnim(tr);

        other.Initialize(UnitInfo, other.Value + Value, Energy);
        Destroy(gameObject);
    }

    public void InitFromSerializable(SerializableUnitItem item)
    {
        transform.position = item.pos;
        Initialize(UnitsManager.GetInfo(item.unitName), item.value, item.energy);
    }
    public void Init(UnitInfo info, int count, float energy)
    {
        Initialize(info, count, energy);
        base.Initialize();
    }
    protected void Initialize(UnitInfo info, int count, float energy)
    {
        UnitInfo = info;
        rawImage.texture = UnitImageManager.Images[info].renderTexture;
        Energy = energy;

        Value = count;
        UpdateValue();

        Initialize();
    }
    protected override void OnDrop()
    {
        base.OnDrop();
        CheckMerge();
    }
    protected override void Initialize()
    {
        to = SceneBuffer.InventoryRect.transform;
    }
    public void UpdateValue()
    {
        text.text = Value == 1 ? "" : CountValidator.ToShortText(Value);
        energyText.text = Energy == 0 ? "" : CountValidator.ToShortText((int)Energy);
        energyImage.enabled = Energy != 0;
    }
    public override void GotItem()
    {
        InventoryManager.AddUnitCount(new Item(UnitInfo, new() { ["fill_energy"] = Energy.ToString() }), Value);
        base.GotItem();
    }

    private void OnEnable() => units.Add(this);
    private void OnDisable() => units.Remove(this);
}
