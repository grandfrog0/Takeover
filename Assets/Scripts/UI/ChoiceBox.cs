using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoiceBox : MonoBehaviour, IPointerClickHandler
{
    public readonly UnityEvent<int> onValueChanged = new();
    [SerializeField] int _value = 0;
    public int Value
    {
        get => _value;
        set
        {
            if (variants.Count == 0) return;

            oldText.text = text.text;

            _value = Mathf.Clamp(value, 0, variants.Count - 1);
            onValueChanged.Invoke(Value);

            text.text = variants[Value].name;
        }
    }
    [SerializeField] TMP_Text text, oldText;
    [SerializeField] List<ChoiceVariant> variants = new();
    [SerializeField] Vector3 startPos, needPos;
    private Coroutine animCoroutine;

    public void Next()
    {
        if (variants.Count == 0) return;

        Value = ++_value % variants.Count;

        if (!animCoroutine.IsUnityNull())
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(NextCoroutine());
    }
    public void Previous()
    {
        if (variants.Count == 0) return;

        _value = --_value % variants.Count;
        if (_value < 0) _value += variants.Count;
        Value = _value;

        if (!animCoroutine.IsUnityNull())
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(PreviousCoroutine());
    }
    public void AddValues(List<string> values) => this.variants.AddRange(values.Select(x => new ChoiceVariant(x.ToString(), x)));
    public void AddValues(List<ChoiceVariant> values) => this.variants.AddRange(values);
    public ChoiceVariant GetVariant(int value) => variants[value];
    public void ClearValues() => variants.Clear();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Next();
        else if (eventData.button == PointerEventData.InputButton.Right)
            Previous();
    }

    private IEnumerator NextCoroutine()
    {
        Vector3 from = new(-needPos.x, needPos.y);
        text.transform.localPosition = needPos;
        oldText.transform.localPosition = startPos;

        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 5)
        {
            text.transform.localPosition = Vector3.Lerp(needPos, startPos, t);
            oldText.transform.localPosition = Vector3.Lerp(startPos, from, t);
            yield return null;
        }

        text.transform.localPosition = startPos;
        oldText.transform.localPosition = from;
    }
    private IEnumerator PreviousCoroutine()
    {
        Vector3 from = new(-needPos.x, needPos.y);
        text.transform.localPosition = from;
        oldText.transform.localPosition = startPos;

        for (float t = 0; t <= 1; t += Time.unscaledDeltaTime * 5)
        {
            text.transform.localPosition = Vector3.Lerp(from, startPos, t);
            oldText.transform.localPosition = Vector3.Lerp(startPos, needPos, t);
            yield return null;
        }

        text.transform.localPosition = startPos;
        oldText.transform.localPosition = needPos;
    }
}
