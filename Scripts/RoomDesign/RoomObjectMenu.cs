
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class RoomObjectMenu : UIMenuGeneral
{
    private DesignManager designManager;

    [SerializeField] float topPos = -420f;
    [SerializeField] float lowerPos = -1000f;

    [SerializeField] float movingTime;

    private bool isOpen;

    [Header("Variants Back")]

    [SerializeField] Sprite variantActive;
    [SerializeField] Sprite variantNotActive;

    [SerializeField] Image[] variantsBackArray;

    [Header("Variants Front")]

    [SerializeField] Image[] variantsArray;

    private Sprite[] currentSpritesArray;
    private string keyCode;

    public void Init(DesignManager manager) { designManager = manager; }

    public void SetSprites(Sprite[] array)
    {
        currentSpritesArray = array;

        for (int i = 0; i < currentSpritesArray.Length; i++)
        {
            variantsArray[i].sprite = currentSpritesArray[i];
        }
    }

    public void SetKey(string key) { keyCode = key; }

    public string GetKey() { return keyCode; }

    public void ChooseSprite(int num)
    {
        designManager.UpdateObjectSprite(keyCode, num, currentSpritesArray[num]);
        SetActiveBack(num);
        //designManager.HideTutorialFloorScreen();
        GameController.tutorialManager.HideFloorColorScreen();
    }

    public void ConfirmSprite() { designManager.ConfirmSprite(); }

    private void SetActiveBack(int num)
    {
        foreach (Image img in variantsBackArray)
        {
            img.transform.DOScale(1f, 0.3f);
        }

        ResetVariantBack();
        variantsBackArray[num].sprite = variantActive;

        variantsBackArray[num].transform.DOScale(1.3f, 0.3f);
    }

    private void ResetVariantBack()
    {
        for (int i = 0; i < variantsBackArray.Length; i++)
        {
            variantsBackArray[i].sprite = variantNotActive;
        }
    }
    #region Main Window

    public bool GetStatus() { return isOpen; }

    public override void OpenMain()
    {
        foreach (Image img in variantsBackArray)
        {
            img.transform.DOScale(1f, 0f);
        }

        transform.DOLocalMoveY(topPos, movingTime);
        ResetVariantBack();
        isOpen = true;
    }

    public IEnumerator CloseWindow()
    {
        transform.DOLocalMoveY(lowerPos, movingTime);

        yield return new WaitForSeconds(movingTime);

        CloseMain();
    }

    public override void CloseMain()
    {
        isOpen = false;
        keyCode = " ";
    }
    #endregion
}
