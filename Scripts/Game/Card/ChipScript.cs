
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class ChipScript : MonoBehaviour, IPointerDownHandler
{
    [Header("Main Links")]

    [SerializeField] Image imageChip;
    [SerializeField] Image imageFrame;
    [SerializeField] Image imageChest;

    private CardController cardController;
    private BoosterController boosterController;
    private bool canBeClicked;
    private bool chipIsOpen;
    private bool isBingoChip;
    private bool isChestChip;
    private Vector3 chipStartScale;
    private int chipNum;

    [Header("Sprites")]

    [SerializeField] Sprite standartChip;
    [SerializeField] Sprite bingoChip;
    [SerializeField] float animationTime = 0.3f;

    public void Init(CardController card, BoosterController booster, int _chipNum)
    {
        cardController = card;
        boosterController = booster;
        chipNum = _chipNum;

        chipStartScale = imageChip.transform.localScale;
        canBeClicked = false;
        chipIsOpen = false;
        isBingoChip = false;
        isChestChip = false;
    }

    #region Chip Part

    public void OpenChip()
    {
        imageFrame.gameObject.SetActive(false);
        imageChest.gameObject.SetActive(false);
        imageChip.gameObject.SetActive(true);
        imageChip.sprite = standartChip;
        chipIsOpen = true;
        StartCoroutine(AnimateChipScale(0.1f));

        if (isChestChip)
            cardController.GetChestBonus(transform.position);

        SetClick(false);
        StartCoroutine(cardController.OpenChip(chipNum));
    }

    public void OpenOnStart()
    {
        imageFrame.gameObject.SetActive(false);
        imageChip.gameObject.SetActive(true);
        imageChip.sprite = standartChip;
        chipIsOpen = true;
        SetClick(false);
    }

    public void SetBingoChip()
    {
        imageChip.sprite = bingoChip;
        isBingoChip = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canBeClicked)
        {
            OpenChip();
            boosterController.UpdateProgress();
            cardController.ConfirmOpenedChip(chipNum);
        }
    }

    public IEnumerator AnimateChipScale(float delay)
    {
        Vector3 newScale = new Vector3(0.5f, 0.5f);
        imageChip.transform.DOPunchScale(newScale, animationTime, 0).SetDelay(delay);

        yield return new WaitForSeconds(delay + animationTime);

        imageChip.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetClick(bool canClick) { canBeClicked = canClick; }

    public void ResetChip()
    {
        canBeClicked = false;
        chipIsOpen = false;
        isBingoChip = false;
        isChestChip = false;
        imageChip.gameObject.SetActive(false);
        imageFrame.gameObject.SetActive(false);
        imageChest.gameObject.SetActive(false);
        imageChip.sprite = standartChip;
    }

    public bool CheckIsOpen() { return chipIsOpen; }

    public bool CheckIsBingo() { return isBingoChip; }

    #endregion

    #region Frame Part

    public void OpenFrame(bool isActive)
    {
        if(canBeClicked)
            imageFrame.gameObject.SetActive(isActive);
    }

    #endregion

    #region Chest Part

    public void SetChestChip()
    {
        isChestChip = true;
        imageChest.gameObject.SetActive(true);
    }

    public void SetChestSprite(Sprite usedChest) { imageChest.sprite = usedChest; }

    public bool CheckIsChest() { return isChestChip; }

    #endregion
}
