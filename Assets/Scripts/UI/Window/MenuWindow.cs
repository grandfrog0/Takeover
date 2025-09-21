using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuWindow : PopupWindow
{
    public override bool Open()
    {
        if (base.Open())
        {
            PauseManager.IsPaused = true;
            SceneBuffer.PauseButton.SetActive(false);

            return true;
        }
        return false;
    }
    public override bool Close()
    {
        PauseManager.IsPaused = false;
        SceneBuffer.PauseButton.SetActive(true);

        return base.Close();
    }
}
