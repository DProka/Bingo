
public class GeneralSave
{
    public static GeneralSave Instance;

    public int playerCoins { get; private set; }
    public int playerMoney { get; private set; }
    public int playerCrystals { get; private set; }
    public int playerLvl { get; private set; }

    public int currentRoomNum { get; private set; }

    public int playerXPLevel { get; private set; }
    public int playerXPPoints { get; private set; }

    public int[] generalBoosterCountArray { get; private set; }

    public int tutorStatus { get; private set; }
    public int tutorRoomProgress { get; private set; }
    public int tutorGameProgress { get; private set; }

    public int rateUs { get; private set; }

    private const string saveKey = "generalSave";

    public GeneralSave()
    {
        Instance = this;

        playerCoins = 20000;
        playerMoney = 100;
        playerLvl = 0;

        currentRoomNum = 0;

        playerXPLevel = 1;
        playerXPPoints = 0;

        tutorStatus = 0;
        tutorRoomProgress = 0;
        tutorGameProgress = 0;

        rateUs = 0;
    }

    #region Player Credits

    public void CalculateCoins(int count)
    {
        playerCoins += count;

        if (playerCoins < 0)
            playerCoins = 0;

        Save();
    }

    public void CalculateMoney(int count)
    {
        playerMoney += count;

        if (playerMoney < 0)
            playerMoney = 0;

        Save();
    }
    
    public void CalculateCrystals(int count)
    {
        playerCrystals += count;

        if (playerCrystals < 0)
            playerCrystals = 0;

        Save();
    }

    public void UpdatePlayerLvl(int lvl)
    {
        playerLvl = lvl;

        Save();
    }

    public void UpdateRoomNum(int roomNum)
    {
        currentRoomNum = roomNum;

        Save();
    }

    public void UpdateRateUs()
    {
        if (rateUs == 0)
            rateUs = 1;

        Save();
    }

    public void SavePlayerXP(int[] xpStats)
    {
        playerXPLevel = xpStats[0];
        playerXPPoints = xpStats[1];

        Save();
    }

    public void SetBoostersCount(int[] countArray)
    {
        generalBoosterCountArray = countArray;
        //Save();
    }
    #endregion

    #region Tutorial Progress

    public void SetTutorialStatus(int status)
    {
        tutorStatus = status;

        Save();
    }

    public void UpdateTutorialProgress(int room, int game)
    {
        tutorRoomProgress = room;
        tutorGameProgress = game;

        Save();
    }

    public int[] GetTutorialProgress()
    {
        int[] array = new int[2];
        array[0] = tutorRoomProgress;
        array[1] = tutorGameProgress;
        return array;
    }
    #endregion

    #region Save Load

    public void ResetSave()
    {
        SaveData.GeneralData general = new SaveData.GeneralData();

        playerCoins = general._playerCoins;
        playerMoney = general._playerMoney;
        playerCrystals = general._playerCrystals;
        playerLvl = general._playerLvl;

        currentRoomNum = general._currentRoomNum;

        playerXPLevel = general._playerXPLevel;
        playerXPPoints = general._playerXPPoints;

        tutorStatus = general._tutorStatus;
        tutorRoomProgress = general._tutorRoomProgress;
        tutorGameProgress = general._tutorGameProgress;

        rateUs = general._rateUs;

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
        playerCrystals = data._playerCrystals;
        playerLvl = data._playerLvl;

        currentRoomNum = data._currentRoomNum;

        playerXPLevel = data._playerXPLevel;
        playerXPPoints = data._playerXPPoints;

        tutorStatus = data._tutorStatus;
        tutorRoomProgress = data._tutorRoomProgress;
        tutorGameProgress = data._tutorGameProgress;

        rateUs = data._rateUs;
    }

    private SaveData.GeneralData GetSaveSnapshot()
    {
        SaveData.GeneralData data = new SaveData.GeneralData()
        {
            _playerCoins = playerCoins,
            _playerMoney = playerMoney,
            _playerCrystals = playerCrystals,
            _playerLvl = playerLvl,

            _currentRoomNum = currentRoomNum,

            _playerXPLevel = playerXPLevel,
            _playerXPPoints = playerXPPoints,

            _tutorStatus = tutorStatus,
            _tutorRoomProgress = tutorRoomProgress,
            _tutorGameProgress = tutorGameProgress,

            _rateUs = rateUs,
        };

        return data;
    }
    #endregion
}
