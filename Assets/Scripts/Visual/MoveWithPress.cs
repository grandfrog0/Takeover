using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveWithPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Transform tr;
    [SerializeField] Vector3 to;
    [SerializeField] bool listenRMB;
    private Vector3 start;

    private void Awake()
    {
        start = tr.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left ||
            (listenRMB && eventData.button == PointerEventData.InputButton.Right))
            tr.localPosition = start + to;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left ||
            (listenRMB && eventData.button == PointerEventData.InputButton.Right))
            tr.localPosition = start;
    }
}
