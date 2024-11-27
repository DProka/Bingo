
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RoomObjectMenu : MonoBehaviour
{
    public bool isOpen { get; private set; }

    [SerializeField] float topPos = -420f;
    [SerializeField] float lowerPos = -1000f;
    [SerializeField] float movingTime;

    private DesignManager designManager;
    private bool isAD;
    private int choosenSprite;
    private bool isFirstOpen;

    [Header("Variants")]

    [SerializeField] ObjectVariantPrefab[] menuObjectsArray;

    public void Init(DesignManager manager)
    {
        designManager = manager;
        CloseMain();

        EventBus.onRewardedADClosed += UpdateSpriteAfterAD;
    }

    public void SetActiveVariantByNum(int num)
    {
        ResetVariants();

        if (num > 0)
            menuObjectsArray[num - 1].SetActive(true);
        else
            menuObjectsArray[num].SetActive(true);

        if (isFirstOpen)
            SwitchADActive(false);
    }

    private void ResetVariants()
    {
        foreach (ObjectVariantPrefab var in menuObjectsArray)
        {
            var.SetActive(false);
        }
    }

    public void SwitchADActive(bool adIsActive)
    {
        foreach (ObjectVariantPrefab obj in menuObjectsArray)
        {
            obj.SwitchADIsActive(adIsActive);
        }
    }

    #region Sprites

    public void ChooseSprite(int num)
    {
        if (!menuObjectsArray[num - 1].active)
        {
            choosenSprite = num;

            if (isFirstOpen)
                UpdateNewSprite();
            else
                MaxSdkManager.Instance.ShowRewarded("ObjectNewSprite");
        }

        if (GameController.tutorialIsActive)
            GameController.tutorialManager.CloseFloorColorScreen();
    }

    public void ConfirmSprite()
    {
        designManager.ConfirmSprite();

        if (GameController.tutorialIsActive)
            GameController.tutorialManager.CloseFloorColorScreen();
    }

    private void UpdateSpriteAfterAD(string location)
    {
        if (location == "ObjectNewSprite")
            UpdateNewSprite();
    }

    private void UpdateNewSprite()
    {
        if (isOpen)
        {
            designManager.UpdateObjectSprite(choosenSprite);
            SetActiveVariantByNum(choosenSprite);
        }
    }

    private void SetSprites(Sprite[] newSpritesArray)
    {
        for (int i = 0; i < menuObjectsArray.Length; i++)
        {
            menuObjectsArray[i].SetMainSprite(newSpritesArray[i]);
        }
    }

    #endregion

    #region Main Window

    public void OpenMain(bool _isFirstOpen, Sprite[] newSpritesArray, bool _isAD, int activeSpriteNum)
    {
        gameObject.SetActive(true);

        isFirstOpen = _isFirstOpen;
        isAD = _isAD;

        SetSprites(newSpritesArray);

        ResetVariants();
        SwitchADActive(isFirstOpen ? false : true);
        menuObjectsArray[0].gameObject.SetActive(isAD ? false : true);
        SetActiveVariantByNum(activeSpriteNum);

        transform.DOLocalMoveY(topPos, movingTime);
        isOpen = true;

        SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick2);
    }

    public IEnumerator CloseWindow()
    {
        transform.DOLocalMoveY(lowerPos, movingTime);

        yield return new WaitForSeconds(movingTime);

        CloseMain();
    }

    public void CloseMain()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }
    #endregion

    private void OnDestroy()
    {
        EventBus.onRewardedADClosed -= UpdateSpriteAfterAD;
    }
}
