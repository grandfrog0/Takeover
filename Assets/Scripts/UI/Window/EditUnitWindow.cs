using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EditUnitWindow : PopupWindow
{
    // Unit and his name
    [SerializeField] TMPro.TMP_Text unitName;
    [SerializeField] RawImage image;

    // Parameters to edit
    [SerializeField] GameObject noParamsText;
    [SerializeField] GameObject editableLabel, editableToggle, editableButton, editableTriggerButton;
    [SerializeField] Transform editablesParent;
    [SerializeField] List<EditableUI> editablesUI = new();
    private List<UnitMethodShell> shells = new();
    private Dictionary<string, object> editResult;

    // Private
    private UnitInfo info;
    private Unit unit;
    private UnityAction deleteAction;

    public void Init()
    {
        // Unit and his name
        unitName.text = info.unitName;
        if (UnitImageManager.Images.TryGetValue(info, out RawUnitImage im))
            image.texture = im.renderTexture;

        // Parameters to edit
        if (info.editables == null || info.editables.editables.Count == 0)
        {
            noParamsText.SetActive(true);
        }
        else
        {
            noParamsText.SetActive(false);

            foreach(Editable editable in info.editables.editables)
            {
                if (editable.EditableType == EditableType.Toggle)
                {
                    EditableToggle ui = Instantiate(editableToggle, editablesParent).GetComponent<EditableToggle>();
                    ui.Init(editable.title, unit.GetValue(editable.valueName));
                    editablesUI.Add(ui);

                    if (editable.customSprite != null)
                    {
                        ui.iconImage.sprite = editable.customSprite;
                        ui.iconImage.SetNativeSize();
                    }
                }
                else if (editable.EditableType == EditableType.BuildStep)
                {
                    EditableLabel ui = Instantiate(editableLabel, editablesParent).GetComponent<EditableLabel>();
                    ui.Init(editable.title, unit.GetValue(editable.valueName));
                    ui.button.onClick.AddListener(() => Edit(editable.step, editable.valueName));
                    editablesUI.Add(ui);

                    if (editable.customSprite != null)
                    {
                        ui.iconImage.sprite = editable.customSprite;
                        ui.iconImage.SetNativeSize();
                    }
                }
                else if (editable.EditableType == EditableType.Method)
                {
                    EditableButton ui = Instantiate(editableButton, editablesParent).GetComponent<EditableButton>();
                    ui.Init(editable.title, () => Call(editable.shell));
                    editablesUI.Add(ui);

                    if (editable.customSprite != null)
                    {
                        ui.iconImage.sprite = editable.customSprite;
                        ui.iconImage.SetNativeSize();
                    }
                }
                else if (editable.EditableType == EditableType.TriggerMethod)
                {
                    EditableTriggerButton ui = Instantiate(editableTriggerButton, editablesParent).GetComponent<EditableTriggerButton>();
                    ui.Init(editable.title, () => Call(editable.shell));
                    editablesUI.Add(ui);

                    if (editable.customSprite != null)
                    {
                        ui.iconImage.sprite = editable.customSprite;
                        ui.iconImage.SetNativeSize();
                    }
                }
            }
        }
    }

    private void Edit(UnitBuildStep step, string valueName)
    {
        Close();
        UnitsManager.StartEdit(unit, step, valueName, unit.gridTransform.Position);
    }

    private void Call(UnitMethodShell shell)
    {
        var s = Instantiate(shell);
        shells.Add(s);
        s.Method(unit);
    }

    public override bool Close()
    {
        if (info?.editables?.editables != null)
            for (int i = 0; i < info.editables.editables.Count; i++)
                if (editablesUI[i].WasChanged)
                {
                    if (editablesUI[i] is EditableToggle)
                        unit.SetValue(info.editables.editables[i].valueName, editablesUI[i].GetValue());
                    else if (editablesUI[i] is EditableButton b)
                        b.Invoke();
                }

        for (int i = 0; i < editablesUI.Count; i++)
            Destroy(editablesUI[i].gameObject);
        editablesUI.Clear();

        for (int i = 0; i < shells.Count; i++)
            Destroy(shells[i].gameObject);
        shells.Clear();

        return base.Close();
    }

    public override bool Open()
    {
        if (base.Open())
        {
            unit = UnitsManager.CurUnit;
            if (unit)
            {
                info = UnitsManager.GetInfo(unit.UnitName);
                if (info) Init();
            }
            return true;
        }
        return false;
    }
    public void InfoButtonClicked() => WindowManager.CreateOpen(PrefabBuffer.CurUnitInfoWindow);
    public void DeleteButtonClicked()
    {
        ConfirmPopup popup = WindowManager.CreateOpenGet(PrefabBuffer.ConfirmPopup) as ConfirmPopup;
        deleteAction = () =>
        {
            popup.OnClosed.RemoveListener(deleteAction);
            if (popup.IsConfirmed)
            {
                Close();
                GridUnit.DeleteUnit(UnitsManager.CurUnit);
                UnitsManager.ClearCurUnit();
            }
        };
        popup.OnClosed.AddListener(deleteAction);
    }
}
