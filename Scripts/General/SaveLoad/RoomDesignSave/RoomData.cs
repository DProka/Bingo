namespace SaveData
{
    [System.Serializable]
    public class RoomData
    {
        public int _roomTier;
        public int[] _openedObjectsArray;
        public int[] _roomSpritesArray;

        public int[] _cashBonusInRoom;

        public RoomData()
        {
            _roomTier = new int();
            _openedObjectsArray = new int[0];
            _roomSpritesArray = new int[0];

            _cashBonusInRoom = new int[2];
        }
    }
}
