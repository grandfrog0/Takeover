using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Instruments;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour, IInitializable
{
    private static Score inst;

    public static UnityEvent<int> OnCoinsScoreChanged { get; } = new();
    private static float _coins = 300;
    public static float Coins 
    { 
        get => _coins; 
        set 
        { 
            _coins = value; 
            OnCoinsScoreChanged.Invoke((int)_coins);
        } 
    }

    [SerializeField] TMP_Text scoreText;
    public InitializeOrder Order => InitializeOrder.InventoryManager;
    public void Initialize()
    {
        inst = this;
        OnCoinsScoreChanged.AddListener(x => scoreText.text = CountValidator.ToShortText(x));
        Coins = AppDataLoader.LoadedData?.coins ?? Coins;
    }
}
