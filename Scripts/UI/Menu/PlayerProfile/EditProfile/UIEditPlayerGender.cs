
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIEditPlayerGender : UIMenuGeneral
{
    private int genderNumber;

    [Header("Switches")]

    [SerializeField] UIGenderSwitch[] switchesArray;

    [Header("Confirm Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;
    private bool buttonActive;

    public void Init()
    {
        SwitchOffAll();

        for (int i = 0; i < switchesArray.Length; i++)
        {
            switchesArray[i].Init();
        }

        if (genderNumber != 0)
            switchesArray[genderNumber].SwitchON();
    }

    public void SetGenderNumber(int number)
    {
        SwitchOffAll();

        if (number > 0)
        {
            genderNumber = number;
            switchesArray[genderNumber - 1].SwitchON();
        }
            
        SetButtonActive(true);
    }

    public int GetGenderNumber() { return genderNumber; }

    public string GetPlayerGender(int number)
    {
        if (number > 0)
            return switchesArray[number - 1].GetName();
        else
            return "Choose Gender";
    }

    public void SwitchOffAll()
    {
        for (int i = 0; i < switchesArray.Length; i++)
        {
            switchesArray[i].SwitchOff();
        }
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
