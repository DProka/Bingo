
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UIRoundOverReward : MonoBehaviour
{
    [SerializeField] GameObject mainObject;

    [Header("Head Part")]

    [SerializeField] GameObject rewardTextImage;

    [Header("Middle Part")]

    [SerializeField] RectTransform rewardImagesTransform;
    [SerializeField] GameObject rewardObject;
    [SerializeField] GameObject puzzleObj;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI puzzleText;

    [Header("Lower Part")]

    [SerializeField] GameObject backButton;
    [SerializeField] float buttonDelay;

    private bool doubleCoinsIsActive;
    private int coinReward;

    public void Init()
    {
        //rewardImagesTransform = rewardObject.GetComponent<RectTransform>();

        SwitchObjectActive(false);
        puzzleObj.SetActive(false);
    }

    public void UpdateRewardText(int coins, int money)
    {
        doubleCoinsIsActive = GameController.Instance.boosterManager.doubleCoinsIsActive;

        //if (doubleCoinsIsActive)
        //    coinReward = coins / 2;

        coinReward = coins;

        if (doubleCoinsIsActive)
            coinsText.text = "x" + (coinReward / 2);
        else
            coinsText.text = "x" + coinReward;
        moneyText.text = "x" + money;

        Debug.Log("coins earned = " + coinReward + " money earned = " + money);
    }

    public void SetPuzzleActive(int count)
    {
        puzzleText.text = "x" + count;
        puzzleObj.SetActive(count > 0);
    }

    public void SetSize(float sizeX)
    {
        rewardImagesTransform.sizeDelta = new Vector2(sizeX, rewardImagesTransform.sizeDelta.y);
        Debug.Log("Reward size = " + sizeX);
    }

    public void ResetPart()
    {
        backButton.SetActive(false);
        backButton.transform.localScale = new Vector3(0.1f, 0.1f, 0);
        rewardTextImage.transform.localScale = new Vector3(0, 0, 0);
        rewardObject.transform.localScale = new Vector3(0, 0, 0);
        //roundIsOverImage.localScale = new Vector3(0, 0, 0);
    }

    public void SwitchObjectActive(bool isActive) => mainObject.SetActive(isActive);

    public void OpenPart() => StartRewardAnimation();

    private void StartRewardAnimation()
    {
        SwitchObjectActive(true);

        SoundController.Instance.PlaySound(SoundController.Sound.RewardScreen);
        rewardTextImage.transform.DOScale(1f, 0.2f).SetDelay(0.7f);
        rewardObject.transform.DOScale(1f, 0.2f).SetDelay(0.7f).OnComplete(() =>
        {
            if (doubleCoinsIsActive)
                AnimateDoubleCoins();
        });

        //if (doubleCoinsIsActive)
        //    AnimateDoubleCoins();
        //else
        if (!doubleCoinsIsActive)
            backButton.transform.DOScale(1, 0.2f).SetDelay(buttonDelay).OnComplete(() => backButton.SetActive(true));

        //roundIsOverImage.DOScale(1f, 0.5f).SetDelay(delay).OnComplete(() =>
        //{
        //SoundController.Instance.PlaySound(SoundController.Sound.RewardScreen);
        //    rewardTextImage.transform.DOScale(1f, 0.2f).SetDelay(0.7f);
        //    rewardObject.transform.DOScale(1f, 0.2f).SetDelay(0.7f);
        //roundIsOverImage.DOScale(0f, 0.2f).SetDelay(0.7f);
        //backButton.transform.DOScale(1, 0.2f).SetDelay(buttonDelay).OnComplete(() => backButton.SetActive(true));
        //});
    }

    private void AnimateDoubleCoins()
    {
        int bonusCoins = coinReward / 2;
        int bonusCoinsPart = bonusCoins / 10;
        int newCoins = 0;

        float delay = 0f;
        int stepCount = 0;

        for (int i = 0; i < 10; i++)
        {
            coinsText.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f, 3).SetDelay(delay).OnComplete(() =>
            {
                bonusCoins += bonusCoinsPart;
                coinsText.text = "x" + bonusCoins;
                stepCount++;
                if (stepCount == 10)
                    backButton.transform.DOScale(1, 0.2f).SetDelay(buttonDelay).OnComplete(() => backButton.SetActive(true));
            });

            delay += 0.3f;

        }
    }
}
