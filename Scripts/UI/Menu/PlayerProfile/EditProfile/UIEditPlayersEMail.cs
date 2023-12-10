
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIEditPlayersEMail : UIMenuGeneral
{
    [SerializeField] TMP_InputField textNewEmail;

    public void LoadPlayersEMail(string email)
    {
        textNewEmail.text = email;
    }

    public string GetPlayersEMail()
    {
        return textNewEmail.text;
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
