namespace SaveData
{
    [System.Serializable]
    public class UIData
    {
        public int _capsulesCount;
        public string _lastCapsuleTime;
        public string _nextCapsuleTime;
        public bool _gachaTutorWasShown;

        public int _rateUs;

        public int _backGroundNum;
        public int _lastOpenedBackground;
        public bool _backGroundWasShown;

        public UIData()
        {
            _capsulesCount = 0;
            _lastCapsuleTime = System.DateTime.Now.ToString();
            _nextCapsuleTime = System.DateTime.Now.AddMinutes(UIController.Instance.settings.gachaSettings.GetMinutesByCount(_capsulesCount)).ToString();
            _gachaTutorWasShown = false;

            _rateUs = 0;

            _backGroundNum = 0;
            _lastOpenedBackground = 0;
            _backGroundWasShown = false;
        }
    }
}
