
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIEditPlayerCountry : UIMenuGeneral
{
    [Header("Main Links")]

    [SerializeField] UIPlayerProfile uiProfile;

    [Header("Menu Pictures")]

    [SerializeField] Image infoMenuAva;
    [SerializeField] Image profileMenuAva;

    [Header("Confirm Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;
    private bool buttonActive;

    [Header("Flags")]

    [SerializeField] GameObject content;
    [SerializeField] UIFlagScript flagPrefab;
    [SerializeField] ProfileData profileData;
    [SerializeField] UIFlagScript[] flagsArray;

    public int countryNumber;

    public void Init(int countryNumber)
    {
        GenerateFlagArray();

        LoadCountry(countryNumber);
    }

    public void GenerateFlagArray()
    {
        flagsArray = new UIFlagScript[profileData.flagArray.Length];

        for (int i = 0; i < profileData.flagArray.Length; i++)
        {
            UIFlagScript pref = Instantiate(flagPrefab, content.transform);
            pref.SetManager(this);
            pref.SetFlag(profileData.flagArray[i]);
            pref.SetNumber(i);
            pref.isActive = true;
            pref.SetActiveFrame(false);
            flagsArray[i] = pref;
        }
    }

    public void LoadCountry(int number)
    {
        countryNumber = number;
        UpdateAllPictures(number);
        flagsArray[number].SetActiveFrame(true);
    }

    public void ChangeCountry(int number)
    {
        countryNumber = number;
        SetAllFlagsNotActive();
        flagsArray[number].SetActiveFrame(true);
        SetButtonActive(true);
    }

    public void SetButtonActive(bool active)
    {
        buttonActive = active;

        if (active)
            buttonImage.sprite = buttonOn;
        else
            buttonImage.sprite = buttonOff;
    }

    public bool GetAvtive() { return buttonActive; }

    public void SetAllFlagsNotActive()
    {
        for (int i = 0; i < flagsArray.Length; i++)
        {
            flagsArray[i].SetActiveFrame(false);
        }
    }

    public int GetCountryNumber() { return countryNumber; }

    public string GetCountryName() { return profileData.flagArray[countryNumber].name; }

    public void UpdateAllPictures(int number)
    {
        infoMenuAva.sprite = profileData.flagArray[number];
        profileMenuAva.sprite = profileData.flagArray[number];
    }

    public Sprite GetFlag(int num) { return profileData.flagArray[num]; }
    public Sprite GetRandomFlag() { return profileData.flagArray[Random.Range(0, profileData.flagArray.Length)]; }

    public override void OpenMain()
    {
        base.OpenMain();
        SetButtonActive(false);
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }
}
