
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TableCardsPart : MonoBehaviour
{
    public List<CardController> cardsList { get; private set; }
    public int tableCount { get; private set; }

    [SerializeField] Transform[] cardsParentArray;

    private TableController controller;
    private TableSettings settings;

    public void Init(TableController _controller, TableSettings _settings)
    {
        controller = _controller;
        settings = _settings;

        EventBus.onRoundEnded += ResetPlace;
    }

    public void UpdateCards()
    {
        if (cardsList != null)
        {
            foreach (CardController card in cardsList)
            {
                card.UpdateCard();
            }
        }
    }

    public void CheckBallNumber(int ballNum)
    {
        for (int i = 0; i < cardsList.Count; i++)
        {
            cardsList[i].CheckBallNumber(ballNum);
        }
    }

    public void GetCardsOnTableList(int count)
    {
        if (tableCount != count)
        {
            tableCount = count;

            foreach (Transform obj in cardsParentArray)
            {
                obj.gameObject.SetActive(false);
            }

            cardsList = new List<CardController>();

            for (int i = 0; i < cardsParentArray[count - 1].childCount; i++)
            {
                cardsList.Add(cardsParentArray[count - 1].GetChild(i).GetComponent<CardController>());
            }

            cardsParentArray[count - 1].gameObject.SetActive(true);
        }

        ResetPlace();
    }

    public void CloseRandomChip(int chipCount)
    {
        float delay = 0f;

        for (int j = 0; j < chipCount; j++)
        {
            for (int i = 0; i < cardsList.Count; i++)
            {
                StartCoroutine(cardsList[i].CloseRandomChip(delay));
                delay += 0.1f;
            }
        }

        if(chipCount == 3)
            Debug.Log("TripplaBoosterIsUsedOnTable");
    }

    public void CloseRandomChipWithoutAnim(int chipCount, float animDelay)
    {
        for (int j = 0; j < chipCount; j++)
        {
            for (int i = 0; i < cardsList.Count; i++)
                StartCoroutine(cardsList[i].CloseRandomChipWithoutAnim(animDelay));
        }
    }

    public void AddRandomBonusChest()
    {
        float delay = 0;

        foreach (CardController card in cardsList)
        {
            StartCoroutine(card.AddRandomBonusChest(delay));
            delay += 0.1f;
        }
    }

    public void CheckBoostersOnStart()
    {
        if (GameController.Instance.boosterManager.autobingoIsActive)
        {
            int random = Random.Range(0, cardsList.Count);

            StartCoroutine(cardsList[random].AddRandomAutoBingo(0.3f));

            Debug.Log("Autobingo CardNum = " + random);
        }
    
        if (GameController.Instance.boosterManager.tripleDoubIsActive)
        {
            CloseRandomChip(3);

            Debug.Log("TrippleDoub is Used");
        }
    }

    public void SetChestSprite(Sprite usedChest)
    {
        foreach (CardController card in cardsList)
            card.SetChestSprite(usedChest);
    }

    #region RoundOver Animation

    public void SwitchRoundOverCardsPlace()
    {
        switch (tableCount)
        {
            case 2:
                cardsList[0].transform.DOScale(0.7f, 0.5f);
                cardsList[1].transform.DOScale(0.7f, 0.5f);

                cardsList[0].transform.DOMove(new Vector2(-6f, 0), 0.5f);
                cardsList[1].transform.DOMove(new Vector2(-1.5f, 0), 0.5f);
                break;
        
            case 4:
                cardsList[0].transform.DOScale(0.8f, 0.5f);
                cardsList[1].transform.DOScale(0.8f, 0.5f);
                cardsList[2].transform.DOScale(0.8f, 0.5f);
                cardsList[3].transform.DOScale(0.8f, 0.5f);

                cardsList[0].transform.DOMove(new Vector2(-6f, 1.4f), 0.5f);
                cardsList[1].transform.DOMove(new Vector2(-6f, -2.8f), 0.5f);

                cardsList[2].transform.DOMove(new Vector2(-1.5f, 1.4f), 0.5f);
                cardsList[3].transform.DOMove(new Vector2(-1.5f, -2.8f), 0.5f);
                break;
        }
    }

    private void ResetPlace()
    {
        switch (tableCount)
        {
            case 2:
                cardsList[0].transform.DOScale(1, 0);
                cardsList[1].transform.DOScale(1, 0);

                cardsList[0].transform.DOMove(new Vector2(-3, -1f), 0);
                cardsList[1].transform.DOMove(new Vector2(3, -1f), 0);
                break;
                
            case 4:
                cardsList[0].transform.DOScale(0.9f, 0);
                cardsList[1].transform.DOScale(0.9f, 0);
                cardsList[2].transform.DOScale(0.9f, 0);
                cardsList[3].transform.DOScale(0.9f, 0);

                cardsList[0].transform.DOMove(new Vector2(-3.7f, 2.4f), 0.5f);
                cardsList[1].transform.DOMove(new Vector2(-3.7f, -2.5f), 0);

                cardsList[2].transform.DOMove(new Vector2(2.55f, 2.4f), 0.5f);
                cardsList[3].transform.DOMove(new Vector2(2.55f, -2.5f), 0.5f);
                break;
        }
    }

    #endregion

    #region Preparing

    public void InitializeCards(int jackpotLvl)
    {
        for (int i = 0; i < cardsList.Count; i++)
        {
            cardsList[i].Init(controller, settings, i, jackpotLvl);
        }
    }

    public void GenerateCards()
    {
        foreach (CardController card in cardsList)
        {
            card.GenerateCard();
        }
    }

    public int[] RandomizeBingosInGame(int bingosInRound, int jackpotCount, int ballsCount)
    {
        //int[] newArray = new int[25];
        int[] newArray = new int[ballsCount];
        int[] cardNumbers = TableCalculations.RandomizeBingoNumbers();

        List<int[]> bingoStrokesList = TableCalculations.GetBingoStrokeList(jackpotCount);

        if (jackpotCount == 0)
        {
            for (int i = 0; i < bingosInRound; i++)
            {
                int card = Random.Range(0, cardsList.Count);
                int strokeNum = Random.Range(0, bingoStrokesList.Count);
                int[] bingoStroke = bingoStrokesList[strokeNum];
                bingoStrokesList.Remove(bingoStroke);

                for (int j = 0; j < bingoStroke.Length; j++)
                {
                    if (newArray[bingoStroke[j]] == 0)
                        newArray[bingoStroke[j]] = cardNumbers[bingoStroke[j]];

                    cardsList[card].SetBingoNum(bingoStroke[j], cardNumbers[bingoStroke[j]]);
                }
            }

            JackpotDisplay.Instance.SetCardNum(Random.Range(0, cardsList.Count));
        }
        else
        {
            int card = Random.Range(0, cardsList.Count);
            int[] condition = new int[] { 0, 1, 2, 3, 4, 6, 12, 16, 20, 21, 22, 23, 24 };

            for (int i = 0; i < condition.Length; i++)
            {
                if (newArray[condition[i]] == 0)
                    newArray[condition[i]] = cardNumbers[condition[i]];

                cardsList[card].SetBingoNum(condition[i], cardNumbers[condition[i]]);
            }

            JackpotDisplay.Instance.SetCardNum(card);
        }

        return newArray;
    }
    #endregion

    #region Tutorial

    public void CloseTutorialChip(bool isBooster, int chipNum) => StartCoroutine(cardsList[0].CloseTutorialChip(isBooster, chipNum));

    public void PrepareTutorialCard()
    {
        cardsList[0].Init(controller, settings, 0, 1);
        cardsList[0].GenerateTutorialCard();
    }

    #endregion

    private void OnDestroy()
    {
        EventBus.onRoundEnded -= ResetPlace;
    }
}
