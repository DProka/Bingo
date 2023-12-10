
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEditAvatar : UIMenuGeneral
{
    [Header("Main Links")]

    [SerializeField] GameObject content;
    [SerializeField] UIAvatarScript avatarPrefab;
    [SerializeField] ProfileData avaBase;
    
    public int avaNumber;
    private UIAvatarScript[] avatarsArray;

    [Header("Confirm Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;
    private bool buttonActive;

    [Header("Avatar Images")]

    [SerializeField] Image mainMenuAva; 
    [SerializeField] Image infoMenuAva; 
    [SerializeField] Image profileMenuAva; 

    public void Init(int avanumber)
    {
        GenerateAvatarsGrid();

        LoadAvatar(avanumber);
    }

    public void GenerateAvatarsGrid()
    {
        avatarsArray = new UIAvatarScript[avaBase.avatarArray.Length];

        for (int i = 0; i < avaBase.avatarArray.Length; i++)
        {
            UIAvatarScript pref = Instantiate(avatarPrefab, content.transform);
            pref.SetManager(this);
            pref.SetPhoto(avaBase.avatarArray[i]);
            pref.SetNumber(i);
            pref.isActive = true;
            pref.SetActiveFrame(false);
            avatarsArray[i] = pref;
        }
    }

    public void LoadAvatar(int number) 
    {
        avaNumber = number;
        UpdateAllPictures(number); 
    }

    public void ChangeAvatar(int number)
    {
        avaNumber = number;
        SetButtonActive(true);
        SetAllFramesNotActive();
        avatarsArray[number].SetActiveFrame(true);
    }

    public void SetAllFramesNotActive()
    {
        for (int i = 0; i < avatarsArray.Length; i++)
        {
            avatarsArray[i].SetActiveFrame(false);
        }
    }

    public void SetAvatarActive()
    {
        UpdateAllPictures(avaNumber);
    }

    public int GetAvatarNumber() { return avaNumber; }

    public void UpdateAllPictures(int number)
    {
        mainMenuAva.sprite = avaBase.avatarArray[number];
        infoMenuAva.sprite = avaBase.avatarArray[number];
        profileMenuAva.sprite = avaBase.avatarArray[number];
    }

    public void SetButtonActive(bool isOn)
    {
        buttonActive = isOn;

        if (isOn)
            buttonImage.sprite = buttonOn;
        else
            buttonImage.sprite = buttonOff;
    }

    public bool CheckButtonActive() { return buttonActive; }

    public Sprite GetAvatar(int number) { return avaBase.avatarArray[number]; }
    public Sprite GetRandomAvatar() { return avaBase.avatarArray[Random.Range(0, avaBase.avatarArray.Length)]; }

    #region Main Menu

    public override void OpenMain()
    {
        base.OpenMain();

        SetButtonActive(false);
        avatarsArray[avaNumber].SetActiveFrame(true);
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }
    #endregion
}
