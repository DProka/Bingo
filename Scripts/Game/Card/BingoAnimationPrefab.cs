using System.Collections;
using UnityEngine;
using Spine.Unity;
using Spine;
using TMPro;
using DG.Tweening;

public class BingoAnimationPrefab : MonoBehaviour
{
    [SerializeField] SkeletonGraphic bingoSkel;
    [SerializeField] SkeletonGraphic jackPotSkel;
    [SerializeField] TextMeshProUGUI coinBonusText;
    [SerializeField] TextMeshProUGUI moneyBonusText;

    //private UIAnimationScreen rewardScreen;
    private bool isJackPot;

    public void Init(UIAnimationScreen _rewardScreen, int bingoNum, int[] currency, bool _isJackPot)
    {
        bingoSkel.gameObject.SetActive(false);
        jackPotSkel.gameObject.SetActive(false);

        //rewardScreen = _rewardScreen;
        isJackPot = _isJackPot;

        if (!isJackPot)
        {
            ActivateAnim(bingoNum);
            StartCoroutine(StartBingoTextAnim(currency));
        }
        else
        {
            StartCoroutine(ActivateJackPotAnim());
            StartCoroutine(StartJackpotTextAnimation(currency[0]));
        }
    }

    private void ActivateAnim(int num)
    {
        bingoSkel.gameObject.SetActive(true);
        bingoSkel.AnimationState.Complete += OnAnimationComplete;

        switch (num)
        {
            case 1:
                bingoSkel.AnimationState.SetAnimation(0, "Bingo", false);
                SoundController.Instance.PlayBingoSound(1);
                break;
        
            case 2:
                bingoSkel.AnimationState.SetAnimation(0, "DoubleBingo", false);
                SoundController.Instance.PlayBingoSound(2);
                break;
        
            case 3:
                bingoSkel.AnimationState.SetAnimation(0, "TripleBingo", false);
                SoundController.Instance.PlayBingoSound(3);
                break;
        
            case >3:
                bingoSkel.AnimationState.SetAnimation(0, "TripleBingo", false);
                SoundController.Instance.PlayBingoSound(3);
                break;
        }
    }

    private IEnumerator ActivateJackPotAnim()
    {
        jackPotSkel.gameObject.SetActive(true);
        jackPotSkel.AnimationState.Complete += OnAnimationComplete;

        jackPotSkel.AnimationState.SetAnimation(0, "Jackpot", false);
        SoundController.Instance.PlayJackPotSound();

        yield return new WaitForSeconds(2f);

        //rewardScreen.SwitchGameActive();
        SwitchGameActive();
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        bingoSkel.AnimationState.Complete -= OnAnimationComplete;
        jackPotSkel.AnimationState.Complete -= OnAnimationComplete;

        if (isJackPot)
            Destroy(gameObject);
    }

    private IEnumerator StartBingoTextAnim(int[] currency)
    {
        int bingoCoins = currency[0];
        int bingoMoney = currency[1];

        coinBonusText.transform.localScale = new Vector3(0, 0, 0);
        moneyBonusText.transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(1.3f);

        coinBonusText.text = $"+{bingoCoins}";
        coinBonusText.gameObject.SetActive(true);
        coinBonusText.transform.DOScale(1, 0.3f);
        coinBonusText.transform.DOMoveY(coinBonusText.transform.position.y + 2f, 1.5f);

        moneyBonusText.text = $"+{bingoMoney}";
        moneyBonusText.gameObject.SetActive(true);
        moneyBonusText.transform.DOScale(1, 0.3f);
        moneyBonusText.transform.DOMoveY(moneyBonusText.transform.position.y + 2f, 1.5f);

        coinBonusText.transform.DOScale(0, 0.3f).SetDelay(1.7f);
        moneyBonusText.transform.DOScale(0, 0.3f).SetDelay(1.7f);

        yield return new WaitForSeconds(2f);

        coinBonusText.gameObject.SetActive(false);
        coinBonusText.transform.localPosition = new Vector3(0, 50, 0);

        moneyBonusText.gameObject.SetActive(false);
        moneyBonusText.transform.localPosition = new Vector3(0, -50, 0);

        //rewardScreen.SwitchGameActive();
        SwitchGameActive();
        Destroy(gameObject);
    }

    private IEnumerator StartJackpotTextAnimation(int coins)
    {
        coinBonusText.transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(1.3f);

        coinBonusText.text = $"+{coins}";
        coinBonusText.gameObject.SetActive(true);
        coinBonusText.transform.DOScale(1, 0.3f);
        coinBonusText.transform.DOMoveY(coinBonusText.transform.position.y + 2f, 1.5f);

        coinBonusText.transform.DOScale(0, 0.3f).SetDelay(1.7f);

        yield return new WaitForSeconds(2f);

        coinBonusText.gameObject.SetActive(false);
        coinBonusText.transform.localPosition = new Vector3(0, 50, 0);

        //rewardScreen.SwitchGameActive();
        SwitchGameActive();
        Destroy(gameObject);
    }

    private void SwitchGameActive() => GameController.Instance.SwitchGameIsActive(true); // GameController.switchGameIsActive?.Invoke(true);
}
