
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UINewLevelScreen : MonoBehaviour
{
    [SerializeField] Image shadeImage;
    [SerializeField] NewLevelScreenSettings settings;

    [Header("Level Part")]

    [SerializeField] Transform levelImageTransform; 
    [SerializeField] Transform rewardObjectTransform;
    [SerializeField] Transform collectButtonTransform;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] ProgressBar levelBar;

    [Header("Level Part")]

    [SerializeField] Transform rewardPart;
    [SerializeField] Transform[] rewardPrefsArray;

    private UINewLevelRewardPrefab[] rewardScriptsArray;

    public void Init()
    {
        rewardScriptsArray = new UINewLevelRewardPrefab[rewardPrefsArray.Length];

        for (int i = 0; i < rewardPrefsArray.Length; i++)
        {
            rewardScriptsArray[i] = rewardPrefsArray[i].GetComponent<UINewLevelRewardPrefab>();
            rewardScriptsArray[i].Init(settings);
        }
    }

    #region Level Bar Part

    public void UpdateBarFullness(float currentPoints, float maxPoints) => levelBar.SetBarFullness(currentPoints, maxPoints, 0);

    public void StartNewLevelAnimation(int[] stats)
    {
        int newLvl = stats[0];
        int playerPoints = stats[2];
        int pointsToNextLvl = stats[3];
        int pointsRest = stats[1];
        int pointsToSecondLvl = stats[4];

        OpenMain();
        lvlText.text = "" + (newLvl - 1);
        levelBar.SetBarFullness(GameController.Instance.previousXPPointsValue, pointsToNextLvl, 0);
        shadeImage.DOFade(0.6f, 0.3f).OnComplete(() => 
        {
            levelImageTransform.DOScale(1, 0.5f);
            rewardObjectTransform.DOScale(1, 0.5f).OnComplete(() =>
            {
                levelBar.SetBarFullness(playerPoints - pointsRest, pointsToNextLvl, 2f);
                StartCoroutine(ContinueNewLevelAnimation(newLvl, pointsRest, pointsToSecondLvl));
            });
        });
    }

    private IEnumerator ContinueNewLevelAnimation(int playerLvl, int pointsRest, int pointsToSecondLvl)
    {
        yield return new WaitForSeconds(2f);

        lvlText.text = "" + playerLvl;
        lvlText.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
        //StartCollectAnimation();
        StartRewardPartAnimation();
    }

    private void StartCollectAnimation()
    {
        collectButtonTransform.DOScale(1, 0.3f);
    }

    private void ResetAnimation()
    {
        shadeImage.DOFade(0, 0f);
        levelImageTransform.DOScale(0, 0);
        rewardObjectTransform.DOScale(0, 0);
        levelBar.ResetProgress();
        collectButtonTransform.DOScale(0, 0);
        ResetRewardPartAnimation();
    }

    #endregion

    #region Reward Part

    public void SetLevelReward(BoosterManager.Type[] typesArray, int[] counts)
    {
        for (int i = 0; i < rewardScriptsArray.Length; i++)
        {
            rewardScriptsArray[i].SetReward(typesArray[i], counts[i]);
        }
    }

    private void StartRewardPartAnimation()
    {
        rewardPart.DOScale(1, 0.3f);

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < rewardPrefsArray.Length; i++)
        {
            sequence.Append(rewardPrefsArray[i].DOScale(1, 0.3f));
        }

        sequence.Play().OnComplete(() => collectButtonTransform.DOScale(1, 0.3f));
    }

    private void ResetRewardPartAnimation()
    {
        rewardPart.DOScale(0, 0);

        foreach (Transform pref in rewardPrefsArray)
            pref.DOScale(0, 0);
    }

    #endregion

    #region Screen

    public void OpenMain()
    {
        gameObject.SetActive(true);
        EventBus.onWindowOpened?.Invoke();
    }

    public void CloseMain()
    {
        ResetAnimation();
        gameObject.SetActive(false);
        EventBus.onWindowClosed?.Invoke();
    }
    #endregion
}
