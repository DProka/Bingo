using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppmetricaTimeTracking : MonoBehaviour
{
    public static int minutesInGame = 0;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("Minutes"))
        {
            minutesInGame = PlayerPrefs.GetInt("Minutes");
        }

        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(60f);

        minutesInGame++;

        Debug.Log("AppMetrica timer = " + minutesInGame);

        AppMetrica.reportEvent("playtime", "{\"minute\":\"" + minutesInGame + "\"}");

        PlayerPrefs.SetInt("Minutes", minutesInGame);      

        StartCoroutine(StartTimer());
    }



    public static void reportLevelEnd(long goldEarned, long cashEarned) // типы можешь поменять, я не знаю там какие, 
    {
        int levelCount=0;

        if (PlayerPrefs.HasKey("LevelCount"))
        {
            levelCount = PlayerPrefs.GetInt("LevelCount");
        }

        levelCount++;


        string eventParameters = "{\"count\":\"" + levelCount + "\", \"playtime\":\"" + minutesInGame + "\", \"goldEarned\":\"" + goldEarned + "\", \"cashEarned\":\"" + cashEarned + "\"}";


        AppMetrica.reportEvent("levelEnd", eventParameters);


        PlayerPrefs.SetInt("LevelCount", levelCount);

        Debug.Log($"levelCount: {levelCount}. goldEarned: {goldEarned}. cashEarned: {cashEarned}.");

    }

    public static void reportRoomFinished(string roomTag,long totalGold, long totalCash) // запускать ее когда куплена последняя доступная вещь в комнате, totalGold - сколко было всего зарабонато золота, totalCash сколько было всего заработано денег
    {
       

        int levelCount = 0;

        if (PlayerPrefs.HasKey("LevelCount"))
        {
            levelCount = PlayerPrefs.GetInt("LevelCount");
        }
        // по идее тут у нас будет уже levelCount который записан в преференс сколько уже прошли комнат.



        string eventParameters = "{\"roomName\":\"" + roomTag + "\", \"levelFinished\":\"" + levelCount + "\", \"playtime\":\"" + minutesInGame + "\", \"totalGold\":\"" + totalGold + "\", \"totalCash\":\"" + totalCash + "\"}";

        AppMetrica.reportEvent("roomFinished", eventParameters);


        PlayerPrefs.SetInt("LevelCount", levelCount);

        Debug.Log($"roomName: {roomTag}. levelFinished: {levelCount}. totalGold: {totalGold}. totalCash: {totalCash}.");
    }

}
