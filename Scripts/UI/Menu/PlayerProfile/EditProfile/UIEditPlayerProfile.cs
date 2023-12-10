
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIEditPlayerProfile : UIMenuGeneral
{
    [SerializeField] PlayerProfile playerProfile;

    private bool isActive;
    private UIMenuGeneral openedMenu;

    [Header("Player Avatar")]

    [SerializeField] UIEditAvatar editAvatar;

    [Header("Player Name")]

    [SerializeField] UIEditPlayersName editName;
    [SerializeField] TextMeshProUGUI buttonNameText;

    [Header("Player Country")]

    [SerializeField] UIEditPlayerCountry editCountry;
    [SerializeField] TextMeshProUGUI buttonCountryText;

    [Header("Player E-Mail")]

    [SerializeField] UIEditPlayersEMail editEmail;
    [SerializeField] TextMeshProUGUI buttonEMailText;

    [Header("Player Gender")]

    [SerializeField] UIEditPlayerGender editGender;
    [SerializeField] TextMeshProUGUI buttonGenderText;

    [Header("Player Birthday")]

    [SerializeField] UIEditPlayerBirthday editBirthday;
    [SerializeField] TextMeshProUGUI buttonBirthdayText;

    public void Init()
    {
        isActive = true;
        CloseMain();

        editAvatar.Init(playerProfile.avatarNumber);

        editCountry.Init(playerProfile.playerCountry);

        editGender.Init();

        editBirthday.Init(playerProfile.playerBirthDate);

        UpdateWindow();
    }

    public void UpdateWindow()
    {
        editAvatar.UpdateAllPictures(playerProfile.avatarNumber);

        editName.LoadPlayersName(playerProfile.playerNickName);
        buttonNameText.text = playerProfile.playerNickName;

        editCountry.UpdateAllPictures(playerProfile.playerCountry);
        buttonCountryText.text = editCountry.GetCountryName();

        editEmail.LoadPlayersEMail(playerProfile.playerEMail);
        buttonEMailText.text = playerProfile.playerEMail;

        editCountry.LoadCountry(playerProfile.playerCountry);
        buttonCountryText.text = editCountry.GetCountryName();

        buttonGenderText.text = editGender.GetPlayerGender(playerProfile.playerGender);
        editGender.SetGenderNumber(playerProfile.playerGender);

        if (editBirthday.GetDate()[0] != 0 && editBirthday.GetDate()[1] != 0)
            buttonBirthdayText.text = $"{editBirthday.GetDate()[0]} . {editBirthday.GetDate()[1]}";
        else
            buttonBirthdayText.text = "Not Selected";
    }

    public void CloseAllWindows()
    {
        editAvatar.CloseMain();
        editName.CloseMain();
        editCountry.CloseMain();
        editEmail.CloseMain();
        editGender.CloseMain();
        editBirthday.CloseMain();
    }

    #region Main Window

    public override void OpenMain() 
    {
        base.OpenMain();

        CloseAllWindows();
        isActive = true;
    }

    public override void CloseMain()
    {
        if (isActive)
        {
            base.CloseMain();
            isActive = false;
        } 
    }
    #endregion

    #region Other Windows

    public void OpenEdit(UIMenuGeneral menu)
    {
        if (isActive)
        {
            SwitchShade(false);
            menu.OpenMain();
            isActive = false;
            openedMenu = menu;
        }
    }

    public void CloseEdit()
    {
        if(openedMenu != null)
        {
            SwitchShade(true);
            openedMenu.CloseMain();
            isActive = true;
            openedMenu = null;
        }
    }
    #endregion

    #region Edit Avatar

    public void SetPlayerAvatar()
    {
        if (editAvatar.CheckButtonActive())
        {
            playerProfile.SetPlayersAvatar(editAvatar.GetAvatarNumber());
            editAvatar.UpdateAllPictures(playerProfile.avatarNumber);
            CloseEdit();
            UpdateWindow();
        }
    }
    #endregion

    #region Edit Name

    public void SetNewPlayerName() 
    { 
        playerProfile.SetPlayersName(editName.GetPlayersName());
        CloseEdit();
        UpdateWindow();
    }
    #endregion

    #region Edit Country

    public void SetPlayerCountry()
    {
        if (editCountry.GetAvtive())
        {
            playerProfile.SetPlayersCountry(editCountry.GetCountryNumber());
            editCountry.UpdateAllPictures(playerProfile.playerCountry);
            CloseEdit();
            UpdateWindow();
        }
    }
    #endregion

    #region Edit E-Mail

    public void SetNewPlayerEMail() 
    { 
        playerProfile.SetPlayersEMail(editEmail.GetPlayersEMail());
        CloseEdit();
        UpdateWindow();
    }
    #endregion

    #region Edit Gender

    public void SetNewPlayerGender()
    {
        if (editGender.GetAvtive())
        {
            playerProfile.SetPlayersGender(editGender.GetGenderNumber());
            CloseEdit();
            UpdateWindow();
        }
    }
    #endregion

    #region Edit Birthday

    public void SetPlayerBirthday()
    {
        if (editBirthday.GetAvtive())
        {
            playerProfile.SetPlayersBirthday(editBirthday.GetDate());
            CloseEdit();
            UpdateWindow();
        }
    }
    #endregion
}
