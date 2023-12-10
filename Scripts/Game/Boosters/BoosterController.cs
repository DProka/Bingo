using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoosterController : MonoBehaviour
{
    private TableController tableController;

    [Header("Main Settings")]

    [SerializeField] Image boosterImage;
    
    private int progress;
    private bool isActive;

    [Header("Progress")]

    [SerializeField] float progressAnimDuration = 0.4f;
    [SerializeField] Sprite[] progressSpriteArray;

    [Header("Boosters")]

    [SerializeField] float activeAnimDuration = 0.3f;
    [SerializeField] Sprite[] boostersSpriteArray;

    private int nextBoosterNum;
    private bool isEnhancerActive;

    private int tutorialprogress;

    [Header("Animations")]

    [SerializeField] Transform cometStartPos;
    [SerializeField] Transform cometParent;
    [SerializeField] CometScript cometPrefab;
    [SerializeField] ParticleSystem coinParticles;

    public void Init(TableController table)
    {
        tableController = table;

        nextBoosterNum = 0;
        isEnhancerActive = false;
    }

    public void ResetBooster()
    {
        tutorialprogress = 0;
        nextBoosterNum = 0;
        isEnhancerActive = false;
        ResetProgressBar();
    }

    #region Progress Bar

    public void UpdateProgress()
    {
        if (!isActive)
        {
            if (progress < 4)
            {
                if (!isEnhancerActive)
                    progress++;
                else
                    progress += 2;

                UpdateProgressSprite();
                isActive = false;
            }

            if (progress >= 4)
            {
                if (!GameController.tutorialIsActive)
                {   
                    RandomizeType();
                }
                else
                {
                    SetTutorialBooster();
                }

                progress = 4;
                isActive = true;
                SetBoosterSprite();
            }
        }
    }

    public void ResetProgressBar()
    {
        progress = 0;

        boosterImage.transform.DOScale(1f, 0f);
        UpdateProgressSprite();
    }

    private void UpdateProgressSprite() 
    {
        boosterImage.sprite = progressSpriteArray[progress];
        boosterImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), progressAnimDuration, 0);
    }

    private void SetBoosterSprite()
    {
        boosterImage.sprite = boostersSpriteArray[nextBoosterNum];
        StartCoroutine(StartBoosterAnim());
    }
    #endregion

    #region Booster Activation
    
    private void RandomizeType()
    {
        int type = 0;

        if (!isEnhancerActive)
            type = Random.Range(1, 6);
        else
            type = Random.Range(1, 5);


        if (type == 5 && isEnhancerActive)
            RandomizeType();

        nextBoosterNum = type;
    }

    public void ActivateBooster()
    {
        if (isActive)
        {
            isActive = false;
            ResetProgressBar();

            if (!GameController.tutorialIsActive)
            {
                GetBoosterBonus();
            }
            else
            {
                ActivateTutorialBooster();
            }
        }
    }

    private void GetBoosterBonus()
    {
        if(nextBoosterNum == 1)
        {
            GetX1Bonus();
        }
        
        if(nextBoosterNum == 2)
        {
            GetX2Bonus();
        }
        
        if(nextBoosterNum == 3)
        {
            GetCoinBonus();
        }
        
        if(nextBoosterNum == 4)
        {
            AddBonusChest();
        }
        
        if(nextBoosterNum == 5)
        {
            GetEnhancerBonus();
        }
    }

    private void GetX1Bonus()
    {
        tableController.CloseRandomChip();
    }

    private void GetX2Bonus()
    {
        tableController.CloseRandomChip();
        tableController.CloseRandomChip();
    }

    private void GetCoinBonus() 
    { 
        tableController.GetCoinBonus();
        coinParticles.Play();
    }

    private void AddBonusChest() 
    {
        tableController.AddRandomBonusChest();
    }
    
    private void GetEnhancerBonus()
    {
        isEnhancerActive = true;
    }
    #endregion

    #region Animation

    private IEnumerator StartBoosterAnim()
    {
        if (!isActive)
            yield break;

        else
        {
            boosterImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), activeAnimDuration, 0);

            yield return new WaitForSeconds(activeAnimDuration + 0.5f);

            StartCoroutine(StartBoosterAnim());
        }
    }

    public void StartCometAnimation(Vector3 finishPos)
    {
        CometScript newComet = Instantiate(cometPrefab, cometParent);
        StartCoroutine(newComet.Init(cometStartPos.position, finishPos));
    }

    #endregion

    #region Tutorial

    public void ActivateTutorialBooster()
    {
        if (tutorialprogress == 0)
        {
            tableController.CloseTutorialChip(true, 18);
            GameController.tutorialManager.UpdateTutorialProgress(8);
            tutorialprogress++;
        }
        else if (tutorialprogress == 1)
        {
            GetBoosterBonus();
            tutorialprogress++;
        }
    }

    private void SetTutorialBooster()
    {
        if(tutorialprogress == 0)
        {
            nextBoosterNum = 1;
            StartCoroutine(GameController.tutorialManager.CallStep7());
        }
        else if(tutorialprogress == 1)
        {
            nextBoosterNum = 3;
        }
    }

    #endregion
}
