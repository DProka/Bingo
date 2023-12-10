using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoomInfo : MonoBehaviour
{
    [Header("Prepare")]

    [SerializeField] GameObject wivConditionImage;

    [Header("Room Statistic")]
    [SerializeField] GameObject roomStat;
    [SerializeField] TextMeshProUGUI players;
    [SerializeField] TextMeshProUGUI bingos;

    public void SetPrepairing()
    {
        roomStat.SetActive(false);
        wivConditionImage.SetActive(true);
    }

    public void SetStatistic(int _players, int _bingos)
    {
        roomStat.SetActive(true);
        wivConditionImage.SetActive(false);

        players.text = _players.ToString();

        bingos.text = _bingos.ToString();
    }
}
