using System.Collections;
using Assets.Scripts.Instruments;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public RenderTexture RenderTexture
    {
        get => (RenderTexture)rawImage.texture;
        set => rawImage.texture = value;
    }
    public Item curItem;
    [SerializeField] RawImage rawImage;
    [SerializeField] HintSeeker seeker;
    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            gameObject.SetActive(_count > 0);
            text.text = CountValidator.ToShortText(_count);
        }
    }
    [SerializeField] TMPro.TMP_Text text;
    public int Energy
    {
        get
        {
            if (curItem.editedParams.TryGetValue("fill_energy", out string str))
                return (int)float.Parse(str);
            else
                return (int)curItem.unit.config.fillEnergy;
        }
        set
        {
            curItem.editedParams["fill_energy"] = value.ToString();
            energyObj.SetActive(value > 0);
            energyText.text = CountValidator.ToShortText(value);
        }
    }
    [SerializeField] TMPro.TMP_Text energyText;
    [SerializeField] GameObject energyObj;

    private Vector3 startPos;
    private Vector3 startMousePos;
    private Coroutine drag;
    [SerializeField] float maxDelta = 0.125f;

    public void BeginDrag()
    {
        startPos = transform.localPosition;
        startMousePos = Input.mousePosition;

        if (!drag.IsUnityNull()) StopCoroutine(drag);
        drag = StartCoroutine(Drag());
    }
    private IEnumerator Drag()
    {
        while (true)
        {
            transform.localPosition = startPos + (Input.mousePosition - startMousePos) / (1 + Vector3.Distance(startPos, transform.localPosition));
            if (Vector3.Distance(startPos, transform.localPosition) > maxDelta)
            {
                ItemsSpawner.FreeModel(curItem, transform.position);
                EndDrag();
            }
            yield return null;
        }
    }
    public void EndDrag()
    {
        if (!drag.IsUnityNull()) StopCoroutine(drag);
        transform.localPosition = startPos;
    }

    public void RealEndDrag()
    {
        ItemsSpawner.RemoveModel(curItem);
    }

    public void Init(Item item, RenderTexture texture)
    {
        startPos = transform.localPosition;

        curItem = item;
        seeker.text = item.unit.unitName;
        RenderTexture = texture;

        Energy = Energy;

        gameObject.SetActive(false);
    }
}
