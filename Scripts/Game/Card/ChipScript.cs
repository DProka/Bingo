
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ChipScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ChipSettings settings;
    [SerializeField] Image imageChip;
    [SerializeField] Image imageFrame;
    [SerializeField] Image imageChest;
    [SerializeField] Image imagePuzzle;
    [SerializeField] Image imageAutoBingo;

    public State currentState { get; private set; }
    public Type currentType { get; private set; }

    private CardController cardController;
    private bool isJackPotChip;
    private int chipNum;


    public void Init(int _chipNum, CardController card)
    {
        cardController = card;
        chipNum = _chipNum;
        isJackPotChip = false;
        SetChipState(State.Open);
    }

    public void SetChipState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Open:
                break;

            case State.Active:
                break;

            case State.Closed:
                imageChip.enabled = true;
                imageChip.sprite = settings.chipSprites[0];
                break;
        
            case State.Bingo:
                imageChip.enabled = true;
                if (!isJackPotChip)
                    imageChip.sprite = settings.chipSprites[1];
                break;
        
            case State.JackPot:
                imageChip.enabled = true;
                imageChip.sprite = settings.chipSprites[2];
                isJackPotChip = true;
                break;
        
        }
    }

    public enum State
    {
        Open,
        Active,
        Closed,
        Bingo,
        JackPot,
    }

    public void SetChipType(Type newGroup)
    {
        ResetChip();
        currentType = newGroup;

        switch (newGroup)
        {
            case Type.Basic:
                break;
        
            case Type.Chest:
                imageChest.enabled = true;
                imageChest.transform.DOPunchScale(new Vector3(0.5f, 0.5f), settings.animationTime, 0).SetDelay(settings.animationDelay).OnComplete(() =>
                {
                    imageChest.transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
                });
                SoundController.Instance.PlaySound(SoundController.Sound.SetChest);
                break;
        
            case Type.Puzzle:
                imagePuzzle.enabled = true;
                break;

            case Type.AutoBingo:
                imageAutoBingo.enabled = true;
                imageAutoBingo.transform.DOPunchScale(new Vector3(0.25f, 0.25f, 0f), settings.animationTime, 0).SetDelay(settings.animationDelay).OnComplete(() =>
                {
                    imageAutoBingo.transform.localScale = new Vector3(0.32f, 0.32f, 0f);
                });
                break;
        }
    }

    public enum Type
    {
        Basic,
        Chest,
        Puzzle,
        AutoBingo
    }

    public void OpenFrame(bool isActive)
    {
        if (currentState == State.Active)
            imageFrame.enabled = isActive;
    }

    public void SetChestSprite(Sprite usedChest) { imageChest.sprite = usedChest; }

    public void OpenChip(bool tap)
    {
        ResetChip();

        if (tap)
        {
            EventBus.onChipClosed?.Invoke(chipNum, tap);
            cardController.ConfirmOpenedChip(chipNum);
        }

        AnimateChipScale(settings.animationDelay);


        switch (currentType)
        {
            case Type.Basic:
                cardController.GetXPBonus(transform.position);
                SoundController.Instance.PlaySound(SoundController.Sound.OpenChip);
                break;
        
            case Type.Chest:
                cardController.GetChestBonus(transform.position);
                SoundController.Instance.PlaySound(SoundController.Sound.OpenChest);
                break;
        
            case Type.Puzzle:
                cardController.GetPuzzleBonus(transform.position);
                //EventBus.onPuzzleOpened?.Invoke(transform.position);
                SoundController.Instance.PlaySound(SoundController.Sound.OpenChest);
                break;
        
            case Type.AutoBingo:
                //cardController.GetAutoBingo(chipNum);
                break;
        }

        SetChipState(State.Closed);
        StartCoroutine(cardController.OpenChip(chipNum));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == State.Active)
            OpenChip(true);
    }

    public void ResetChip()
    {
        imageChip.enabled = false;
        imageFrame.enabled = false;
        imageChest.enabled = false;
        imagePuzzle.enabled = false;
        imageAutoBingo.enabled = false;
    }

    public void AnimateChipScale(float delay)
    {
        Vector3 newScale = new Vector3(0.5f, 0.5f);
        imageChip.transform.DOPunchScale(newScale, settings.animationTime, 0).SetDelay(delay).OnComplete(() =>
        {
            imageChip.transform.localScale = new Vector3(1, 1, 1);
        });
    }
}
