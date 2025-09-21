using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawUnitImage : MonoBehaviour
{
    private Camera renderCamera;
    public RenderTexture renderTexture;
    public GameObject spawnedModel { get; private set; }

    public void SetActive(bool val)
    {
        renderCamera.gameObject.SetActive(val);
        spawnedModel.gameObject.SetActive(val);
    }

    public void Init(GameObject unitModelPrefab, Vector3 pos, Transform parent, float camSize = 1, V2 addRenderTextureSize = default)
    {
        if (addRenderTextureSize == default) 
            addRenderTextureSize = new(128, 128);

        // Создаём Render Texture (128x128 для оптимизации)
        renderTexture = new RenderTexture(addRenderTextureSize.x, addRenderTextureSize.y, 16);
        renderTexture.Create();

        // Создаём камеру
        GameObject cameraObj = new GameObject("ItemCamera");
        cameraObj.transform.parent = parent;
        cameraObj.transform.position = pos;
        renderCamera = cameraObj.AddComponent<Camera>();
        renderCamera.targetTexture = renderTexture;
        renderCamera.cullingMask = LayerMask.GetMask("UIModel", "Global Light"); // Рендерит только предметы на этом слое
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.clear;

        // Спавним модель
        spawnedModel = Instantiate(unitModelPrefab);
        spawnedModel.transform.parent = parent;
        spawnedModel.transform.position = pos;
        SetLayerChilds(spawnedModel.transform);

        // Настраиваем камеру под модель
        renderCamera.transform.position = spawnedModel.transform.position + new Vector3(0, 0, -2);
        renderCamera.orthographic = true;
        renderCamera.orthographicSize = camSize;
    }

    private void SetLayerChilds(Transform tr)
    {
        tr.gameObject.layer = LayerMask.NameToLayer("UIModel");
        for (int i = 0; i < tr.childCount; i++)
            SetLayerChilds(tr.GetChild(i));
    }

    void OnDestroy()
    {
        if (renderTexture != null) renderTexture.Release();
        if (renderCamera != null) Destroy(renderCamera.gameObject);
        if (spawnedModel != null) Destroy(spawnedModel);
    }
}
