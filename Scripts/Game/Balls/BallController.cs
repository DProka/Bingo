
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool isHorizontal;

    [Header("Balls Animation")]

    [SerializeField] float newBallAnimDuration;
    [SerializeField] float ballMovingDuration;

    [Header("Other")]

    private BallGenerator ballGen;
    private List<BallObject> ballsList;

    private BallNumberGenerator ballNum;
    private List<int> ballsNumList;
    private int nextBallNumber;

    public void Init()
    {
        ballNum = GetComponent<BallNumberGenerator>();
        ballGen = GetComponent<BallGenerator>();

        ballsList = new List<BallObject>();
    }

    #region Numbers

    public void GetTutorialListOfNumbers(int[] list)
    {
        ballsNumList = new List<int>();

        for (int i = 0; i < list.Length; i++)
        {
            ballsNumList.Add(list[i]);
        }
    }
    
    public void SetListOfNumbers(int[] list)
    {
        ballNum.SetNewNumbersArray(list);
        ballsNumList = ballNum.GetNumbersList();
    }
    
    public int GetBallsLeft() { return ballsNumList.Count; }
    #endregion

    #region Generator

    public void GenerateNewBall()
    {
        MoveBalls();
        BallObject ball = ballGen.GenerateNewBall(isHorizontal, GetNextBallNumber());
        ballsList.Add(ball);
        BallAnimation.GetNewBallAnimation(ball, newBallAnimDuration);
        CheckExcessBalls();
    }

    public int GetBall() { return nextBallNumber; }

    public void ResetBalls()
    {
        if (ballsList.Count > 0)
        {
            for (int i = 0; i < ballsList.Count; i++)
            {
                Destroy(ballsList[i].gameObject);
            }
        }

        ballsList = new List<BallObject>();
    }

    public bool CheckAvailableBalls()
    {
        if (ballsNumList.Count <= 0)
        {
            return false;
        }
        else
            return true;
    }

    public void GetCardsCount(int count)
    {
        if (count <= 2)
        {
            isHorizontal = true;
        }
        else
        {
            isHorizontal = false;
        }
    }

    private void CheckExcessBalls()
    {
        if(ballsList.Count > 5)
        {
            BallAnimation.HideBall(ballsList[0], 0.3f);
            StartCoroutine(RemoveBall());
        }
    }

    private IEnumerator RemoveBall()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(ballsList[0].gameObject);
        ballsList.RemoveAt(0);
    }

    private void MoveBalls()
    {
        if(ballsList.Count > 0)
        {
            for (int i = 0; i < ballsList.Count; i++)
            {
                BallAnimation.MoveBall(ballsList[i], ballMovingDuration, isHorizontal);
            }
        }
    }

    private int GetNextBallNumber()
    {
        int num = 0;

        if (ballsNumList.Count > 0)
        {
            num = ballsNumList[0];
            ballsNumList.RemoveAt(0);
        }

        nextBallNumber = num;
        return num;
    }
    #endregion
}
