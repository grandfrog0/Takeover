using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Inputs : MonoBehaviour, IInitializable
{
    private static Inputs inst;

    static Dictionary<KeyCode, InputEvent> keys;
    static Dictionary<int, KeyCode> mouseKeys = new()
    {
        [0] = KeyCode.Mouse0,
        [1] = KeyCode.Mouse1,
        [2] = KeyCode.Mouse2,
    };
    static UnityEvent<float> scrollEvent = new();

    public InitializeOrder Order => InitializeOrder.Inputs;


    public void Update()
    {
        foreach((KeyCode key, InputEvent input) in keys)
        {
            if (Input.GetKeyDown(key))
                input.OnKeyDown();
            else if (Input.GetKey(key))
                input.OnKeyHold();
            else if (Input.GetKeyUp(key))
                input.OnKeyUp();
        }

        if (Input.mouseScrollDelta.y != 0)
            scrollEvent.Invoke(Input.mouseScrollDelta.y);
    }

    public static bool IsPressed(KeyCode key)
        => keys.TryGetValue(key, out InputEvent input) ? input.IsPressed : Input.GetKeyDown(key);

    public static bool IsMousePressed()
        => Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);

    public static bool IsPointerOnUI(bool includeWorldSpace = false)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
            if (canvas != null && (includeWorldSpace || canvas.renderMode != RenderMode.WorldSpace))
                return true;
        }

        return false;
    }
    public static bool IsMouseInsideScreen() => MyMath.IsInsideRect(Input.mousePosition, 0, Screen.width, 0, Screen.height);

    public static void AddDownListener(KeyCode keyCode, UnityAction action)
    {
        if (!keys.ContainsKey(keyCode))
            keys[keyCode] = new();

        keys[keyCode].OnDown.AddListener(action);
    }
    public static void RemoveDownListener(KeyCode keyCode, UnityAction action)
    {
        if (keys.ContainsKey(keyCode))
            keys[keyCode].OnDown.RemoveListener(action);
    }
    public static void AddUpListener(KeyCode keyCode, UnityAction action)
    {
        if (!keys.ContainsKey(keyCode))
            keys[keyCode] = new();

        keys[keyCode].OnUp.AddListener(action);
    }
    public static void RemoveUpListener(KeyCode keyCode, UnityAction action)
    {
        if (keys.ContainsKey(keyCode))
            keys[keyCode].OnUp.RemoveListener(action);
    }
    public static void AddHoldListener(KeyCode keyCode, UnityAction action)
    {
        if (!keys.ContainsKey(keyCode))
                keys[keyCode] = new();

        keys[keyCode].OnHold.AddListener(action);
    }
    public static void RemoveHoldListener(KeyCode keyCode, UnityAction action)
    {
        if (keys.ContainsKey(keyCode))
            keys[keyCode].OnHold.RemoveListener(action);
    }

    public static void AddMouseDownListener(int mouse, UnityAction action)
        => keys[mouseKeys[mouse]].OnDown.AddListener(action);
    public static void AddMouseUpListener(int mouse, UnityAction action)
        => keys[mouseKeys[mouse]].OnUp.AddListener(action);
    public static void AddMouseHoldListener(int mouse, UnityAction action)
        => keys[mouseKeys[mouse]].OnHold.AddListener(action);

    public static void AddMoveDownListener(UnityAction<V2> action)
    {
        AddDownListener(KeyCode.W, () => action((0, 1)));
        AddDownListener(KeyCode.A, () => action((-1, 0)));
        AddDownListener(KeyCode.S, () => action((0, -1)));
        AddDownListener(KeyCode.D, () => action((1, 0)));
    }
    public static void RemoveMoveDownListener(UnityAction<V2> action)
    {
        RemoveDownListener(KeyCode.W, () => action((0, 1)));
        RemoveDownListener(KeyCode.A, () => action((-1, 0)));
        RemoveDownListener(KeyCode.S, () => action((0, -1)));
        RemoveDownListener(KeyCode.D, () => action((1, 0)));
    }
    public static void AddMoveUpListener(UnityAction<V2> action)
    {
        AddUpListener(KeyCode.W, () => action((0, 1)));
        AddUpListener(KeyCode.A, () => action((-1, 0)));
        AddUpListener(KeyCode.S, () => action((0, -1)));
        AddUpListener(KeyCode.D, () => action((1, 0)));
    }
    public static void RemoveMoveUpListener(UnityAction<V2> action)
    {
        RemoveUpListener(KeyCode.W, () => action((0, 1)));
        RemoveUpListener(KeyCode.A, () => action((-1, 0)));
        RemoveUpListener(KeyCode.S, () => action((0, -1)));
        RemoveUpListener(KeyCode.D, () => action((1, 0)));
    }
    public static void AddMoveHoldListener(UnityAction<V2> action)
    {
        AddHoldListener(KeyCode.W, () => action((0, 1)));
        AddHoldListener(KeyCode.A, () => action((-1, 0)));
        AddHoldListener(KeyCode.S, () => action((0, -1)));
        AddHoldListener(KeyCode.D, () => action((1, 0)));
    }
    public static void RemoveMoveHoldListener(UnityAction<V2> action)
    {
        RemoveHoldListener(KeyCode.W, () => action((0, 1)));
        RemoveHoldListener(KeyCode.A, () => action((-1, 0)));
        RemoveHoldListener(KeyCode.S, () => action((0, -1)));
        RemoveHoldListener(KeyCode.D, () => action((1, 0)));
    }

    public static void AddScrollListener(UnityAction<float> action)
    {
        scrollEvent.AddListener(action);
    }

    public void Initialize()
    {
        inst = this;

        keys = new()
        {
            [KeyCode.Mouse0] = new(false),
            [KeyCode.Mouse1] = new(false),
            [KeyCode.Mouse2] = new(false),

            [KeyCode.Escape] = new(),
            [KeyCode.Return] = new(),
        };

        keys[KeyCode.W] = keys[KeyCode.UpArrow] = new();
        keys[KeyCode.A] = keys[KeyCode.LeftArrow] = new();
        keys[KeyCode.S] = keys[KeyCode.DownArrow] = new();
        keys[KeyCode.D] = keys[KeyCode.RightArrow] = new();
    }
}