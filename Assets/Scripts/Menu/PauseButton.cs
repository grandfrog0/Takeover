using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour, IInitializable
{
    private Button button;
    public InitializeOrder Order => InitializeOrder.MenuManager;
    public void Initialize()
    {
        button = GetComponent<Button>();
        KeyChains.AddDown(KeyCode.Escape, button.onClick.Invoke);
    }
}
