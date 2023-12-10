namespace SaveData
{
    [System.Serializable]
    public class PlayerSettings
    {
        public int _voice;
        public int _music;
        public int _sound;

        public int _gameLanguage;

        public PlayerSettings()
        {
            _voice = 1;
            _music = 1;
            _sound = 1;

            _gameLanguage = 0;
        }
    }
}
