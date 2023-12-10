
using UnityEngine;
using TMPro;

public class UIPlayerInfo : MonoBehaviour
{
    [Header("Player Statistic")]

    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textID;
    [SerializeField] GameObject mainWindow;
    [SerializeField] UIPlayerStatistic playerStatistic;

    public void UpdateHeadInfo(PlayerProfile profile)
    {
        textName.text = profile.playerNickName;
        textID.text = $"Player ID: {profile.playerID}";
        playerStatistic.UpdateStatistic(profile);
    }

    public void OpenMain() { mainWindow.SetActive(true); }
    public void CloseMain() { mainWindow.SetActive(false); }
}
