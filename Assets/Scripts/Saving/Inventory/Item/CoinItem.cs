using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Instruments;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoinItem : BaseDropItem
{
    private static HashSet<CoinItem> coins = new();
    public static List<SerializableCoinItem> DeloadCoins()
        => coins.Select(x => new SerializableCoinItem(x)).ToList();
    public static void LoadCoins(List<SerializableCoinItem> list)
    {
        foreach (var x in list)
            Create().InitFromSerializable(x);
    }
    public static CoinItem Create()
        => Instantiate(PrefabBuffer.CoinItem, SceneBuffer.WorldCanvas.transform).GetComponent<CoinItem>();

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
    private CoinItem other;

    public void CheckMerge()
    {
        foreach(CoinItem coin in coins)
            if (coin != this && Vector3.Distance(transform.position, coin.transform.position) <= MergeRadius)
                coin.Merge(this);
    }
    public void Merge(CoinItem other)
    {
        if (IsCombining) return;
        this.other = other;
        IsCombining = true;
        coroutine = StartCoroutine(MergeAnim(other.transform));
    }

    protected override IEnumerator MergeAnim(Transform tr)
    {
        yield return base.MergeAnim(tr);

        other.Initialize(other.Value + Value);
        Destroy(gameObject);
    }

    public void InitFromSerializable(SerializableCoinItem item)
    {
        transform.position = item.pos;
        Initialize(item.value);
    }
    public void Init(int count)
    {
        Initialize(count);
        base.Initialize();
    }
    protected void Initialize(int count)
    {
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
        to = SceneBuffer.ScoreRect.transform;
    }
    public void UpdateValue()
    {
        text.text = Value == 1 ? "" : CountValidator.ToShortText(Value);
        image.localScale = image.localScale.normalized * Mathf.Clamp(0.75f + Mathf.Pow(Value, 1 / 10f), 1, 3);
    }
    public override void GotItem()
    {
        Score.Coins += Value;
        base.GotItem();
    }

    private void OnEnable() => coins.Add(this);
    private void OnDisable() => coins.Remove(this);
}
