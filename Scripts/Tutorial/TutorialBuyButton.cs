
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class TutorialBuyButton : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] Image mainImage;
    [SerializeField] GameObject buyButton;
    [SerializeField] TutorialArrow arrow;
    [SerializeField] Image arrowImage;
    [SerializeField] TMP_FontAsset priceFont;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] float priceTextSize;

    private bool isOpen;

    public void Init()
    {
        priceText.font = priceFont;
        priceText.fontSize = priceTextSize;
        buyButton.transform.localScale = new Vector3(0, 1, 0);
        isOpen = false;
    }

    public void SwitchButton()
    {
        if (!isOpen)
        {
            buyButton.transform.DOScaleX(1, 0.2f);
            isOpen = true;
            StartCoroutine(arrow.MoveArrow(1));
        }
        else
        {
            buyButton.transform.DOScaleX(0, 0.2f);
            isOpen = false;
            StartCoroutine(arrow.MoveArrow(-1));
        }
    }

    public void SetActive(bool isActive) => mainObject.SetActive(isActive);
    public Vector3 GetPosition() { return mainImage.transform.position; }
    public void SetPosition(Vector3 newPos) => gameObject.transform.position = newPos;
    public void SwitchActive(bool isActive) => arrow.SetActive(isActive);
    public void SetMainSprite(Sprite newSprite) => mainImage.sprite = newSprite;
}
