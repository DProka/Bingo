
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIMainMenu : MonoBehaviour
{
    [Header("Main Links")]

    [SerializeField] RectTransform menu;
    [SerializeField] RectTransform newGameButton;
    [SerializeField] RectTransform settingsButton;
    [SerializeField] Transform headPart;
    [SerializeField] Transform rightPart;
    [SerializeField] UIBoostersMenu boosterMenu;

    [Header("player Stats")]

    [SerializeField] TextMeshProUGUI[] coinsText;
    [SerializeField] TextMeshProUGUI[] moneyText;
    [SerializeField] TextMeshProUGUI[] crystalText;
    [SerializeField] TextMeshProUGUI[] PlayerLevelButtonText;

    [Header("Player XP Points")]

    [SerializeField] TextMeshProUGUI playerLevelText;
    [SerializeField] ProgressBar progressBar;

    private Canvas canvas;
    private bool interfaceActive;

    public void Init()
    {
        canvas = GetComponent<Canvas>();
    }

    public void UpdateUI(int coins, int money, int crystals, int playedRounds, int xpLvl, int xpPoints, int maxPoints)
    {
        SetTextInArray(coinsText, coins);
        SetTextInArray(moneyText, money);
        SetTextInArray(crystalText, crystals);
        SetTextInArray(PlayerLevelButtonText, playedRounds);
        UpdatePlayerXPPoints(xpLvl, xpPoints, maxPoints, 0f);
    }

    public void HideInterface(bool hide)
    {
        if (hide && interfaceActive)
        {
            headPart.DOMove(new Vector3(0, 8f, 0), 0.2f);
            rightPart.DOMoveX(13.5f, 0.2f);
            settingsButton.DOMoveX(-13f, 0.2f).OnComplete(() =>
            {
                SwitchCanvas(false);
            });
            interfaceActive = false;
            boosterMenu.SwitchMenuByBool(false);
        }
        else if (!hide && !interfaceActive)
        {
            headPart.DOMoveY(4f, 0.2f);
            rightPart.DOMoveX(8.9f, 0.2f);
            settingsButton.DOMoveX(-7.8f, 0.2f);
            interfaceActive = true;
            SwitchCanvas(true);
        }
    }

    public void UpdatePlayerXPPoints(int level, int currentPoints, int maxPoints, float animTime)
    {
        playerLevelText.text = "" + level;
        progressBar.SetBarFullness(currentPoints, maxPoints, animTime);
    }

    public bool CheckCanvasIsActive() { return canvas.enabled; }

    private void SetTextInArray(TextMeshProUGUI[] array, int count)
    {
        string currencyText = CombineCurrency(count);

        foreach (TextMeshProUGUI part in array)
            part.text = currencyText;
    }

    private string CombineCurrency(int currency)
    {
        float price = 0;
        string text = "";

        if (currency / 10000 != 0)
        {
            price = (float)currency / 1000;
            text = string.Format("{0: 0.0}", price) + "K";
            //Debug.Log(price);
        }
        else
        {
            price = currency;
            text = "" + price;
        }

        if (currency / 1000000 != 0)
        {
            price = (float)currency / 1000000;
            text = string.Format("{0: 0.0}", price) + "M";
        }

        if (currency / 1000000000 != 0)
        {
            price = (float)currency / 1000000000;
            text = string.Format("{0: 0.0}", price) + "B";
        }

        if (currency / 1000000000000 != 0)
        {
            price = (float)currency / 1000000000000;
            text = string.Format("{0: 0.0}", price) + "T";
        }

        return text;
    }

    #region Buttons

    public void SwitchNewGameButton(bool isActive) => newGameButton.gameObject.SetActive(isActive);

    public void OpenNewGameMenu() => UIController.Instance.CallScreen(UIController.Menu.JoinGameMenu);
    
    public void OpenSettingsMenu() => UIController.Instance.CallScreen(UIController.Menu.SettingsMenu);

    #endregion

    #region Window

    public void OpenMenu()
    {
        SwitchCanvas(true);
        HideInterface(false);
        SwitchNewGameButton(true);
        interfaceActive = true;
    }

    public void CloseMenu() => SwitchCanvas(false);
    
    private void SwitchCanvas(bool isActive) { canvas.enabled = isActive; }
    #endregion
}
