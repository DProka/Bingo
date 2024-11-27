using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimeBoosterPrefab : MonoBehaviour
{
    public bool _isTimer => isTimer;
    public BoosterManager.Type boosterType => type;

    [SerializeField] Image backImage;
    [SerializeField] Image boosterImage;
    [SerializeField] TextMeshProUGUI prefabText;

    [SerializeField] BoosterManager.Type type;
    [SerializeField] bool isTimer;

    private TimeBoosterMenuSettings settings;

    private int id;
    
    public void Init(TimeBoosterMenuSettings _settings, int _id)
    {
        settings = _settings;
        id = _id;

        Material textMaterial = prefabText.fontMaterial;
        textMaterial.SetColor(ShaderUtilities.ID_OutlineColor, _isTimer? settings.textColorsArray[1] : settings.textColorsArray[0]);
        prefabText.fontMaterial = textMaterial;

        backImage.sprite = _isTimer? settings.backPrefabArray[1] : settings.backPrefabArray[0];
    }

    public void UpdateText(string text) => prefabText.text = text;

    public void UpdateCount(int _count) => prefabText.text = "" + _count;
}
