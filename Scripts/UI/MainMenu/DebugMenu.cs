
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] TableController tableController;
    [SerializeField] GameObject mainWindow;

    private bool menuIsActive;

    [SerializeField] int addCoins;
    [SerializeField] int addMoney;
    [SerializeField] int addCrystals;

    [Header("Autoclick")]

    [SerializeField] TextMeshProUGUI autoclickButtonText;

    private bool autoclickActive;

    public void Init()
    {
        menuIsActive = false;
        mainWindow.SetActive(false);

        autoclickActive = false;
    }

    public void GetCoins() => GameController.Instance.CalculateCoins(addCoins);
    public void GetMoney() => GameController.Instance.CalculateMoney(addMoney);
    public void GetCrystals() => GameController.Instance.CalculateCrystals(addCrystals);

    public void ShowInterstitial()
    {
        //MaxSdkManager.showInterstitial?.Invoke("DebugTest");
        MaxSdkManager.Instance.ShowRewarded("DebugTest");
    }

    public void IncreaseLvl(bool plus) 
    { 
        if(plus)
            gameController.IncreaseRoundCount(1);
        else
            gameController.IncreaseRoundCount(-1);
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

    public void SetAutoclick()
    {
        if (!autoclickActive)
        {
            autoclickActive = true;
            autoclickButtonText.text = "Autoclick Is ON";
        }
        else
        {
            autoclickActive = false;
            autoclickButtonText.text = "Autoclick Is OFF";
        }

        tableController.SwitchAutoGame(autoclickActive);
    }

    public void SetBoosterCount(int num)
    {
        BoosterManager.Type type = BoosterManager.Type.Airplane;

        switch (num)
        {
            case 0:
                type = BoosterManager.Type.Plus5Balls;
                break;
        
            case 1:
                type = BoosterManager.Type.DoubleProgress;
                break;
        
            case 2:
                type = BoosterManager.Type.AutoBingo;
                break;
        
            case 3:
                type = BoosterManager.Type.DoubleMoney;
                break;
        
            case 4:
                type = BoosterManager.Type.AutoDoub;
                break;
        
            case 5:
                type = BoosterManager.Type.TripleDoub;
                break;
        
            case 6:
                type = BoosterManager.Type.Airplane;
                break;
        }

        GameController.Instance.boosterManager.SetBoosterCount(type, 1); 
    }

    public void SetBoosterTime(int num)
    {
        BoosterManager.Type type = BoosterManager.Type.Airplane;

        switch (num)
        {
            case 0:
                type = BoosterManager.Type.Hint;
                break;

            case 1:
                type = BoosterManager.Type.Battery;
                break;

            case 2:
                type = BoosterManager.Type.AutoBonus;
                break;

            case 3:
                type = BoosterManager.Type.WildDaub;
                break;

            case 4:
                type = BoosterManager.Type.DoubleXp;
                break;
        }

        GameController.Instance.boosterManager.SetBoosterTime(type, 5);
    }
}
