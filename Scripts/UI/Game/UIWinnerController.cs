
using UnityEngine;

public class UIWinnerController : MonoBehaviour
{
    [SerializeField] PlayerProfile playerProfile;
    [SerializeField] UIEditAvatar avatar;
    [SerializeField] UIEditPlayerCountry flag;

    [SerializeField] UIWinnerPrefab[] winnersArray;

    [SerializeField] Sprite noAvatar;
    [SerializeField] Sprite noFlag;

    private int place;

    public void Init()
    {
        ResetWinners();
    }

    public void SetBotWinner()
    {
        if(place < winnersArray.Length)
        {
            place += 1;
            winnersArray[place - 1].SetWinnerAvatar(avatar.GetRandomAvatar(), flag.GetRandomFlag());
        }
    }

    public void SetPlayerWinner()
    {
        if (place < winnersArray.Length)
        {
            place += 1;
            winnersArray[place - 1].SetWinnerAvatar(avatar.GetAvatar(playerProfile.avatarNumber), flag.GetFlag(playerProfile.playerCountry));
        }
    }

    public void ResetWinners()
    {
        place = 0;

        for (int i = 0; i < winnersArray.Length; i++)
        {
            winnersArray[i].Init(i + 1);
            winnersArray[i].SetWinnerAvatar(noAvatar, noFlag);
        }
    }
}
