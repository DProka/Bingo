namespace SaveData
{
    [System.Serializable]
    public class PlayerSettingsData
    {
        public int _voice;
        public int _music;
        public int _sound;

        public int _currentBackgroundNum;

        public int _lastBid;
        public int _cardsTutorial;

        public int _gameLanguage;

        public PlayerSettingsData()
        {
            _voice = 1;
            _music = 1;
            _sound = 1;

            _currentBackgroundNum = 0;

            _lastBid = 1;
            _cardsTutorial = 0;

            _gameLanguage = 0;
        }
    }
}
