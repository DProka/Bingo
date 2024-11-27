
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIRoundOverScreen : MonoBehaviour
{
    [Header("Main Window")]

    [SerializeField] Image shade;
    [SerializeField] float shadeDelay;
    [SerializeField] Transform roundIsOverImage;

    [Header("Airplane Part")]

    [SerializeField] UIRoundOverAirplane airplanePart;
    [SerializeField] Transform airplaneImageTransform;
    [SerializeField] Image airplaneAnimImage;

    [Header("Reward Part")]

    [SerializeField] UIRoundOverReward rewardPart;
    [SerializeField] float[] rewardSizeArray;

    private Canvas canvas;
    private int puzzleCount;

    private bool airplaneWasUsed;
    private int tutorWasShown = 0;
    private string adLocation = "AirplaneRoundOver";

    public void Init()
    {
        canvas = GetComponent<Canvas>();

        EventBus.onPuzzleOpened += SetPuzzleActive;
        EventBus.onRewardedADClosed += StartAdAirplane;

        rewardPart.Init();
        airplanePart.Init(this);
        tutorWasShown = airplanePart.tutorialWasShown;

        puzzleCount = 0;

        ResetScreen();
    }

    public void UpdateCardRewardText(int coins, int money) => rewardPart.UpdateRewardText(coins, money);

    public void CallScreenByNum(int num)
    {
        switch (num)
        {
            case 0:
                airplanePart.OpenPart();
                break;

            case 1:
                shade.DOFade(0.7f, shadeDelay);
                rewardPart.OpenPart();
                break;
        }
    }

    private void ShadeDelay()
    {
        if (GameController.Instance.currentXPLevel > 2 || GameController.Instance.boosterManager.airplaneCount > 0)
            CallScreenByNum(0);
        else
            CallScreenByNum(1);
    }

    private void SetPuzzleActive()
    {
        puzzleCount++;
        rewardPart.SetPuzzleActive(puzzleCount);
    }

    private void ResetScreen()
    {
        shade.color = new Color(0f, 0f, 0f, 0f);
        rewardPart.ResetPart();
        airplaneWasUsed = false;
        roundIsOverImage.localScale = new Vector3(0, 0, 0);
        ResetAirplane();
    }

    private void SwitchAirplaneScreen(bool isActive) => airplanePart.SwitchObjectActive(isActive);

    private void StartRoundOverAnimation(float delay)
    {
        roundIsOverImage.DOScale(1f, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            roundIsOverImage.DOScale(0f, 0.2f).SetDelay(0.7f).OnComplete(() =>
            {
                UIController.Instance.SwitchTableUI(false);
                GameController.Instance.StartCardsEndRoundMove();
                ShadeDelay();
            });
        });
    }

    #region Airplane

    public void UseAirplane(bool isAD)
    {
        if (isAD)
        {
            MaxSdkManager.Instance.ShowRewarded(adLocation);
            airplanePart.SwitchTimer(false);
        }
        else
        {
            if (!airplaneWasUsed)
            {
                airplanePart.UpdateAirplaneCountText();
                StartAirplaneAnimation();
                GameController.Instance.boosterManager.UseOneAirplane(tutorWasShown == 0);
                airplanePart.SwitchTimer(false);

                if (tutorWasShown == 1)
                {
                    airplaneWasUsed = true;
                    airplanePart.SwitchUseButtonActive(false);
                }
            }
        }

        if (tutorWasShown != airplanePart.tutorialWasShown)
            tutorWasShown = airplanePart.tutorialWasShown;
    }

    private void StartAirplaneAnimation()
    {
        SwitchAirplaneScreen(false);
        airplaneAnimImage.transform.position = airplaneImageTransform.position;

        airplaneAnimImage.transform.DOScale(0.7f, 0.5f);
        airplaneAnimImage.transform.DOMoveY(8, 1.5f).OnComplete(() =>
        {
            airplaneAnimImage.transform.DOMove(new Vector2(-3.67f, -7), 0f);
            airplaneAnimImage.transform.DOMoveY(0, 1f).OnComplete(() =>
            {
                airplaneAnimImage.DOFade(0f, 0.3f).OnComplete(() =>
                {
                    ResetAirplane();
                    StartCoroutine(ContinueTimer(1.5f));
                });
            });
        });

        //airplaneAnimImage.transform.DOMoveY(0, 1.5f).OnComplete(() =>
        //{
        //    airplaneAnimImage.DOFade(0f, 0.3f).OnComplete(() =>
        //    {
        //        ResetAirplane();
        //        StartCoroutine(ContinueTimer(1.5f));
        //    });
        //});
    }

    private void ResetAirplane()
    {
        airplaneAnimImage.transform.DOScale(1f, 0f);
        airplaneAnimImage.transform.DOMoveY(-7, 0);
        airplaneAnimImage.DOFade(1f, 0);
    }

    private IEnumerator ContinueTimer(float delay)
    {
        yield return new WaitForSeconds(delay);

        airplanePart.SwitchTimer(true);
        SwitchAirplaneScreen(true);
    }

    private void StartAdAirplane(string location)
    {
        if (adLocation == location)
        {
            GameController.Instance.boosterManager.UseOneAirplane(true);
            StartAirplaneAnimation();
        }
    }

    #endregion

    #region Main Window

    public void OpenWindow()
    {
        ResetScreen();
        float rewardSizeX = puzzleCount > 0 ? rewardSizeArray[1] : rewardSizeArray[0];
        rewardPart.SetSize(rewardSizeX);
        canvas.enabled = true;

        SoundController.Instance.PlaySound(SoundController.Sound.RoundIsOver);

        StartRoundOverAnimation(0.5f);
    }

    public void CloseWindow()
    {
        canvas.enabled = false;
        puzzleCount = 0;
        EventBus.onWindowClosed?.Invoke();
    }
    #endregion

    private void OnDestroy()
    {
        EventBus.onPuzzleOpened -= SetPuzzleActive;
    }
}
