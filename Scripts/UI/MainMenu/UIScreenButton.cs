
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScreenButton : UIButtonMain
{
    public Status currentStatus { get; private set; }

    [SerializeField] Image mainImage;
    [SerializeField] Image attentionImage;
    [SerializeField] Type buttonType;

    private bool attentionIsActive;
    private bool animationIsActive;

    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (attentionIsActive && !animationIsActive)
            StartAttentionAnimation();
    }

    public void SwitchStatus(Status newStatus)
    {
        currentStatus = newStatus;
        mainImage.enabled = false;
        attentionImage.enabled = false;

        switch (newStatus)
        {
            case Status.Closed:
                break;

            case Status.Open:
                mainImage.enabled = true;
                break;

            case Status.Attention:
                mainImage.enabled = true;
                attentionImage.enabled = true;
                break;
        }
    }

    public enum Status
    {
        Closed,
        Open,
        Attention
    }
    
    public enum Type
    {
        Backgrounds,
        Puzzles,
        Gacha,
        Boosters,
    }

    public void SwitchAttentionActive(bool isActive)
    {
        attentionIsActive = isActive;

        StartAttentionAnimation();
    }

    public void OnMouseDown()
    {
        if (mainImage.enabled)
        {
            switch (buttonType)
            {
                case Type.Backgrounds:
                    UIController.Instance.CallScreen(UIController.Menu.BackGroundsMenu);
                    break;
            
                case Type.Puzzles:
                    UIController.Instance.CallScreen(UIController.Menu.PuzzleMenu1);
                    break;
            
                case Type.Gacha:
                    UIController.Instance.CallScreen(UIController.Menu.GachaMenu);
                    break;
            
                case Type.Boosters:
                    UIController.Instance.CallScreen(UIController.Menu.BoosterScreen);
                    break;
            }

            SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
        }
    }

    private void StartAttentionAnimation()
    {
        animationIsActive = true;

        attentionImage.DOFade(0, 1f). SetDelay(1f).OnComplete(() =>
        {
            attentionImage.DOFade(1, 1f).OnComplete(() =>
            {
                animationIsActive = false;
            });
        });
    }

}
