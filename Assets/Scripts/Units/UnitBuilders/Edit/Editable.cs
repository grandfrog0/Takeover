using System;
using UnityEngine;

[Serializable]
public class Editable
{
    public string title;
    public string valueName;
    public EditableType EditableType;
    public UnitBuildStep step;
    public UnitMethodShell shell;
    public Sprite customSprite;
}