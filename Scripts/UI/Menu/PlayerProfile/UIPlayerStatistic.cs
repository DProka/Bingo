
using UnityEngine;
using TMPro;

public class UIPlayerStatistic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playedCards; 
    [SerializeField] TextMeshProUGUI totalBingos;

    public void UpdateStatistic(PlayerProfile profile)
    {
        playedCards.text = $"{profile.playedCards}";
        totalBingos.text = $"{profile.totalBingos}";
    }
}
