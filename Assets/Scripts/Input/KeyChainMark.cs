using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyChainMark : MonoBehaviour
{
    [SerializeField] RectTransform frame;
    [SerializeField] TMP_Text keyCodeText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] Image frameImage;
    [SerializeField] Sprite def, pressed;

    [SerializeField] float addWidth;

    private float startHeight;

    public KeyCode KeyCode { get; private set; }
    public string Description { set => descriptionText.text = value; }

    public void Init(KeyCode keyCode, string pseudonym, string description)
    {
        bool isPressed = Inputs.IsPressed(keyCode);
        frameImage.sprite = isPressed ? pressed : def;
        KeyCode = keyCode;

        keyCodeText.text = pseudonym;
        Description = description;

        startHeight = frame.sizeDelta.y;

        frame.sizeDelta = new Vector2(keyCodeText.preferredWidth + addWidth, isPressed ? startHeight / 1.25f : startHeight);
    }

    public void OnPointerDown()
    {
        frameImage.sprite = pressed;
        frame.sizeDelta = new Vector2(frame.sizeDelta.x, startHeight / 1.25f);
    }

    public void OnPointerUp()
    {
        if (!Inputs.IsPressed(KeyCode))
        {
            frameImage.sprite = def;
            frame.sizeDelta = new Vector2(frame.sizeDelta.x, startHeight);
        }
    }

    public void OnClick() => KeyChains.Chains[KeyCode].OnDown();
}
