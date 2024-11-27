
using System.Linq;

public class PlayerXPCalculator
{
    public int[] levelConditionsArray { get; private set; }

    public PlayerXPCalculator(int[] conditions)
    {
        levelConditionsArray = conditions;

        int[] anotherLvls = new int[100];
        int lastCost = levelConditionsArray[levelConditionsArray.Length - 1];

        for (int i = 0; i < anotherLvls.Length; i++)
        {
            lastCost += 5000;
            anotherLvls[i] = lastCost;
        }

        int[] newArray = levelConditionsArray.Concat(anotherLvls).ToArray();
        levelConditionsArray = newArray;
    }

    public int[] CheckNewLevel(int currentLvl, int currentPoints)
    {
        int[] checkedStats = new int[] { currentLvl, currentPoints, currentPoints, 0, 0 };
        int maxPoints = levelConditionsArray[currentLvl];

        if (currentPoints >= maxPoints)
        {
            checkedStats[0] = currentLvl += 1;
            checkedStats[1] = currentPoints - maxPoints;
            checkedStats[2] = currentPoints;
            checkedStats[3] = levelConditionsArray[checkedStats[0] - 1];
            checkedStats[4] = levelConditionsArray[checkedStats[0]];
        }

        return checkedStats;
    }
}
