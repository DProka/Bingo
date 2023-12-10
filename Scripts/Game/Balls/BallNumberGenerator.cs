
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallNumberGenerator : MonoBehaviour
{
    public List<int> numbers;

    public List<int> GetNumbersList()
    {
        return numbers;
    }
    
    public void SetNewNumbersArray(int[] numbersArray)
    {
        numbers = new List<int>();

        int[] newNumbers = FillEmptyNumbers(numbersArray);

        for (int i = 0; i < newNumbers.Length; i++)
        {
            numbers.Add(newNumbers[i]);
        }

        //MixNumbersList();
        ShuffleNumbers();
    }

    private int[] FillEmptyNumbers(int[] array)
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
            if(fullArray[i] == 0)
            {
                int randomIndex = Random.Range(0, numbersList.Count);
                fullArray[i] = numbersList[randomIndex];
                numbersList.Remove(fullArray[i]);
            }
        }

        return fullArray;
    }

    private void MixNumbersList()
    {
        List<int> numbersList = numbers;

        for (int i = numbersList.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            int tmp = numbersList[j];
            numbersList[j] = numbersList[i];
            numbersList[i] = tmp;
        }

        numbers = numbersList;
    }

    private void ShuffleNumbers()
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

        numbers = shuffledNumbers;
    }
}
