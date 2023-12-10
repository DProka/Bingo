
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIEditPlayerBirthday : UIMenuGeneral
{
    [Header("Main Links")]

    [SerializeField] UIPlayerProfile uiProfile;

    [Header("Confirm Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;
    private bool buttonActive;

    [Header("Numbers")]

    [SerializeField] GameObject dayContent;
    [SerializeField] GameObject monthContent;
    [SerializeField] UINumberScript numberPrefab;
    [SerializeField] UINumberScript[] dayArray;
    [SerializeField] UINumberScript[] monthArray;

    private int[] date;

    public void Init(int[] _date)
    {
        GenerateNumbers();

        date = _date;
    }

    public void GenerateNumbers()
    {
        dayArray = new UINumberScript[31];
        monthArray = new UINumberScript[12];

        for (int i = 0; i < 31; i++)
        {
            UINumberScript pref = Instantiate(numberPrefab, dayContent.transform);

            pref.Init(this, i + 1);
            pref.SetDayBool(true);
            dayArray[i] = pref;
        }

        for (int i = 0; i < 12; i++)
        {
            UINumberScript pref = Instantiate(numberPrefab, monthContent.transform);

            pref.Init(this, i + 1);
            pref.SetDayBool(false);
            monthArray[i] = pref;
        }
    }

    public int[] GetDate() 
    {
        return date; 
    }

    public void SetDay (int _day) 
    { 
        date[0] = _day;

        for (int i = 0; i < dayArray.Length; i++)
        {
            dayArray[i].SetColorActive(false);
        }

        dayArray[_day - 1].SetColorActive(true);
        SetButtonActive(true);
    }

    public void SetMonth (int _month) 
    { 
        date[1] = _month;

        for (int i = 0; i < monthArray.Length; i++)
        {
            monthArray[i].SetColorActive(false);
        }

        monthArray[_month - 1].SetColorActive(true);
        SetButtonActive(true);
    }
    
    public void SetButtonActive(bool active)
    {
        if(active && date[0] != 0 && date[1] != 0)
        {
            buttonActive = active;

            buttonImage.sprite = buttonOn;
        }
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
