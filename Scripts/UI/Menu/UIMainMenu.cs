
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIMainMenu : MonoBehaviour
{
    [Header("Main Links")]

    [SerializeField] GeneralSave generalData;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject cityButton;
    [SerializeField] GameObject headMenu;

    private float headStartPos;
    private float gameButtonStartPos;
    private float cityButtonStartPos;
    private bool interfaceActive;

    [Header("player Stats")]

    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI PlayerLevelButtonText;

    [Header("Room Menu")]

    [SerializeField] UIJoinGame joinGameMenu;

    public void Init()
    {
        headStartPos = headMenu.transform.position.y;
        gameButtonStartPos = newGameButton.transform.position.x;
        cityButtonStartPos = cityButton.transform.position.x;
    }

    public void UpdateUI()
    {
        coinsText.text = generalData.GetCoins().ToString();
        moneyText.text = generalData.GetMoney().ToString();
        SetPlayerLevel();
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void HideInterface(bool hide)
    {
        if (hide)
        {
            headStartPos = headMenu.transform.position.y;
            gameButtonStartPos = newGameButton.transform.position.x;
            cityButtonStartPos = cityButton.transform.position.x;

            headMenu.transform.DOMoveY(9f, 0.2f);
            newGameButton.transform.DOMoveX(14f, 0.2f);
            cityButton.transform.DOMoveX(-14f, 0.2f);
        }
        else
        {
            headMenu.transform.DOMoveY(headStartPos, 0.2f);
            newGameButton.transform.DOMoveX(gameButtonStartPos, 0.2f);
            cityButton.transform.DOMoveX(cityButtonStartPos, 0.2f);
        }
    }

    public void SetPlayerLevel()
    {
        PlayerLevelButtonText.text = $"{generalData.GetLevel()}";
        //joinGameMenu.SetLevelText(generalData.GetLevel());
        joinGameMenu.SetMaxBetCount(generalData.GetLevel());
    }
}
