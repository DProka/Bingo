namespace SaveData
{
    [System.Serializable]
    public class GeneralData
    {
        public int _playerCoins;
        public int _playerMoney;
        public int _playerLvl;

        public int _tutorStatus;

        public GeneralData()
        {
            _playerCoins = 20000;
            _playerMoney = 100;
            _playerLvl = 0;
            
            _tutorStatus = 0;
        }
    }
}
