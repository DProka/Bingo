
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimationScreen : MonoBehaviour
{
    public static UIAnimationScreen Instance;

    public bool animationIsActive { get; private set; }

    [SerializeField] Transform mainMenuCoins;
    [SerializeField] Transform mainMenuMoney;
    [SerializeField] Transform mainMenuCrystals;

    [Header("Bingo Animation")]

    [SerializeField] BingoAnimationPrefab bingoAnimPrefab;

    [Header("Reward Animation")]

    [SerializeField] UIAnimationPilePart pilePart;

    [Header("In Game Animations")]

    [SerializeField] BonusTextPrefabScript bonusTextPrefab;
    [SerializeField] ParticleSystem moneyParticles;
    [SerializeField] ParticleSystem coinParticles;

    [Header("Puzzle")]

    [SerializeField] Image puzzleRewardImage;
    [SerializeField] Transform puzzleMenuFinish;

    [Header("Item")]

    [SerializeField] Transform puzzleFinish;
    [SerializeField] UIItemPrefab itemPrefab;
    [SerializeField] Sprite[] itemsSpritesArray;

    [Header("Booster")]

    [SerializeField] Transform boosterPos;
    [SerializeField] Image enhancerImage;
    [SerializeField] Transform enhancerStart;
    [SerializeField] Transform enhancerFinish;

    [Header("Comet")]

    [SerializeField] Transform startCometTransform;

    private AnimationScreenSettings settings;

    private Vector2 puzzleRewardPosition;
    private float animationTimer;

    public void Init(AnimationScreenSettings _settings)
    {
        Instance = this;
        settings = _settings;

        EventBus.onRewardRecieved += StartEndRoundReward;
        EventBus.onPuzzleOpened += StartPuzzleTableAnimation;

        pilePart.Init();

        puzzleRewardImage.enabled = false;
        puzzleRewardPosition = puzzleRewardImage.transform.position;

        enhancerImage.enabled = false;
    }

    private void Update()
    {
        if (animationIsActive)
        {
            animationTimer -= Time.deltaTime;

            if (animationTimer <= 0)
                animationIsActive = false;
        }
    }

    #region Pile Part

    public void StartEndRoundReward() => Invoke("StartRewardAnimWithDelay", 0.2f);

    public void StartBuyObjAnimation(Vector3 finish) => StartPileAnimation(UIAnimationPilePart.Type.Money, finish, mainMenuMoney.position);
    
    public void StartBecomeCoinsAnimation() => StartPileAnimation(UIAnimationPilePart.Type.Coins, mainMenuCoins.position, new Vector3(0, 0, 0));
    
    public void StartPileAnimation(UIAnimationPilePart.Type type, Vector3 finish, Vector3 start)
    {
        pilePart.StartPileAnimation(type, start, finish);
        SetAnimationTimer(pilePart._pileLifeTime);
    }

    private void SetAnimationTimer(float timer)
    {
        animationIsActive = true;
        animationTimer = timer;
    }

    #endregion

    #region TablePart

    public void CallBingoAnimation(Transform parent, int animNum, int[] currency, bool isJackPot)
    {
        BingoAnimationPrefab newAnim = Instantiate(bingoAnimPrefab, parent.position, Quaternion.identity, parent);
        newAnim.Init(this, animNum, currency, isJackPot);
    }

    public void StartPuzzleRewardAnimation(bool isFirstAnim)
    {
        Invoke("StartPuzzleRewardAnimation2", 0.2f);
    }

    private void StartPuzzleRewardAnimation2()
    {
        puzzleRewardImage.transform.position = puzzleRewardPosition;
        puzzleRewardImage.transform.DOScale(1f, 0);
        puzzleRewardImage.DOFade(1f, 0f);
        puzzleRewardImage.enabled = true;

        Sequence puzzleSequence = DOTween.Sequence()
            .Append(puzzleRewardImage.transform.DOScale(0.3f, settings.puzzleRewardAnimTime).SetEase(Ease.OutBack))
            .Join(puzzleRewardImage.transform.DOMove(puzzleMenuFinish.position, settings.puzzleRewardAnimTime))
            .Join(puzzleRewardImage.DOFade(0f, settings.puzzleRewardAnimTime / 3).SetDelay(settings.puzzleRewardAnimTime / 2).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                puzzleRewardImage.enabled = false;
            });
    }

    public void StartCometAnimation(Vector3 finishPos)
    {
        CometScript newComet = Instantiate(settings.cometPrefab, transform);
        newComet.Init(startCometTransform.position, finishPos, settings.cometAnimationTime);
    }

    public void GetCoinBonus(int bonus) => StartBonusTextAnimation(Bonus.Coin, bonus, coinParticles.transform.position);

    public void GetChestBonus(Vector3 chestPos, int moneyBonus, int xpBonus)
    {
        StartBonusTextAnimation(Bonus.Chest, moneyBonus, new Vector3(chestPos.x, chestPos.y + 0.4f));
        StartBonusTextAnimation(Bonus.XP, xpBonus, chestPos);
    }

    public void GetXPBonus(Vector3 chipPos, int bonus) => StartBonusTextAnimation(Bonus.XP, bonus, chipPos);

    #endregion

    #region BonusTextPart

    public void StartBonusItemAnimation(int bonusCount, Vector3 pos)
    {
        BonusTextPrefabScript newBonus = Instantiate(bonusTextPrefab, transform);
        newBonus.transform.position = new Vector3(pos.x, pos.y, 0);
        string bonusText = "    +" + bonusCount;
        Color[] colorsArray = settings.bonusCrystalsTextColorsArray;
        newBonus.Init(colorsArray, bonusText, true);
    }

    private void StartBonusTextAnimation(Bonus bonus, int bonusCount, Vector3 pos)
    {
        BonusTextPrefabScript newBonus = Instantiate(bonusTextPrefab, transform);
        newBonus.transform.position = new Vector3(pos.x, pos.y, 0);
        string bonusText = "+" + bonusCount;
        Color[] colorsArray = settings.bonusCoinsTextColorsArray;

        switch (bonus)
        {
            case Bonus.Coin:
                coinParticles.Play();
                break;

            case Bonus.Chest:
                moneyParticles.transform.position = new Vector3(pos.x, pos.y, 0);
                moneyParticles.Play();
                colorsArray = settings.bonusMoneyTextColorsArray;
                break;

            case Bonus.XP:
                colorsArray = settings.bonusXPTextColorsArray;
                bonusText = "+" + bonusCount + "XP";
                break;
        }

        newBonus.Init(colorsArray, bonusText, false);
    }

    private enum Bonus
    {
        Coin,
        Chest,
        XP
    }

    #endregion

    #region ItemPart

    public void StartPuzzleTableAnimation() => StartItemAnimation(ItemType.Puzzle);
    
    public void StartItemAnimation(ItemType type)
    {
        UIItemPrefab newItem = Instantiate(itemPrefab, transform);

        switch (type)
        {
            case ItemType.Puzzle:
                newItem.Init(itemsSpritesArray[0], 1f, boosterPos.position, puzzleFinish.position);
                break;
        }
    }

    public enum ItemType
    {
        Puzzle
    }

    #endregion

    #region Enhancer

    public void StartDoubleProgressAnimation() => StartTableEnhancerAnimation();
    
    private void StartTableEnhancerAnimation()
    {
        enhancerImage.transform.position = enhancerStart.position;
        enhancerImage.transform.DOScale(1f, 0);
        enhancerImage.DOFade(1f, 0f);
        enhancerImage.enabled = true;

        enhancerImage.transform.DOMove(enhancerFinish.position, 1f).OnComplete(() =>
        {
            enhancerImage.DOFade(0f, 0.3f).OnComplete(() => enhancerImage.enabled = false);
        });
    }

    #endregion

    private void OnDestroy()
    {
        EventBus.onRewardRecieved -= StartEndRoundReward;
        EventBus.onPuzzleOpened -= StartPuzzleTableAnimation;
    }
}
