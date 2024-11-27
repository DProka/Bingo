
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BuyObjectButton : MonoBehaviour
{
    [Header("Main Links")]

    [SerializeField] GameObject mainObject;
    [SerializeField] GameObject mainButton;
    
    [SerializeField] Image buyButtonImg;
    [SerializeField] GameObject buyButtonObj;
    [SerializeField] Image mainButtonImage;
    
    [Header("Font")]

    [SerializeField] TMP_FontAsset priceFont;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] float fontSize;

    private RoomObjectScript roomObject;
    private bool isOpen;
    private bool isAd = false;
    private bool canBeClicked;

    public void Init(RoomObjectScript script, Sprite buttonSprite, int price, bool _isAd)
    {
        roomObject = script;
        isAd = _isAd;

        if(priceText != null)
        {
            priceText.font = priceFont;
            priceText.fontSize = fontSize;
            priceText.text = $"{price}";
        }

        SwitchOffButton();
        mainButtonImage.sprite = buttonSprite;
    }
    
    public void OnMouseDown()
    {
        if (canBeClicked)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 objectPos = transform.position;

            if (!isOpen)
            {
                SwitchButton();
            }
            else
            {
                if (mousePos.x < objectPos.x)
                {
                    SwitchButton();
                }
                else
                {
                    BuyObject();
                }
            }

            SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
        }
    }

    public void SwitchButton()
    {
        if (!isOpen)
        {
            buyButtonImg.DOFillAmount(1, 0.2f);
            buyButtonObj.transform.DOScaleX(1, 0.2f);
            isOpen = true;
        }
        else
        {
            buyButtonImg.DOFillAmount(0, 0.2f);
            buyButtonObj.transform.DOScaleX(0, 0.2f);
            isOpen = false;
        }
    }

    public void BuyObject() 
    {
        if (isOpen)
        {
            if (isAd)
            {
                roomObject.BuyObject();
            }
            else
            {
                BuyAdObject();
            }

            SwitchButton();
        }
    }

    public void SwitchCanBeClicked(bool _canBeClicked) { canBeClicked = _canBeClicked; }

    public void SetActive(bool isActive) { mainObject.SetActive(isActive); }

    public Vector3 GetPosition() { return mainButton.transform.position; }

    private void BuyAdObject()
    {
        roomObject.BuyObject();
    }

    private void SwitchOffButton()
    {
        buyButtonImg.DOFillAmount(0, 0.2f);
        buyButtonObj.transform.DOScaleX(0, 0.2f);
        isOpen = false;
    }
}
