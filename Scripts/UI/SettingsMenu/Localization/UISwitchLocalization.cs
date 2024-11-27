
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISwitchLocalization : UIMenuGeneral
{
    [Header("Main")]

    [SerializeField] LocalizationManager localizationManager;

    public string key;
    private int num;

    [SerializeField] Transform variants;
    private UILangSwitch[] langArray;

    [Header("Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;

    private bool buttonActive;

    public void Init()
    {
        localizationManager.Init();

        langArray = new UILangSwitch[variants.childCount];
        for (int i = 0; i < langArray.Length; i++)
        {
            langArray[i] = variants.GetChild(i).GetComponent<UILangSwitch>();
            langArray[i].Init();
        }

        SetButtonActive(false);
    }

    public override void OpenMain()
    {
        base.OpenMain();

        SwitchOffAll();
        langArray[num].SwitchON();
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }

    public int GetNumber() { return num; }

    public void SwitchLanguage(string _key, int _num)
    {
        num = _num;
        key = _key;

        SwitchOffAll();
        langArray[num].SwitchON();
        SetButtonActive(true);

        localizationManager.SetLanguage(num);
    }

    public void LoadLocalization(int _num) 
    {
        localizationManager.SetLanguage(_num);
        num = _num;
    }

    public void SwitchOffAll()
    {
        for (int i = 0; i < langArray.Length; i++)
        {
            langArray[i].SwitchOff();
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

    public void ConfirmLanguage()
    {
        if (buttonActive)
        {
            CloseMain();
        }
    }
}
