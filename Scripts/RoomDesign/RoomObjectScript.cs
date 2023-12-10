
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomObjectScript : MonoBehaviour
{
    [Header("Links")]

    [SerializeField] GameObject mainObject;
    [SerializeField] GameObject imageObject;
    [SerializeField] BuyObjectButton buyButton;
    [SerializeField] Image mainImage;

    private PolygonCollider2D collider;
    private DesignManager designManager;

    [Header("Settings")]

    private int price;
    [SerializeField] int tier;
    [SerializeField] string key;
    [SerializeField] float setImageAnimationSpeed = 0.3f;


    private bool isOpen;
    private bool isActive;

    public void Init(DesignManager manager) 
    {
        designManager = manager;
        collider = GetComponent<PolygonCollider2D>();
        collider.points = imageObject.GetComponent<PolygonCollider2D>().points;
        buyButton.Init(this);

        SetIsOpen(false);
        SetActive(false);
    }

    public void SetImage(Sprite newSprite) 
    {
        mainImage.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), setImageAnimationSpeed, 0);    
        mainImage.sprite = newSprite; 
    }

    public void OnMouseDown()
    {
        if (isActive)
        {
            if (isOpen)
                designManager.CallObjectMenu(this);
        }
    }

    public void SetIsOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        if (isOpen)
        {
            imageObject.SetActive(true);
            buyButton.SetActive(false);
        }
        else
        {
            imageObject.SetActive(false);
            buyButton.SetActive(true);
        }
    }

    public void BuyObject()
    {
        designManager.BuyObject(this, price);
        mainImage.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f, 0);
    }

    public void SetActive(bool _isActive)
    {
        isActive = _isActive;

        if (_isActive)
            mainObject.SetActive(true);
        else
            mainObject.SetActive(false);
    }

    public void SetPrice(int _price) 
    { 
        price = _price;
        buyButton.SetPrice(price);
    }

    public string GetKey() { return key; }

    public int GetTier() { return tier; }

    public bool GetStatus() { return isOpen; }

    public void SwitchBuyButtonAvtive(bool isActive) { buyButton.SetActive(isActive); }

    public Vector3 GetBuyButtonPosition() { return buyButton.GetPosition(); }
}
