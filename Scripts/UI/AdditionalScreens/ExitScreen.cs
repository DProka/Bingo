
using UnityEngine;

public class ExitScreen : UIMenuGeneral
{
    [Header("Exit Screen")]

    [SerializeField] GameObject gameExitScreen;
    [SerializeField] GameObject roundExitScreen;

    private bool isUIScreen;

    public void Init()
    {
        gameExitScreen.SetActive(false);
        roundExitScreen.SetActive(false);
    }

    public void OpenScreen(bool _isUIScreen)
    {
        if (!gameObject.activeSelf && GameController.Instance.playedRoundsCount > 1)
        {
            isUIScreen = _isUIScreen;

            if (_isUIScreen)
            {
                gameExitScreen.SetActive(true);
                roundExitScreen.SetActive(false);
            }
            else
            {
                gameExitScreen.SetActive(false);
                roundExitScreen.SetActive(true);
                GameController.Instance.SwitchGameIsActive(false);
            }

            OpenMain();
        }
    }

    public void CloseScreen()
    {
        if (!isUIScreen)
            GameController.Instance.SwitchGameIsActive(true);
        else
            EventBus.onWindowClosed?.Invoke();

        CloseMain();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ExitRound()
    {
        GameController.Instance.CalculateMoney(-100);
        UIController.Instance.CallScreen(UIController.Menu.MainMenu);
    }
}
