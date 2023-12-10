
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject mainWindow;

    private bool menuIsActive;

    [SerializeField] int addCoins;
    [SerializeField] int addMoney;

    public void Init()
    {
        menuIsActive = false;
        mainWindow.SetActive(false);
    }

    public void GetCoins() { gameController.CalculateCoins(true, addCoins); }
    public void GetMoney() { gameController.CalculateMoney(true, addMoney); }

    public void IncreaseLvl(bool plus) 
    { 
        if(plus)
            gameController.IncreasePlayerLvl(1);
        else
            gameController.IncreasePlayerLvl(-1);
    }

    public void ResetGame()
    {
        gameController.ResetProgress();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMenu() 
    {
        if (!menuIsActive)
        {
            mainWindow.SetActive(true);
            menuIsActive = true;
        }
        else
        {
            mainWindow.SetActive(false);
            menuIsActive = false;
        }
    }
}
