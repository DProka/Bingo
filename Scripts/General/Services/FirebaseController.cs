
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using Firebase.Extensions;

public class FirebaseController
{
    public static FirebaseController Instance;

    //private FirebaseApp app;

    public int interstitialStartLevel { get; private set; }
    public int[][] roomsPricesArray { get; private set; }

    public FirebaseController()
    {
        Instance = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //app = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                FetchRemoteConfig();
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        //Debug.Log(FirebaseAnalytics.EventAppOpen);
    }

    #region TrackData

    public static void trackImpressionData(MaxSdkBase.AdInfo adInfo, string[] otherParams)
    {

        double revenue = adInfo.Revenue;
        var impressionParameters = new[] {
          new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
          new Firebase.Analytics.Parameter("ad_source", adInfo.NetworkName),
          new Firebase.Analytics.Parameter("ad_unit_name", otherParams[1]),
          new Firebase.Analytics.Parameter("ad_format", otherParams[2]),
          new Firebase.Analytics.Parameter("value", revenue),
          new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        };

        FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
    }

    #endregion

    #region RemoteConfig

    private void FetchRemoteConfig()
    {
        // Определение значений по умолчанию для Remote Config
        var defaults = new Dictionary<string, object>();
        defaults.Add("interstitialStartLevel", 7);
        defaults.Add("room1_prices", "{\"tier1\":50, \"tier2\":100, \"tier3\":100, \"tier4\":120, \"tier5\":140, \"tier6\":160, \"tier7\":180, \"tier8\":200, \"tier9\":220, \"tier10\":240, \"tier11\":260, \"tier12\":280, \"tier13\":300, \"tier14\":320, \"tier15\":340,}");
        defaults.Add("room2_prices", "{\"tier1\":80, \"tier2\":120, \"tier3\":160, \"tier4\":200, \"tier5\":220, \"tier6\":240, \"tier7\":260, \"tier8\":280, \"tier9\":300, \"tier10\":320, \"tier11\":340, \"tier12\":360, \"tier13\":380, \"tier14\":400, \"tier15\":420,}");

        // Установка значений по умолчанию
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Значения по умолчанию установлены.");
            FetchDataAsync();
        });
    }

    // Фетч данных с Firebase Remote Config
    private Task FetchDataAsync()
    {
        // Задаем минимальный интервал для обновления
        var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Remote Config фетч успешен");
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(activateTask =>
                    {
                        Debug.Log("Remote Config данные активированы.");

                        string valuesString = FirebaseRemoteConfig.DefaultInstance.GetValue("interstitialStartLevel").StringValue;
                        interstitialStartLevel = ConvertStringToInt(valuesString);
                        //Debug.Log("Received InterstitialValue: " + valuesString + " Parsed Value: " + interstitialStartLevel);

                        string room1Prices = FirebaseRemoteConfig.DefaultInstance.GetValue("room1_prices").StringValue;
                        string room2Prices = FirebaseRemoteConfig.DefaultInstance.GetValue("room2_prices").StringValue;
                        RoomItemPricesCollection room1PricesCollection = RoomItemPricesCollection.CreateFromJSON(room1Prices);
                        RoomItemPricesCollection room2PricesCollection = RoomItemPricesCollection.CreateFromJSON(room2Prices);
                        roomsPricesArray = new int[2][];
                        roomsPricesArray[0] = room1PricesCollection.GetPricesArray();
                        roomsPricesArray[1] = room2PricesCollection.GetPricesArray();
                        //Debug.Log("Received JSON: " + room1Prices);
                        //Debug.Log("Received JSON: " + room2Prices);
                        //Debug.Log("room1 prices: " + string.Join(", ", roomsPricesArray[0]));
                        //Debug.Log("room2 prices: " + string.Join(", ", roomsPricesArray[1]));

                        GameController.Instance.SetFirebaseSettings();
                    });
            }
            else
            {
                Debug.LogError("Фетч данных с Remote Config не удался");
            }
        });
    }
    #endregion

    #region Values

    private int ConvertStringToInt(string valueString)
    {
        int newValue = int.Parse(valueString);

        return newValue;
    }

    #endregion

    [System.Serializable]
    public class RoomItemPricesCollection
    {
        public int tier1;
        public int tier2;
        public int tier3;
        public int tier4;
        public int tier5;
        public int tier6;
        public int tier7;
        public int tier8;
        public int tier9;
        public int tier10;
        public int tier11;
        public int tier12;
        public int tier13;
        public int tier14;
        public int tier15;

        public static RoomItemPricesCollection CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<RoomItemPricesCollection>(jsonString);
        }

        public int[] GetPricesArray()
        {
            return new int[] { tier1, tier2, tier3, tier4, tier5, tier6, tier7, tier8, tier9, tier10, tier11, tier12, tier13, tier14, tier15, };
        }
    }

    //{"tier1":50, "tier2":100, "tier3":100, "tier4":120, "tier5":140, "tier6":160, "tier7":180, "tier8":200, "tier9":220, "tier10":240, "tier11":260, "tier12":280, "tier13":300, "tier14":320, "tier15":340}
    //{"tier1":80, "tier2":120, "tier3":160, "tier4":200, "tier5":220, "tier6":240, "tier7":260, "tier8":280, "tier9":300, "tier10":320, "tier11":340, "tier12":360, "tier13":380, "tier14":400, "tier15":420}

}
