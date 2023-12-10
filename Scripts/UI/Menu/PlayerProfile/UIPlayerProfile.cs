
using UnityEngine;
using DG.Tweening;

public class UIPlayerProfile : UIMenuGeneral
{
    [Header("Main Links")]

    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] UIController uiController;

    private bool isActive;

    [Header("Profile Settings")]

    [SerializeField] GameObject profileMenu;
    [SerializeField] UIPlayerInfo playerInfo;
    [SerializeField] UIEditPlayerProfile editProfile;

    public void Init()
    {
        isActive = true;
        CloseMain();

        profileMenu.SetActive(false);

        playerInfo.UpdateHeadInfo(playerProfile);

        editProfile.Init();

        CloseAllWindows();
    }

    #region Edit Profile

    public void OpenEditProfile()
    {
        SwitchShade(false);
        isActive = false;
        editProfile.OpenMain();
    }

    public void CloseEditProfile()
    {
        SwitchShade(true);
        isActive = true;
        editProfile.CloseMain();
    }
    #endregion

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();

        isActive = true;
        CloseAllWindows();
        profileMenu.SetActive(true);
        playerInfo.UpdateHeadInfo(playerProfile);
        playerInfo.OpenMain();
    }

    public override void CloseMain()
    {
        if (isActive)
            base.CloseMain();
    }

    public void CloseAllWindows() { editProfile.CloseMain(); }
    #endregion
}
