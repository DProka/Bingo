
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]

public class LocalizedText : MonoBehaviour
{

    private TextMeshProUGUI text;
    private string key;

    private void Start()
    {
        Localize();
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }

    public void GetComponents()
    {
        text = GetComponent<TextMeshProUGUI>();
        key = text.text;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    public void Localize(string newKey = null)
    {
        if (text == null)
            GetComponents();

        if (newKey != null)
            key = newKey;

        text.text = LocalizationManager.GetTranslate(key);
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }
}
