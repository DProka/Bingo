using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIEditPlayersName : UIMenuGeneral
{
    [SerializeField] TMP_InputField textNewName;

    [Header("Confirm Button")]

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonOn;
    [SerializeField] Sprite buttonOff;
    private bool buttonActive;

    public void LoadPlayersName(string name)
    {
        textNewName.text = name;
    }

    public void CheckInput()
    {

    }

    public string GetPlayersName()
    {
        return textNewName.text;
    }

    public override void OpenMain()
    {
        base.OpenMain();
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }
}
