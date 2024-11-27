namespace SaveData
{
    [System.Serializable]
    public class PuzzleMenuData
    {
        public int _nextNum;
        public int _newPuzzleCount;
        public int[] _puzzlesOrderArray;
        public int[] _partsOrderArray;
        public int[] _partsStatusArray;
        public int _buttonIsActive;
        public int _tutorComplete;

        public PuzzleMenuData()
        {
            _nextNum = 0;
            _newPuzzleCount = 0;
            _puzzlesOrderArray = new int[0];
            _partsOrderArray = new int[0];
            _partsStatusArray = new int[0];
            _buttonIsActive = 0;
            _tutorComplete = 0;
        }
    }
}
