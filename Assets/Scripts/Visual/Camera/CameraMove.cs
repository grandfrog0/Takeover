using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour, IInitializable
{
    public const float defaultSize = 7;

    private Camera main;
    private Vector3 start;
    private bool hasStart;

    [SerializeField] float sizeScrollMultiplier = 500;
    [SerializeField] float minSize = 2.5f;
    [SerializeField] float maxSize = 25f;

    public InitializeOrder Order => InitializeOrder.Camera;

    public void Initialize()
    {
        main = Camera.main;

        CameraInfo info = AppDataLoader.LoadedData?.camera ?? new CameraInfo();
        main.transform.position = new (info.position.x, info.position.y, -10);
        main.orthographicSize = info.size;

        Inputs.AddMouseDownListener(0, () =>
        {
            hasStart = !Inputs.IsPointerOnUI(includeWorldSpace:false);
            start = main.ScreenToWorldPoint(Input.mousePosition);
        });

        Inputs.AddMouseHoldListener(0, () =>
        {
            if (!hasStart) return;

            Vector2 delta = (start - main.ScreenToWorldPoint(Input.mousePosition));
            main.transform.position = new Vector3(main.transform.position.x + delta.x, main.transform.position.y + delta.y, -10);
            start = main.ScreenToWorldPoint(Input.mousePosition);
        });

        Inputs.AddScrollListener(value =>
        {
            if (Inputs.IsPointerOnUI(includeWorldSpace: false) || !Inputs.IsMouseInsideScreen()) return;

            Vector3 mouseWorldPosBefore = main.ScreenToWorldPoint(Input.mousePosition);

            float val = value * sizeScrollMultiplier * Time.deltaTime;
            main.orthographicSize -= val;
            main.orthographicSize = Mathf.Clamp(main.orthographicSize, minSize, maxSize);

            Vector3 mouseWorldPosAfter = main.ScreenToWorldPoint(Input.mousePosition);

            main.transform.position += mouseWorldPosBefore - mouseWorldPosAfter;
        });
    }
}
