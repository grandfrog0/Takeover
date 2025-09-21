using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    private static WindowManager inst;
    public static void OnWindowOpened(Window window)
    {
        inst.openedWindows.Add(window);
        inst.backgroundPanel.gameObject.SetActive(true);

        inst.backgroundPanel.transform.SetAsLastSibling();
        window.gameObject.transform.SetAsLastSibling();

    }
    public static void OnWindowClosed(Window window)
    {
        inst.openedWindows.Remove(window);

        inst.backgroundPanel.transform.SetAsLastSibling();
        if (inst.openedWindows.Count != 0) 
            inst.openedWindows[^1].transform.SetAsLastSibling();

        if (inst.openedWindows.Count == 0)
            inst.backgroundPanel.gameObject.SetActive(false);
    }

    public static Window CreateOpenGet(GameObject obj)
    {
        Window window = Instantiate(obj, inst.windowsParent).GetComponent<Window>();
        window.Open();
        return window;
    }
    public static void CreateOpen(GameObject obj)
    {
        Instantiate(obj, inst.windowsParent).GetComponent<Window>().Open();
    }

    [SerializeField] Transform windowsParent;
    [SerializeField] GameObject backgroundPanel;
    [SerializeField] List<Window> openedWindows;
    private void Awake() => inst = this;
}
