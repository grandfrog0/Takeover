using System;
using UnityEngine;

[Serializable]
public class CameraInfo
{
    public V2 position;
    public float size;

    public CameraInfo()
    {
        position = (0, 0);
        size = (int)(CameraMove.defaultSize * 1000) / 1000f;
    }
    public CameraInfo(Camera cam)
    {
        position = cam.transform.position;
        size = (int)(cam.orthographicSize * 1000) / 1000f;
    }
    public override string ToString() => $"{position}, {size}";
}

