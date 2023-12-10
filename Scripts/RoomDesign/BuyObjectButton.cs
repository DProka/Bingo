
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BuyObjectButton : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] GameObject mainButton;
    [SerializeField] GameObject buyButton;
    [SerializeField] TextMeshProUGUI priceText;

    private bool isOpen;
    private RoomObjectScript roomObject;

    public void Init(RoomObjectScript script)
    {
        roomObject = script;
        buyButton.transform.localScale = new Vector3(0, 1, 0);
        isOpen = false;
    }

    public void SwitchButton()
    {
        if (!isOpen)
        {
            buyButton.transform.DOScaleX(1, 0.2f);
            isOpen = true;
        }
        else
        {
            buyButton.transform.DOScaleX(0, 0.2f);
            isOpen = false;
        }
    }

    public void BuyObject() 
    {
        if (isOpen)
        {
            roomObject.BuyObject();
            SwitchButton();
        }
    }

    public void SetActive(bool isActive)
    {
        mainObject.SetActive(isActive);
    }

    public void SetPrice(int price) { priceText.text = $"{price}"; }

    public Vector3 GetPosition() { return mainButton.transform.position; }

}
