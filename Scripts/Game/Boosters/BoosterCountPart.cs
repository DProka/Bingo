
public class BoosterCountPart
{
    public int autoDoubCount { get; private set; }
    public int tripleDoubCount { get; private set; }
    public int airplaneCount { get; private set; }
    public int doubleMoneyCount { get; private set; }

    private BoosterSave boosterSave;

    public BoosterCountPart(BoosterSave _boosterSave)
    {
        boosterSave = _boosterSave;
        Load();
    }

    public void UseOneBoosters(BoosterManager.Type type)
    {
        switch (type)
        {
            case BoosterManager.Type.AutoDoub:
                autoDoubCount -= 1;
                break;
        
            case BoosterManager.Type.TripleDoub:
                tripleDoubCount -= 1;
                break;
        
            case BoosterManager.Type.DoubleMoney:
                doubleMoneyCount -= 1;
                break;
        }

        //if (autoDoubCount > 0)
        //{
        //    autoDoubCount -= 1;
        //}

        //if (tripleDoubCount > 0)
        //{
        //    tripleDoubCount -= 1;
        //}

        //if (doubleMoneyCount > 0)
        //{
        //    doubleMoneyCount -= 1;
        //}

        Save();
    }

    public void SetCountByType(BoosterManager.Type type, int count)
    {
        switch (type)
        {
            case BoosterManager.Type.AutoDoub:
                autoDoubCount += count;
                break;
        
            case BoosterManager.Type.TripleDoub:
                tripleDoubCount += count;
                break;
        
            case BoosterManager.Type.Airplane:
                airplaneCount += count;
                break;
        
            case BoosterManager.Type.DoubleMoney:
                doubleMoneyCount += count;
                break;
        }

        Save();
    }

    public int GetCountByType(BoosterManager.Type type)
    {
        int count = 0;

        switch (type)
        {
            case BoosterManager.Type.AutoDoub:
                count = autoDoubCount;
                break;

            case BoosterManager.Type.TripleDoub:
                count = tripleDoubCount;
                break;

            case BoosterManager.Type.Airplane:
                count = airplaneCount;
                break;

            case BoosterManager.Type.DoubleMoney:
                count = doubleMoneyCount;
                break;
        }

        return count;
    }

    public void UseOneAirplane()
    {
        if (airplaneCount > 0)
            airplaneCount -= 1;
        else
            airplaneCount = 0;

        Save();
    }

    private void Save() => boosterSave.SaveCountBoosters(new int[] { autoDoubCount, tripleDoubCount, airplaneCount, doubleMoneyCount });

    private void Load()
    {
        autoDoubCount = boosterSave.autoDoubCount;
        tripleDoubCount = boosterSave.tripleDoubCount;
        airplaneCount = boosterSave.airplaneCount;
        doubleMoneyCount = boosterSave.doubleMoneyCount;
    }
}
