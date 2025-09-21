using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains important objects from scene
/// </summary>
public class SceneBuffer : MonoBehaviour, IInitializable
{
    private static SceneBuffer inst;
    public static GameObject Canvas => inst._canvas;
    public static GameObject WorldCanvas => inst._worldCanvas;
    public static GameObject ScoreRect => inst._scoreRect;
    public static GameObject InventoryRect => inst._inventoryRect;
    public static OverlayedUI PauseButton => inst._pauseButton;


    [Header("Canvas")]
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject _worldCanvas;
    [Header("UI Rects")]
    [SerializeField] GameObject _scoreRect;
    [SerializeField] GameObject _inventoryRect;
    [Header("UI Buttons")]
    [SerializeField] OverlayedUI _pauseButton;
    public InitializeOrder Order => InitializeOrder.Buffers;
    public void Initialize() => inst = this;
}
