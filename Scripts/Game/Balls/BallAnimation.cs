using DG.Tweening;
using UnityEngine;

public class BallAnimation : MonoBehaviour
{
    public static void GetNewBallAnimation(BallObject ball, float duration)
    {
        ball.transform.DOScale(1f, duration);
    }

    public static void MoveBall(BallObject ball, float duration, bool isHorizontal)
    {
        if (ball.transform.localScale.x > 0.6)
        {
            if(isHorizontal)
                ball.transform.DOMoveX(ball.transform.position.x + ball.gameObject.transform.localScale.x * 1.7f, duration);
            else
                ball.transform.DOMoveY(ball.transform.position.y - ball.gameObject.transform.localScale.y * 1.7f, duration);
        }
        
        ball.transform.DOLocalRotate(new Vector3(0f , 0f, -360f), 0.5f, RotateMode.FastBeyond360);
    }

    public static void HideBall(BallObject ball, float duration)
    {
        ball.transform.DOScale(new Vector3(0f, 0f, 0f), duration);
    }
}
