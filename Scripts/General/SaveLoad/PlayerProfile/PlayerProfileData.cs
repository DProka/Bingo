namespace SaveData
{
    [System.Serializable]
    public class PlayerProfile
    {
        public string _playerID;

        public string _playerNickName;
        public int _avatarNumber;
        public int _playerCountry;
        public string _playerEMail;
        public int _playerGender;
        public int[] _playerBirthDate;

        public int _playedCards;
        public int _totalBingos;
        
        public int _gameLevel;
        public int _pointsToLevel;

        public int _playerCredits;

        public PlayerProfile()
        {
            _playerID = "PlayerID";

            _playerNickName = "NewPlayer";
            _avatarNumber = 0;
            _playerCountry = 0;
            _playerEMail = "Enter Email";
            _playerGender = 0;
            _playerBirthDate = new int[2];

            _playedCards = 0;
            _totalBingos = 0;

            _gameLevel = 0;
            _pointsToLevel = 0;

            _playerCredits = 10;

        }
    }
}