
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BallObject : MonoBehaviour
{
    public TextMeshProUGUI textLetter;
    public TextMeshProUGUI textNumber;
    public Image ballImage;

    public void SetBall(string letter, Color textColor, int number, Sprite sprite)
    {
        textLetter.text = letter;
        textLetter.color = textColor;
        textNumber.text = number.ToString();
        //textNumber.color = textColor;
        textNumber.outlineColor = textColor;
        ballImage.sprite = sprite;
    }
}
