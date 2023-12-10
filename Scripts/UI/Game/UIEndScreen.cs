
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UIEndScreen : MonoBehaviour
{
    [Header("Main Window")]

    [SerializeField] GameObject window;
    //[SerializeField] GameObject back;
    [SerializeField] Image shade;
    [SerializeField] float shadeDelay;
    [SerializeField] GameObject roundOverImg;

    [Header("Back Button")]

    [SerializeField] GameObject backButton;
    [SerializeField] float buttonDelay;

    [Header("Reward Text")]

    [SerializeField] GameObject rewardObj;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI moneyText;
    
    [Header("Reward Animation")]

    [SerializeField] UIPileReward coinsPile;
    [SerializeField] UIPileReward moneyPile;

    [SerializeField] RectTransform coinsFinishPos;
    [SerializeField] RectTransform moneyFinishPos;

    public void Init()
    {
        coinsPile.Init();
        moneyPile.Init();
    }

    public void UpdateCardRewardText(int coins, int money) 
    { 
        coinsText.text = $"{coins}"; 
        moneyText.text = $"{money}"; 
    }
    
    IEnumerator ShadeDelay()
    {
        roundOverImg.transform.DOScale(0, 0);
        rewardObj.transform.DOScale(0, 0);

        shade.DOFade(0.7f, shadeDelay);
        
        yield return new WaitForSeconds(shadeDelay);

        //back.SetActive(true);
        //back.transform.DOScale(1, 0.2f);

        roundOverImg.transform.DOScale(1f, 0.2f);
        rewardObj.transform.DOScale(1f, 0.2f);

        StartCoroutine(ButtonDelay());
    }

    IEnumerator ButtonDelay()
    {
        yield return new WaitForSeconds(buttonDelay);

        backButton.SetActive(true);
        backButton.transform.DOScale(1, 0.2f);
    }

    #region Main Window

    public void OpenWindow()
    {
        window.SetActive(true);
        backButton.SetActive(false);
        //back.SetActive(false);

        //back.transform.localScale = new Vector3(0.1f, 0.1f, 0);
        backButton.transform.localScale = new Vector3(0.1f, 0.1f, 0);
        shade.color = new Color(0f, 0f, 0f, 0f);

        StartCoroutine(ShadeDelay());
    }

    public void StartRewardAnimation()
    {
        coinsPile.SetFinishPosition(coinsFinishPos.position);
        moneyPile.SetFinishPosition(moneyFinishPos.position);

        coinsPile.StartRewardPileAnimation();
        moneyPile.StartRewardPileAnimation();
    }

    public void CloseWindow() 
    {
        window.SetActive(false);
    }
    #endregion
}
