using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class MaxSdkManager : MonoBehaviour
{
    public static MaxSdkManager Instance;
    public static bool adIsActive { get; private set; }

    [SerializeField] string sdkKey;
    [SerializeField] string adInterstitialId = "";
    [SerializeField] string adRewardedId = "";
    [SerializeField] TextMeshProUGUI debugVar;

    private int retryAttemptInt;
    private int retryAttemptRew;

    private string country;
    private string excludedCountries = "";//"US,UK";

    private string adLocation;
    private float lastRewardTime;

    private void Awake()
    {
        Instance = this;
        adIsActive = true;
        StartCoroutine(InitializeADS());
    }

    private IEnumerator InitializeADS()
    {
        yield return new WaitForSeconds(1f);

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            if (checkCountry())
            {
                InitializeInterstitialAds();
                InitializeRewardedAds();
            }
        };

        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();

    }

    private bool checkCountry()
    {
        if (country == null)
        {
            country = MaxSdk.GetSdkConfiguration().CountryCode;

            if (excludedCountries.Contains(country))
            {
                adIsActive = false;
                return false;

            }

            string eventParameters = "{\"country\":\"" + country + "\", \"AdIsActive\":\"" + adIsActive + "\"}";
            AppMetrica.reportEvent("county_apploving", eventParameters);

        }

        return true;
    }

    #region Interstitial

    private void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adInterstitialId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttemptInt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttemptInt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppMetrica.reportADEvent(adInfo, adLocation, "room" + GameController.Instance. GetCurrentRoomNum(), "Interstitial", country);
        FirebaseController.trackImpressionData(adInfo, new string[] { country, adUnitId, adLocation, "Interstitial", "room" + GameController.Instance.GetCurrentRoomNum() });
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
        EventBus.onRewardRecieved?.Invoke();
    }

    public void ShowInterstitial(string location)
    {
        if (adIsActive)
            {
                adLocation = location;

                checkCountry(); // на всякий случай поставим вдруг он после инициализации не успевает иногда получить страну, там в функции есть проверка чтобы он страну один раз только получал.


                if (Time.time - lastRewardTime > 30f) // только если больше 30 секунд прошло с последнего реварда
                {
                    if (MaxSdk.IsInterstitialReady(adInterstitialId))
                    {
                        MaxSdk.ShowInterstitial(adInterstitialId);
                        return;
                    }
                    else
                    {
                        LoadInterstitial();

                    }
                }
            }
        

        // это по идее выполниться всегда кроме случая когда интер вызвали.
        EventBus.onRewardRecieved?.Invoke(); 
    }

    #endregion

    #region Rewarded

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adRewardedId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttemptRew = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttemptRew++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptRew));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        EventBus.onRewardedADClosed?.Invoke(adLocation);
        LoadRewardedAd();
        lastRewardTime = Time.time;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppMetrica.reportADEvent(adInfo, adLocation, "room" + GameController.Instance.GetCurrentRoomNum(), "Rewarded", country);
        FirebaseController.trackImpressionData(adInfo, new string[] { country, adUnitId, adLocation, "Rewarded" });
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        //тут вообще ничего не делаем, уже вызвали все в hidden
    }

    public void ShowRewarded(string location) // ревардед не проверяем на adIsActive, реварды всегда активны во всех странах, эт касается только inter and banner
    { // реварды вроде мы всегда должны показывать
            adLocation = location;

            if (MaxSdk.IsRewardedAdReady(adRewardedId))
            {
                EventBus.onWindowOpened?.Invoke();
                MaxSdk.ShowRewardedAd(adRewardedId);
            }
            else
            {
                LoadRewardedAd();
            }
    }

    #endregion

    /*
    #region Geolocation

    private IEnumerator GetCountryCode()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://ipinfo.io/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка при получении геолокации: " + request.error);
        }
        else
        {
            string jsonResult = request.downloadHandler.text;
            var jsonData = JsonUtility.FromJson<IPInfo>(jsonResult);
            country = jsonData.country;
            Debug.Log("Your country: " + country);
        }
    }

    [System.Serializable]
    public class IPInfo
    {
        public string country;
    }
    #endregion

    */
}
