using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    private static Hint inst;
    public static void SelectHint(string text)
    {
        if (!inst.fadeCoroutine.IsUnityNull()) inst.StopCoroutine(inst.fadeCoroutine);
        if (!inst.moveCoroutine.IsUnityNull()) inst.StopCoroutine(inst.moveCoroutine);

        if (SettingsManager.CurrentSettings.showHints)
        {
            inst.fadeCoroutine = inst.StartCoroutine(inst.Unfade());

            inst.text.text = text;
            int rowsCount = (int)(inst.text.preferredWidth / 750) + 1;
            inst.rect.sizeDelta = new Vector2(20 + inst.text.preferredWidth / rowsCount, 25 + 50 * rowsCount);

            inst.moveCoroutine = inst.StartCoroutine(inst.MoveAtMousePos());
        }
    }
    public static void DeselectHint()
    {
        if (!inst.fadeCoroutine.IsUnityNull()) inst.StopCoroutine(inst.fadeCoroutine);
        inst.fadeCoroutine = inst.StartCoroutine(inst.Fade());
    }

    [SerializeField] RectTransform rect;
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] Image im;
    private Coroutine moveCoroutine, fadeCoroutine;
    private Color startImColor, startTextColor;

    private IEnumerator MoveAtMousePos()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Screen.width, Screen.height) * -0.5f + Input.mousePosition;
            pos = new(Mathf.Clamp(pos.x, -Screen.width / 2, Screen.width / 2 - rect.sizeDelta.x), Mathf.Clamp(pos.y, -Screen.height / 2, Screen.height / 2 - rect.sizeDelta.y));
            rect.anchoredPosition = pos;
            yield return null;
        }
    }
    private IEnumerator Fade()
    {
        for (float t = 0; t < 1; t += Time.fixedDeltaTime)
        {
            im.color = Color.Lerp(im.color, Color.clear, t);
            text.color = Color.Lerp(text.color, Color.clear, t);
            yield return null;
        }

        im.color = Color.clear;
        text.color = Color.clear;
    }
    private IEnumerator Unfade()
    {
        for (float t = 0; t < 1; t += Time.fixedDeltaTime)
        {
            im.color = Color.Lerp(im.color, startImColor, t);
            text.color = Color.Lerp(text.color, startTextColor, t);
            yield return null;
        }

        im.color = startImColor;
        text.color = startTextColor;
    }
    private void Awake()
    {
        inst = this;

        startImColor = im.color;
        startTextColor = text.color;

        im.color = Color.clear;
        text.color = Color.clear;
    }
}
