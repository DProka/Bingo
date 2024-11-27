
using UnityEngine;
using UnityEngine.UI;

public class UIJoinMenuCardPart : MonoBehaviour
{
    [SerializeField] UICardCountButton[] cardButtonsArray;
    [SerializeField] Image cardsLock;
    [SerializeField] UIMessageToUnlock messageToUnlock;
    [SerializeField] UITutorial4Cards tutorial;

    private JoinGameMenuSettings settings;

    public void Init(JoinGameMenuSettings _settings)
    {
        settings = _settings;

        cardButtonsArray[0].Init(settings, true);
        cardButtonsArray[1].Init(settings, false);
        tutorial.Init();

        messageToUnlock.HideMessage();
    }

    public void SwitchChoosedCardsCount(int count)
    {
        switch (count)
        {
            case 2:
                cardButtonsArray[0].SwitchActive(true);
                cardButtonsArray[1].SwitchActive(false);
                break;

            case 4:
                cardButtonsArray[0].SwitchActive(false);
                cardButtonsArray[1].SwitchActive(true);
                break;
        }
    }

    public void Set4CardsLock(bool isOpen)
    {
        if (isOpen)
        {
            cardsLock.enabled = false;
            cardButtonsArray[1].SetButtonActive(true);
            tutorial.OpenTutorial();
        }
        else
        {
            cardsLock.enabled = true;
            cardButtonsArray[1].SetButtonActive(false);
        }
    }

    public void SwitchChestSprite(int num)
    {
        foreach (UICardCountButton button in cardButtonsArray)
            button.SetChestSprite(num);
    }

    public void StartLockMessageAnimation() => messageToUnlock.StartMessageAnimation();
}
