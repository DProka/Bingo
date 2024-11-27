
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LocalizationManager : MonoBehaviour
{
    public static int selectedLanguage;

    public delegate void LanguageChangeHandler();
    public static event LanguageChangeHandler OnLanguageChange;
    
    private static Dictionary<string, List<string>> localization;

    [SerializeField] TextAsset textFile;

    public void Init()
    {
        if (localization == null)
            LoadLocalization();
    }

    public void SetLanguage(int id)
    {
        selectedLanguage = id;
        OnLanguageChange?.Invoke();
    }

    public void LoadLocalization()
    {
        localization = new Dictionary<string, List<string>>();

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textFile.text);
        
        foreach(XmlNode key in xmlDocument["Keys"].ChildNodes)
        {
            string keyStr = key.Attributes["Name"].Value;

            List<string> values = new List<string>();
            foreach (XmlNode translate in key["Translates"].ChildNodes)
                values.Add(translate.InnerText);

            localization[keyStr] = values;
        }
    }

    public static string GetTranslate(string key, int languageId = -1)
    {
        if (languageId == -1)
            languageId = selectedLanguage;

        if (localization.ContainsKey(key))
            return localization[key][languageId];

        return key;
    }
}
