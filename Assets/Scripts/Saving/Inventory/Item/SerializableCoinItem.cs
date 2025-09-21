
using System;
using UnityEngine;

[Serializable]
public class SerializableCoinItem
{
    public Vector2 pos;
    public int value;

    public SerializableCoinItem(CoinItem coinItem)
    {
        pos = coinItem.transform.position;
        value = coinItem.Value;
    }
}

