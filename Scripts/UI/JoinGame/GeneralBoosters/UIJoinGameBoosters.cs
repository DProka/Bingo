
using UnityEngine;

public class UIJoinGameBoosters : MonoBehaviour
{
    [Header("Boosters")]

    [SerializeField] UIGeneralBoosterButton[] boosterButtonsArray;

    private UIJoinGame mainMenu;
    private JoinGameMenuSettings settings;

    public void Init(UIJoinGame _mainMenu, JoinGameMenuSettings _settings)
    {
        mainMenu = _mainMenu;
        settings = _settings;

        EventBus.onRoundEnded += CheckBoosters;

        InitializeBoosters();
    }

    public void CheckBoosters()
    {
        //HideAllMessages();
        CheckBoostersAvailability();
        UpdateCounts();
    }

    public void ActivateBooster(int num, bool isActive) => mainMenu.SwitchBoosterByNum(num, isActive);

    public void UpdateCounts()
    {
        boosterButtonsArray[0].UpdateCount(GameController.Instance.boosterManager.ballsPlus5Count);
        boosterButtonsArray[1].UpdateCount(GameController.Instance.boosterManager.doubleProgressCount);
        boosterButtonsArray[2].UpdateCount(GameController.Instance.boosterManager.autoBingoCount);
    }

    public void HideAllMessages()
    {
        foreach (UIGeneralBoosterButton obj in boosterButtonsArray)
        {
            obj.HideMessage();
        }
    }

    private void InitializeBoosters()
    {
        for (int i = 0; i < boosterButtonsArray.Length; i++)
        {
            boosterButtonsArray[i].Init(this, settings, i, 0);
        }

        boosterButtonsArray[0].UpdateCount(GameController.Instance.boosterManager.ballsPlus5Count);
        boosterButtonsArray[1].UpdateCount(GameController.Instance.boosterManager.doubleProgressCount);
        boosterButtonsArray[2].UpdateCount(GameController.Instance.boosterManager.autoBingoCount);

        CheckBoostersAvailability();
        //HideAllMessages();
    }

    private void CheckBoostersAvailability()
    {
        for (int i = 0; i < boosterButtonsArray.Length; i++)
        {
            if (GameController.Instance.currentXPLevel >= settings.bonusUnlockLvlArray[i])
                boosterButtonsArray[i].UpdateStatus(UIGeneralBoosterButton.Status.Open);
            else
                boosterButtonsArray[i].UpdateStatus(UIGeneralBoosterButton.Status.Closed);
        }
    }

    private void OnDestroy()
    {
        EventBus.onRoundEnded -= CheckBoosters;
    }
}
