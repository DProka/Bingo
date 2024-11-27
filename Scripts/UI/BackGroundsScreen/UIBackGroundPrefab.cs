
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIBackGroundPrefab : MonoBehaviour
{
    public State currentState { get; private set; }

    [SerializeField] Image backImage;
    [SerializeField] Image arrowImage;
    [SerializeField] GameObject[] framesArray;

    private UIBackGroundsScreen backGroundsScreen;
    private TextMeshProUGUI[] reachRoundTextArray;
    private int id;


    public void Init(UIBackGroundsScreen _backGroundsScreen, int _id, Sprite backSprite)
    {
        backGroundsScreen = _backGroundsScreen;
        id = _id;
        backImage.sprite = backSprite;

        reachRoundTextArray = framesArray[0].transform.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI text in reachRoundTextArray)
        {
            text.text = "Unlock at Round " + id * 20;
        }
    }

    public void ActivateCurrentBackGround()
    {
        if(id * 20 <= GameController.Instance.playedRoundsCount)
            backGroundsScreen.ActivateBackGround(id);
    }

    public void SwitchNextArrow(bool isActive) => arrowImage.enabled = isActive;

    public void SwitchState(State newState)
    {
        currentState = newState;
        foreach (GameObject frame in framesArray)
            frame.SetActive(false);

        switch (newState)
        {
            case State.Locked:
                framesArray[0].SetActive(true);
                break;
        
            case State.Open:
                framesArray[1].SetActive(true);
                break;
        
            case State.Active:
                framesArray[2].SetActive(true);
                break;
        }

        
        backImage.transform.DOScale(1.04f, 0.3f);
    }

    public enum State
    {
        Locked,
        Open,
        Active,
    }
}
