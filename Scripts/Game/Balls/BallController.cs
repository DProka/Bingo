
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public List<int> ballsNumList { get; private set; }

    [SerializeField] RectTransform[] ballParentArray;

    private TableSettings settings;
    private bool isHorizontal;
    private List<BallObject> ballsList;
    private int nextBallNumber;

    public void Init(TableSettings _settings)
    {
        settings = _settings;

        ballsList = new List<BallObject>();
    }

    #region Numbers

    public void SetListOfNumbers(int[] list)
    {
        ResetBalls();
        ballsNumList = TableCalculations.SetNewNumbersArray(list);
    }

    public void GetTutorialListOfNumbers(int[] list)
    {
        ResetBalls();
        ballsNumList = new List<int>();

        for (int i = 0; i < list.Length; i++)
        {
            ballsNumList.Add(list[i]);
        }
    }
    #endregion

    #region Generator

    public void GenerateNewBall()
    {
        MoveBalls();
        GenerateNewBall(isHorizontal, GetNextBallNumber());
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

    public void SetCardsCount(int count)
    {
        isHorizontal = count <= 2;

        Debug.Log("Balls are horizontal = " + isHorizontal);
    }

    public void GenerateNewBall(bool isHorizontal, int number)
    {
        string[] lettersArray = new string[] { "B", "I", "N", "G", "O" };
        int n = (int)(number / 15.1f);
        Sprite sprite = settings.ballsSprites[n];
        Color color = settings.textColors[n];
        string letter = lettersArray[n];

        BallObject newBall = isHorizontal ? Instantiate(settings.ballPrefab, ballParentArray[0]) : Instantiate(settings.ballPrefab, ballParentArray[1]);
        newBall.transform.localScale = new Vector3(0, 0, 0);
        newBall.SetBall(letter, color, number, sprite);
        ballsList.Add(newBall);

        float ballScale = isHorizontal ? 1 : 0.8f;

        BallAnimation.GetNewBallAnimation(newBall, ballScale, settings.ballAppearSpeed);
    }

    private void CheckExcessBalls()
    {
        if (ballsList.Count > 5)
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
        if (ballsList.Count > 0)
        {
            for (int i = 0; i < ballsList.Count; i++)
            {
                BallAnimation.MoveBall(ballsList[i], settings.ballMoveSpeed, isHorizontal);
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
