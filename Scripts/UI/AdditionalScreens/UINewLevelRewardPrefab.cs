
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINewLevelRewardPrefab : MonoBehaviour
{
    public BoosterManager.Type currentType { get; private set; }

    [SerializeField] Image rewardImage;
    [SerializeField] TextMeshProUGUI[] countTextArray;

    private NewLevelScreenSettings settings;

    public void Init(NewLevelScreenSettings _settings)
    {
        settings = _settings;
    }

    public void SetReward(BoosterManager.Type type, int count)
    {
        currentType = type;
        SetSpriteByType(type);

        foreach(TextMeshProUGUI text in countTextArray)
            text.text = "x" + count;
    }

    private void SetSpriteByType(BoosterManager.Type type)
    {
        Sprite newSprite = settings.airplane;
        Material textMaterial = countTextArray[1].fontMaterial;

        switch (type)
        {
            case BoosterManager.Type.Plus5Balls:
                newSprite = settings.ballsPlus5;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.purple);
                break;
        
            case BoosterManager.Type.Airplane:
                newSprite = settings.airplane;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.purple);
                break;
        
            case BoosterManager.Type.TripleDoub:
                newSprite = settings.trippleDoub;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.orange);
                break;
        
            case BoosterManager.Type.AutoBingo:
                newSprite = settings.autoBingo;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.purple);
                break;
        
            case BoosterManager.Type.DoubleProgress:
                newSprite = settings.doubleProgress;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.orange);
                break;
        
            case BoosterManager.Type.Crystal:
                newSprite = settings.crystal;
                textMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, settings.purple);
                break;
        }

        rewardImage.sprite = newSprite;
        countTextArray[1].fontMaterial = textMaterial;
    }
}
