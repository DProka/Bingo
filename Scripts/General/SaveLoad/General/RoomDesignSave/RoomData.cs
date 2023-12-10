namespace SaveData
{
    [System.Serializable]
    public class RoomData
    {
        public int _activeRoom;
        
        public int[] _roomProgressArray;
        
        public int[] _room1SpritesArray;
        
        public RoomData()
        {
            _activeRoom = 0;
            _roomProgressArray = new int[1];

            _room1SpritesArray = new int[1];
        }
    }
}
