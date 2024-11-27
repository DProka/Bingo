
using UnityEngine;

public class UIAnimationPilePart : MonoBehaviour
{
    public float _pileLifeTime { get; private set; }

    [SerializeField] UIPilePrefab pilePrefab;
    [SerializeField] float pileLifeTime = 2f;

    [SerializeField] Sprite[] spritesArray;

    public void Init()
    {

    }

    public void StartPileAnimation(Type type, Vector3 startPos, Vector3 finishPos)
    {
        Sprite newSprite = spritesArray[0];

        switch (type)
        {
            case Type.Money:
                newSprite = spritesArray[1];
                SoundController.Instance.PlaySound(SoundController.Sound.PileOfMoney);
                break;
        
            case Type.Crystals:
                newSprite = spritesArray[2];
                break;
        }

        UIPilePrefab pile = Instantiate(pilePrefab, transform);
        pile.Init(newSprite, pileLifeTime, startPos, finishPos);
    }

    public enum Type
    {
        Coins,
        Money,
        Crystals
    }
}
