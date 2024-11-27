using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TableCalculations
{
    #region Bingo

    public static List<int[]> GetBingoStrokeList(int jacpotCount)
    {
        List<int[]> bingoStrokesList = new List<int[]>()
        {
        new int[] { 5, 6, 7, 8, 9 },
        new int[] { 10, 11, 12, 13, 14 },
        new int[] { 15, 16, 17, 18, 19 },
        new int[] { 0, 5, 10, 15, 20 },
        new int[] { 1, 6, 11, 16, 21 },
        new int[] { 2, 7, 12, 17, 22 },
        new int[] { 3, 8, 13, 18, 23 },
        new int[] { 4, 9, 14, 19, 24 },
        new int[] { 0, 6, 12, 18, 24 },
        new int[] { 4, 8, 12, 16, 20 },
        new int[] { 0, 4, 12, 20, 24 }
        };

        if (jacpotCount == 0)
        {
            bingoStrokesList.Add(new int[] { 0, 1, 2, 3, 4 });
            bingoStrokesList.Add(new int[] { 20, 21, 22, 23, 24 });
        }

        return bingoStrokesList;
    }

    public static int CheckJackpotChance(int chance)
    {
        int num = Random.Range(1, 100);
        int jackpotCount = 0;

        Debug.Log($"JackPot procentage - {num}");

        if (num <= chance)
            jackpotCount = 1;

        return jackpotCount;
    }

    public static int[] GetConditionNumbers(int num)
    {
        int[] array = new int[5];

        for (int i = 0; i < array.Length; i++)
        {
            switch (num)
            {
                case 0:
                    array[i] = i;
                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                    array[i] = i + (5 * num);
                    break;

                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    array[i] = i * 5 + (num - 5);
                    break;

                case 10:
                    array[i] = i * 6;
                    break;

                case 11:
                    array[i] = (i + 1) * 4;
                    break;

                case 12:
                    array = new int[] { 0, 4, 12, 20, 24 };
                    break;
            }
        }

        return array;
    }

    public static int[] RandomizeBingoNumbers()
    {
        int[] bingoArray = new int[25];

        int[] b = new int[5];
        int[] i = new int[5];
        int[] n = new int[5];
        int[] g = new int[5];
        int[] o = new int[5];

        b = RandomizeColumnNumbers(1, 16);
        i = RandomizeColumnNumbers(16, 31);
        n = RandomizeColumnNumbers(31, 46);
        g = RandomizeColumnNumbers(46, 61);
        o = RandomizeColumnNumbers(61, 76);

        for (int s = 0; s < bingoArray.Length; s++)
        {
            if (bingoArray[s] == 0)
            {
                if (s < 5)
                    bingoArray[s] = b[s];

                if (s >= 5 && s < 10)
                    bingoArray[s] = i[s - 5];

                if (s >= 10 && s < 15)
                    bingoArray[s] = n[s - 10];

                if (s >= 15 && s < 20)
                    bingoArray[s] = g[s - 15];

                if (s >= 20 && s < bingoArray.Length)
                    bingoArray[s] = o[s - 20];
            }
        }

        return bingoArray;
    }

    private static int[] RandomizeColumnNumbers(int minValue, int maxValue)
    {
        int[] values = new int[5];

        for (int i = 0; i < values.Length; i++)
        {
            bool contains;
            int next;

            do
            {
                next = Random.Range(minValue, maxValue);
                contains = false;

                for (int index = 0; index < i; index++)
                {
                    int n = values[index];
                    if (n == next)
                    {
                        contains = true;
                        break;
                    }
                }
            }

            while (contains);

            values[i] = next;
        }

        return values;
    }

    #endregion

    #region Cards

    public static int CalculateBingoCoins(int bingoCount, int jackPot)
    {
        int bingoCoins = 0;

        switch (bingoCount)
        {
            case 1:
                bingoCoins = 1000;
                break;

            case 2:
                bingoCoins = 2000;
                break;

            case 3:
            case > 3:
                bingoCoins = 5000;
                break;
        }

        return bingoCoins * jackPot;
    }

    public static int CalculateBingoMoney(int bingoCount, int jackPot)
    {
        int bingoMoney = new int();

        switch (bingoCount)
        {
            case 1:
                bingoMoney = 30;
                break;

            case 2:
                bingoMoney = 50;
                break;

            case 3:
            case > 3:
                bingoMoney = 70;
                break;
        }

        return bingoMoney * jackPot;
    }

    public static int GetJackpotBonus(int jackPot)
    {
        int bonus = 0;

        switch (jackPot)
        {
            case 1:
                bonus = 5000;
                break;

            case 2:
                bonus = 10000;
                break;

            case 3:
                bonus = 20000;
                break;

            case 4:
                bonus = 50000;
                break;

            case 5:
                bonus = 100000;
                break;
        }

        return bonus;
    }

    #endregion

    #region Balls

    public static List<int> SetNewNumbersArray(int[] numbersArray)
    {
        List<int> numbers = new List<int>();

        int[] newNumbers = FillEmptyNumbers(numbersArray);

        for (int i = 0; i < newNumbers.Length; i++)
        {
            numbers.Add(newNumbers[i]);
        }

        Debug.Log("Shuffled Numbers: " + string.Join(", ", newNumbers));

         return ShuffleNumbers(numbers);
    }

    private static int[] FillEmptyNumbers(int[] array)
    {
        List<int> numbersList = new List<int>();

        for (int i = 0; i < 75; i++)
        {
            numbersList.Add(i + 1);
        }

        foreach (int num in array)
        {
            if (numbersList.Contains(num))
            {
                numbersList.Remove(num);
            }
        }

        int[] fullArray = array;

        for (int i = 0; i < fullArray.Length; i++)
        {
            if (fullArray[i] == 0)
            {
                int randomIndex = Random.Range(0, numbersList.Count);
                fullArray[i] = numbersList[randomIndex];
                numbersList.Remove(fullArray[i]);
            }
        }

        return fullArray;
    }

    private static List<int> ShuffleNumbers(List<int> numbers)
    {
        List<int> shuffledNumbers = new List<int>();

        List<int>[] categories = new List<int>[5];
        for (int i = 0; i < 5; i++)
        {
            categories[i] = numbers.Where(num => (i * 15 + 1) <= num && num <= (i + 1) * 15).ToList();
        }

        while (categories.Any(category => category.Count > 0))
        {
            for (int i = 0; i < 5; i++)
            {
                if (categories[i].Count > 0)
                {
                    int index = Random.Range(0, categories[i].Count);
                    shuffledNumbers.Add(categories[i][index]);

                    categories[i].RemoveAt(index);
                }
            }
        }

        Debug.Log("Shuffled Numbers: " + string.Join(", ", shuffledNumbers));

        // Определение последней категории числа
        int lastNumber = shuffledNumbers[shuffledNumbers.Count - 1];
        int lastCategory = (lastNumber - 1) / 15;

        int countInCategory = 0;

        // Подсчет чисел в последней категории, начиная с конца списка
        for (int i = shuffledNumbers.Count - 1; i >= 0; i--)
        {
            int currentCategory = (shuffledNumbers[i] - 1) / 15;

            Debug.Log("Number Categories: " + string.Join(", ", currentCategory));

            if (currentCategory == lastCategory)
            {
                countInCategory++;

                // Если найдено более двух чисел в категории, переставляем их
                if (countInCategory > 2)
                {
                    int temp = shuffledNumbers[i];
                    shuffledNumbers.RemoveAt(i);
                    int randomIndex = Random.Range(0, shuffledNumbers.Count / 2);
                    shuffledNumbers.Insert(randomIndex, temp);
                }
            }
            else
                // Если обнаружена другая категория, прекращаем поиск
                break;
        }

        numbers = shuffledNumbers;

        Debug.Log("Shuffled Numbers: " + string.Join(", ", numbers));

        return numbers;
    }

    #endregion
}
