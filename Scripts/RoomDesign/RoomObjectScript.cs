
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomObjectScript : MonoBehaviour
{
    [Header("Links")]

    [SerializeField] int tier;
    [SerializeField] int price;
    [SerializeField] float setImageAnimationSpeed = 0.3f;
    [SerializeField] bool isAD = false;

    public bool _isAD { get; private set; }
    public int id { get; private set; }
    public int _tier { get; private set; }
    public bool isOpen { get; private set; }

    private bool isActive;
    private PolygonCollider2D collider;
    private RoomPrefabScript roomPrefab;

    [Header("Main Image")]

    [SerializeField] Image mainImage;
    [SerializeField] Sprite[] spritesArray;
    [SerializeField] Sprite[] shortSpritesArray;

    [Header("Buy Button")]

    [SerializeField] BuyObjectButton buyButton;
    [SerializeField] Sprite buttonSprite;

    public void Init(RoomPrefabScript room, int _id)
    {
        roomPrefab = room;
        _isAD = isAD;
        id = _id;
        _tier = tier;
        RectTransform rec = GetComponent<RectTransform>();
        rec.localPosition = new Vector3(rec.localPosition.x, rec.localPosition.y, -tier);

        collider = GetComponent<PolygonCollider2D>();
        collider.points = mainImage.GetComponent<PolygonCollider2D>().points;
        collider.enabled = false;

        SetIsOpen(false);
        SetActive(false);
    }

    public void SetPrice(int _price)
    {
        price = _price;
        buyButton.Init(this, buttonSprite, price, isAD);
    }

    public void OnMouseDown()
    {
        if (isActive)
        {
            if (isOpen)
                roomPrefab.CallObjectMenu(this);
        }
    }

    public void BuyObject()
    {
        if (!isAD)
        {
            roomPrefab.PurchaseObject(id, price);
            mainImage.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f, 0);

            AppMetrica.reportEvent("Room_item_bought", "{\"item_id\":\"" + id + "\", \"item_tier\":\"" + tier + "\", \"item_name\":\"" + gameObject.name + "\"}");
        }
        else
        {
            MaxSdkManager.Instance.ShowRewarded("ADPet");
        }
    }
    
    public Vector3 GetBuyButtonPosition() { return buyButton.GetPosition(); }
    public Sprite[] GetShortSprites() { return shortSpritesArray; }
    public Sprite GetButtonSprite() { return buttonSprite; }
    public Vector3 GetBuyObjectPosition() { return buyButton.transform.position; }

    #region Actions

    public void SetImage(int num)
    {
        mainImage.transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0f), setImageAnimationSpeed, 0);
        mainImage.sprite = spritesArray[num];
    }

    public void SwitchButtonCanBeClicked(bool canBeClicked) { buyButton.SwitchCanBeClicked(canBeClicked); }

    public void SwitchBuyButtonActive(bool isActive) { buyButton.SetActive(isActive); }

    public void SetObjectStatus(Status status)
    {
        switch (status)
        {
            case Status.isClosed:
                SetActive(false);
                break;
        
            case Status.isOpen:
                SetActive(true);
                SetIsOpen(false);
                break;
        
            case Status.isPurchased:
                SetActive(true);
                SetIsOpen(true);
                break;
        }
    }

    public enum Status
    {
        isClosed,
        isOpen,
        isPurchased
    }

    private void SetIsOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        if (isOpen)
        {
            mainImage.enabled = true;
            buyButton.SetActive(false);
            collider.enabled = true;
        }
        else
        {
            mainImage.enabled = false;
            buyButton.SetActive(true);
            SwitchButtonCanBeClicked(true);
            collider.enabled = false;
        }
    }

    private void SetActive(bool _isActive)
    {
        isActive = _isActive;
        gameObject.SetActive(_isActive);
    }
    #endregion
}
