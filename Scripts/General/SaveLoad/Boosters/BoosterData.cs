namespace SaveData
{
    [System.Serializable]
    public class BoosterData
    {
        public int _ballsPlus5Count;
        public int _doubleProgressCount;
        public int _autoBingoCount;

        public int _doubleMoneyCount;
        public int _autoDoubCount;
        public int _tripleDoubCount;
        public int _airplaneCount;

        public string _hintTime;
        public string _batteryTime;
        public string _autoBonusTime;
        public string _wildDoubTime;
        public string _doubleXpTime;

        public BoosterData()
        {
            _ballsPlus5Count = 0;
            _doubleProgressCount = 0;
            _autoBingoCount = 0;

            _doubleMoneyCount = 0;
            _autoDoubCount = 0;
            _tripleDoubCount = 0;
            _airplaneCount = 0;

            _hintTime = System.DateTime.Now.ToString();
            _batteryTime = System.DateTime.Now.ToString();
            _autoBonusTime = System.DateTime.Now.ToString();
            _wildDoubTime = System.DateTime.Now.ToString();
            _doubleXpTime = System.DateTime.Now.ToString();
        }
    }
}

