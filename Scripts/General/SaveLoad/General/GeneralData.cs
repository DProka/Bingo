namespace SaveData
{
    [System.Serializable]
    public class GeneralData
    {
        public int _playerCoins;
        public int _playerMoney;
        public int _playerCrystals;
        public int _playerLvl;

        public int _currentRoomNum;

        public int _playerXPLevel;
        public int _playerXPPoints;

        public int _tutorStatus;
        public int _tutorRoomProgress;
        public int _tutorGameProgress;

        public int _rateUs;

        public GeneralData()
        {
            _playerCoins = 20000;
            _playerMoney = 100;
            _playerCrystals = 0;
            _playerLvl = 0;

            _currentRoomNum = 0;

            _playerXPLevel = 1;
            _playerXPPoints = 0;

            _tutorStatus = 0;
            _tutorRoomProgress = 0;
            _tutorGameProgress = 0;

            _rateUs = 0;
        }
    }
}
