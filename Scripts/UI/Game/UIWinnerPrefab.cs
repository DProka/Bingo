
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWinnerPrefab : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Image flag;
    [SerializeField] TextMeshProUGUI place;

    public void Init(int num)
    {
        place.text = $"{num} Place";
    }

    public void SetWinnerAvatar(Sprite _avatar, Sprite _flag)
    {
        avatar.sprite = _avatar;
        flag.sprite = _flag;
    }
}
