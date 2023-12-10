
using UnityEngine;

public class BallGenerator : MonoBehaviour
{
    [SerializeField] RectTransform hPivot;
    [SerializeField] RectTransform vPivot;
    
    [SerializeField] GameObject ballPrefab;

    [Header("Ball Sprites")]

    [SerializeField] string[] lettersArray;
    [SerializeField] Color[] textColors;
    [SerializeField] Sprite[] ballsSprites;

    public BallObject GenerateNewBall(bool isHorizontal, int number)
    {
        GameObject newBall = isHorizontal ? Instantiate(ballPrefab, hPivot) : Instantiate(ballPrefab, vPivot);
        BallObject ball = newBall.GetComponent<BallObject>();
        ball.transform.localScale = new Vector3(0, 0, 0);

        ball.SetBall(SetLetter(number), SetColor(number), number, SetSprite(number));

        return ball;
    }

    public Sprite SetSprite(int number)
    {
        int n = (int)(number / 15.1f);
        Sprite sprite = ballsSprites[n];

        return sprite;
    }

    private Color SetColor(int number)
    {
        int n = (int)(number / 15.1f);
        Color color = textColors[n];

        return color;
    }
    
    private string SetLetter(int number)
    {
        int n = (int)(number / 15.1f);
        string letter = lettersArray[n];

        return letter;
    }
}
