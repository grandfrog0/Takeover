using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// спавнит все ячейки для предметов, отвечает за выбор точки для билда.
public class ItemsSpawner : MonoBehaviour, IInitializable
{
    private static ItemsSpawner inst;
    public static void FreeModel(Item item, Vector3 start) => inst.StartSearchingPlace(item, start);
    public static void RemoveModel(Item item)
    {
        if (item == inst.curItem)
            inst.StopSearchingPlace();
    }
    public static void CancelSearch()
    {
        inst.canceled = true;
        inst.StopSearchingPlace();
    }

    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform itemsParent;
    // unitInfo-energy
    private Dict<Item, InventoryItem> items; 

    [SerializeField] RawImage freeModelImage;
    [SerializeField] float moveSpeed = 5;

    [SerializeField] BuildChoicePoint pointPrefab;
    private BuildChoicePoint point;

    private Item curItem;
    private Coroutine coroutine;
    private bool onGui = false;
    private bool canceled = false;

    private Vector3 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition).ZeroZ() - new Vector3(0.5f, 0.5f);

    public InitializeOrder Order => InitializeOrder.ItemsSpawner;
    public void Initialize()
    {
        inst = this;

        items = new();
        foreach((Item item, int count) in InventoryManager.GetCopy().items)
        {
            InventoryItem invItem = Instantiate(itemPrefab, itemsParent).GetComponent<InventoryItem>();
            invItem.Init(item, UnitImageManager.Images[item.unit].renderTexture);
            invItem.Count = count;
            items.Add(item, invItem);
        }
        InventoryManager.OnItemCountChanged.AddListener(UnitCountChanged);
    }

    private void UnitCountChanged(Item item, int value)
    {
        if (!items.ContainsKey(item))
        {
            InventoryItem invItem = Instantiate(itemPrefab, itemsParent).GetComponent<InventoryItem>();
            invItem.Init(item, UnitImageManager.Images[item.unit].renderTexture);
            invItem.Count = InventoryManager.GetCount(item);
            items.Add(item, invItem);
        }
        else
        {
            InventoryItem invItem = items[item];
            invItem.Init(item, UnitImageManager.Images[item.unit].renderTexture);
            invItem.Count = InventoryManager.GetCount(item);
        }
        items[item].Count = value;
    }

    public void StartSearchingPlace(Item item, Vector3 start)
    {
        StopSearchingPlace();

        KeyChains.AddUp(KeyCode.Mouse0, StopSearchingPlace);
        KeyChains.AddDown(KeyCode.Escape, CancelSearch, "Cancel placing");

        freeModelImage.transform.position = start;
        freeModelImage.color = Color.white;
        freeModelImage.texture = UnitImageManager.Images[item.unit].renderTexture;
        curItem = item;

        point = Instantiate(pointPrefab, start, Quaternion.identity);
        point.PlaceType = PlaceType.CanPlaceUnit;

        coroutine = StartCoroutine(SearchPlace());
    }
    public void StopSearchingPlace()
    {
        if (!coroutine.IsUnityNull())
        {
            if (!canceled && !Inputs.IsPointerOnUI(includeWorldSpace: false) && Inputs.IsMouseInsideScreen())
                UnitsManager.StartBuild(curItem.unit, MousePos, curItem.editedParams);

            curItem = null;
            freeModelImage.color = Color.clear;

            StopCoroutine(coroutine);
            coroutine = null;

            KeyChains.RemoveUp(KeyCode.Mouse0, StopSearchingPlace);
            KeyChains.RemoveDown(KeyCode.Escape, CancelSearch);

            if (point != null) 
                Destroy(point.gameObject);

            canceled = false;
        }
    }
    private IEnumerator SearchPlace()
    {
        while (true)
        {
            Vector3 pos = MousePos;

            freeModelImage.transform.position = Vector3.Lerp(
                freeModelImage.transform.position, 
                pos,
                moveSpeed * Time.deltaTime);

            if (!Inputs.IsPointerOnUI(includeWorldSpace: false))
            {
                onGui = false;
                point.SetPoint(pos);
            }
            else if (!onGui)
            {
                onGui = true;
                point.ClearPoint();
            }

            yield return null;
        }
    }
}
