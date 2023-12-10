
using UnityEngine;
using TMPro;

public class UITable : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] TextMeshProUGUI bingoCountText;
  
    [Header("Cards")]

    [SerializeField] GameObject tableCard1;
    [SerializeField] GameObject tableCard2;
    [SerializeField] GameObject tableCard3;
    [SerializeField] GameObject tableCard4;

    [Header("Balls")]

    [SerializeField] GameObject containerH;
    [SerializeField] GameObject containerV;

    [Header("Boost")]

    [SerializeField] BoosterController boosterController;

    #region Table Part

    public void OpenTable(int tableCount)
    {
        HideAllComponents();

        if (tableCount == 1)
        {
            tableCard1.SetActive(true);
            containerH.SetActive(true);
        }
        if (tableCount == 2)
        {
            tableCard2.SetActive(true);
            containerH.SetActive(true);
        }
        if (tableCount == 3)
        {
            tableCard3.SetActive(true);
            containerV.SetActive(true);
        }
        if (tableCount == 4)
        {
            tableCard4.SetActive(true);
            containerV.SetActive(true);
        }
    }

    public void HideAllComponents()
    {
        tableCard1.SetActive(false);
        tableCard2.SetActive(false);
        tableCard3.SetActive(false);
        tableCard4.SetActive(false);

        containerH.SetActive(false);
        containerV.SetActive(false);
    }

    public void SetBingoCountText(int count)
    {
        bingoCountText.text = count.ToString();
    }
    #endregion

    #region Main Window

    public void OpenMenu() { menu.SetActive(true); }
    
    public void CloseMenu() { menu.SetActive(false); }
    #endregion
}
