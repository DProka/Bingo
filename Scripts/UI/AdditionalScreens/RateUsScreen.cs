using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RateUsScreen : UIMenuGeneral
{
    [Header("Rate Us Part")]

    [SerializeField] Image girlImage;
    [SerializeField] GameObject window1;
    [SerializeField] GameObject window2;
    [SerializeField] GameObject window3;

    public void Init()
    {
        window1.transform.DOScale(0, 0);
        window2.transform.DOScale(0, 0);
        window3.transform.DOScale(0, 0);

        window1.SetActive(false);
        window2.SetActive(false);
        window3.SetActive(false);

        girlImage.DOFade(0, 0f);
        girlImage.gameObject.SetActive(false);
    }

    public void CallRateUs() => Invoke("CheckIsShown", 0.3f);

    private void CheckIsShown()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        CallWindow(1);
    }

    private void CallWindow(int num)
    {
        switch (num)
        {
            case 1:
                AnimateWindow1();
                break;

            case 2:
            case 3:
                AnimateWindow2(num);
                break;
        }
    }

    private void AnimateWindow1()
    {
        girlImage.gameObject.SetActive(true);
        girlImage.DOFade(1, 0.5f).SetDelay(0.2f).OnComplete(() =>
        {
            OpenMain();
            window1.transform.DOScale(1, 0.3f);
        });
    }

    private void AnimateWindow2(int num)
    {
        window1.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            if(num == 2)
            {
                window2.SetActive(true);
                window2.transform.DOScale(1, 0.3f);
            }
            else if (num == 3)
            {
                window3.SetActive(true);
                window3.transform.DOScale(1, 0.3f);
            }

        });
        
    }

    #region Buttons

    public void OpenPlayMarket()
    {
        Application.OpenURL("market://details?id=bingo.games.lucky.journey");
        girlImage.DOFade(0, 0.5f).OnComplete(() => CloseMain());
    }

    public void OpenEmailMessage()
    {
        Application.OpenURL("mailto:e.karavashkin@gmail.com?subject=Bingo_Feedback");
        girlImage.DOFade(0, 0.5f).OnComplete(() => CloseMain());
    }

    #endregion

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();
        EventBus.onWindowOpened?.Invoke();
    }

    public override void CloseMain()
    {
        base.CloseMain();
        EventBus.onWindowClosed?.Invoke();
    }
    #endregion
}
