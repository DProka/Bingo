
using UnityEngine;

public class GeneralSave : MonoBehaviour
{
    private int playerCoins;
    private int playerMoney;
    private int playerLvl;

    private int tutorStatus;

    private const string saveKey = "generalSave";

    #region Player Credits

    public int GetCoins() { return playerCoins; }

    public int GetMoney() { return playerMoney; }

    public int GetLevel() { return playerLvl; }

    public void CalculateCoins(bool isPlus, int count)
    {
        if (isPlus)
        {
            playerCoins += count;
            Debug.Log($"Coins Added {count}");
            Debug.Log($"player Coins {playerCoins}");
        }
        else
        {
            if(playerCoins >= count)
                playerCoins -= count;
        }

        Save();
    }
    
    public void CalculateMoney(bool isPlus, int count)
    {
        if (isPlus)
        {
            playerMoney += count;
        }
        else
        {
            if(playerMoney >= count)
                playerMoney -= count;
        }

        Save();
    }

    public void UpdatePlayerLvl(int lvl)
    {
        playerLvl = lvl;

        Save();
    }
    #endregion

    public void SetTutorialStatus(int status) 
    { 
        tutorStatus = status;

        Save();
    }

    public int GetTutorialStatus() { return tutorStatus; }

    #region Save Load

    public void ResetSave()
    {
        SaveData.GeneralData general = new SaveData.GeneralData();

        playerCoins = general._playerCoins;
        playerMoney = general._playerMoney;
        playerLvl = general._playerLvl;

        tutorStatus = general._tutorStatus;
        
        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.GeneralData>(saveKey);

        playerCoins = data._playerCoins;
        playerMoney = data._playerMoney;
        playerLvl = data._playerLvl;

        tutorStatus = data._tutorStatus;
    }

    private SaveData.GeneralData GetSaveSnapshot()
    {
        SaveData.GeneralData data = new SaveData.GeneralData()
        {
            _playerCoins = playerCoins,
            _playerMoney = playerMoney,
            _playerLvl = playerLvl,

            _tutorStatus = tutorStatus,
        };

        return data;
    }
    #endregion
}
