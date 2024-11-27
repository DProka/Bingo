
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using Spine;

public class PuzzlePartPrefabScript : MonoBehaviour
{
    public int partID { get; private set; }
    public int puzzleNum { get; private set; }
    public int partNum { get; private set; }
    public Status currentStatus { get; private set; }


    [SerializeField] Image backImage;
    [SerializeField] Image[] partImageArray;
    [SerializeField] Image recievedImage;
    [SerializeField] SkeletonGraphic glowAnimation;
    [SerializeField] SkeletonGraphic lockAnimation;

    private Sprite[] backSpritesArray;
    private Sprite[] statusSpritesArray;
    private RectTransform rectTransform;

    public void Init(int[] settings, PuzzleMenuSettings menuSettings)
    {
        partID = settings[0];
        puzzleNum = settings[1];
        partNum = settings[2];
        SetPartSettings(menuSettings);
        backSpritesArray = menuSettings.partBackArray;
        statusSpritesArray = menuSettings.partStatusArray;

        rectTransform = GetComponent<RectTransform>();
    }

    public void SetStatus(Status status)
    {
        glowAnimation.enabled = false;
        glowAnimation.AnimationState.TimeScale = 0f;
        lockAnimation.enabled = false;
        lockAnimation.AnimationState.TimeScale = 0f;
        recievedImage.enabled = false;
        backImage.enabled = true;

        foreach (Image img in partImageArray)
            img.enabled = true;

        switch (status)
        {
            case Status.Closed:
                currentStatus = status;
                backImage.sprite = backSpritesArray[0];
                recievedImage.sprite = statusSpritesArray[0];
                lockAnimation.enabled = true;
                lockAnimation.AnimationState.SetAnimation(0, "animation", false);
                SwitchPartImage(false);
                break;

            case Status.Open:
                currentStatus = status;
                backImage.sprite = backSpritesArray[1];
                glowAnimation.enabled = true;
                glowAnimation.AnimationState.SetAnimation(0, "Idel", true);
                glowAnimation.AnimationState.TimeScale = 1f;
                SwitchPartImage(true);
                break;

            case Status.Recieved:
                currentStatus = status;
                backImage.sprite = backSpritesArray[2];
                recievedImage.sprite = statusSpritesArray[1];
                recievedImage.enabled = true;
                SwitchPartImage(false);
                break;
        }
    }

    public enum Status
    {
        Closed,
        Open,
        Recieved,
    }

    public void OpenPart()
    {
        StartOpenAnimation();
    }

    public void SwitchPartImage(bool isActive)
    {
        partImageArray[0].DOFade(isActive ? 1 : 0.3f, 0.3f);
        partImageArray[1].enabled = isActive;
    }

    public Vector2 GetPartPosition() { return rectTransform.position; }

    public bool CheckAvailability()
    {
        bool availability = false;

        if (currentStatus == Status.Open)
            if (rectTransform.position.x > -4.5 && rectTransform.position.x < 4.5)
                availability = true;

        return availability;
    }

    public void CheckActivity()
    {
        if (transform.position.x > -6.5 && transform.position.x < 6.5)
        {
            backImage.enabled = true;
            foreach (Image img in partImageArray)
                img.enabled = true;

            switch (currentStatus)
            {
                case Status.Closed:
                    lockAnimation.enabled = true;
                    partImageArray[1].enabled = false;
                    break;
            
                case Status.Open:
                    glowAnimation.enabled = true;
                    break;
            
                case Status.Recieved:
                    recievedImage.enabled = true;
                    partImageArray[1].enabled = false;
                    break;
            }
        }
        else
        {
            glowAnimation.enabled = false;
            lockAnimation.enabled = false;
            recievedImage.enabled = false;
            backImage.enabled = false;
            foreach (Image img in partImageArray)
                img.enabled = false;
        }
    }

    private void StartOpenAnimation()
    {
        lockAnimation.AnimationState.Complete += ContinueOpenAnimation;

        glowAnimation.transform.DOScale(0f, 0f);
        lockAnimation.AnimationState.SetAnimation(0, "animation", false);
        lockAnimation.AnimationState.TimeScale = 1f;
        SoundController.Instance.PlaySound(SoundController.Sound.BonusUse);
    }
    
    private void ContinueOpenAnimation(TrackEntry trackEntry)
    {
        lockAnimation.AnimationState.Complete -= ContinueOpenAnimation;

        SetStatus(Status.Open);
        glowAnimation.enabled = true;
        glowAnimation.transform.DOScale(1f, 0.5f);
    }

    private void SetPartSettings(PuzzleMenuSettings menuSettings)
    {
        int index = 0;

        for (int i = 0; i < menuSettings.puzzlesArray.Length; i++)
        {
            for (int j = 0; j < menuSettings.puzzlesArray[i].partSpritesArray.Length; j++)
            {
                if (index == partID)
                {
                    puzzleNum = i;
                    partNum = j;
                    partImageArray[0].sprite = menuSettings.puzzlesArray[i].partSpritesArray[j];
                    partImageArray[1].sprite = menuSettings.puzzlesArray[i].partSpritesArray[j];
                }

                index++;
            }
        }
    }
}
