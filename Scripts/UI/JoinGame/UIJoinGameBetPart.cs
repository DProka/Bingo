
using UnityEngine;
using UnityEngine.UI;

public class UIJoinGameBetPart : MonoBehaviour
{
    public int betCount { get; private set; }
    public int maxBetCount { get; private set; }

    [SerializeField] Image buttonMinusImage;
    [SerializeField] Image buttonPlusImage;

    [SerializeField] Image[] lockImagesArray;
    [SerializeField] Image[] betImagesArray;

    private JoinGameMenuSettings settings;

    public void Init(JoinGameMenuSettings _settings)
    {
        settings = _settings;
        maxBetCount = 1;
    }

    public void SetBetCount(int bet) { betCount = bet; }

    public void SetBet(bool plus)
    {
        if (plus && betCount < 5)
        {
            betCount += 1;
            SoundController.Instance.PlayBidSelect(betCount - 2);
        }
        else if (!plus && betCount > 1)
        {
            betCount -= 1;
            SoundController.Instance.PlayBidSelect(betCount - 1);
        }

        Debug.Log("BetCount = " + betCount );
    }

    public void SetTutorBet(int num)
    {
        betCount = num;
        
        if(betCount > 1)
            SoundController.Instance.PlayBidSelect(betCount - 2);

        Debug.Log("Tutorial BetCount = " + betCount);
    }

    public void SetMaxBetCount()
    {
        int currentLvl = GameController.Instance.currentXPLevel;

        for (int i = 0; i < settings.xpLevelsArray.Length; i++)
        {
            if (currentLvl >= settings.xpLevelsArray[i])
            {
                maxBetCount = i + 2;
                Debug.Log("MaxBet = " + maxBetCount + ", XP level = " + currentLvl);
            }
        }
    }

    public void CheckBetButtonsImage()
    {
        if (betCount == 1)
            buttonMinusImage.sprite = settings.minusImageArray[0];
        else if (betCount > 1)
            buttonMinusImage.sprite = settings.minusImageArray[1];

        if (betCount == 5)
            buttonPlusImage.sprite = settings.plusImageArray[0];
        else if (betCount < 5)
            buttonPlusImage.sprite = settings.plusImageArray[1];

        //if (GameController.Instance.currentXPLevel < settings.xpLevelsArray[0])
          //  betCount = 1;
    }

    public void SwitchBetImage()
    {
        for (int i = 0; i < betImagesArray.Length; i++)
        {
            if (i < betCount - 1)
            {
                betImagesArray[i].enabled = true;
                //lockImagesArray[i].enabled = false;
            }
            else
            {
                betImagesArray[i].enabled = false;
                //lockImagesArray[i].enabled = true;
            }
        }

        CheckLockedBets();
    }

    public void CheckLockedBets()
    {
        for (int i = 0; i < lockImagesArray.Length; i++)
        {
            if(GameController.Instance.currentXPLevel < settings.xpLevelsArray[i])
            {
                lockImagesArray[i].enabled = true;
            }
            else
                lockImagesArray[i].enabled = false;
        }
    }

}
