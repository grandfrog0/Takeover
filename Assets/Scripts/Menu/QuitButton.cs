using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void AskToQuit()
    {
        ConfirmPopup popup = (ConfirmPopup)WindowManager.CreateOpenGet(PrefabBuffer.ConfirmPopup);
        popup.Init("Are you sure you want to quit the game?");
        popup.OnConfirmed.AddListener(Quit);
    }
    public void Quit() => Application.Quit();
}
