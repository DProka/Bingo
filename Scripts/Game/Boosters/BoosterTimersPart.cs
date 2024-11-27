using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterTimersPart
{
    public bool hintActive { get; private set; }
    public bool batteryActive { get; private set; }
    public bool autoBonusActive { get; private set; }
    public bool wildDoubActive { get; private set; }
    public bool doubleXpActive { get; private set; }

    private System.DateTime hintTime;
    private System.DateTime batteryTime;
    private System.DateTime autoBonusTime;
    private System.DateTime wildDoubTime;
    private System.DateTime doubleXpTime;

    private BoosterSave boosterSave;

    public BoosterTimersPart(BoosterSave _boosterSave)
    {
        EventBus.onWindowClosed += CheckTimeBoosters;

        boosterSave = _boosterSave;
        Load();
    }

    public void SwitchDoubleXp(bool isActive) => doubleXpActive = isActive;

    #region Time Boosters

    public void CheckTimeBoosters()
    {
        hintActive = CheckTimerActive(hintTime);
        batteryActive = CheckTimerActive(batteryTime);
        autoBonusActive = CheckTimerActive(autoBonusTime);
        wildDoubActive = CheckTimerActive(wildDoubTime);
        doubleXpActive = CheckTimerActive(doubleXpTime);
    }

    public void SetTimerByType(BoosterManager.Type type, float newTime)
    {
        switch (type)
        {
            case BoosterManager.Type.Hint:
                hintTime = ResetTimer(hintTime, newTime);
                break;

            case BoosterManager.Type.Battery:
                batteryTime = ResetTimer(batteryTime, newTime);
                break;

            case BoosterManager.Type.AutoBonus:
                autoBonusTime = ResetTimer(autoBonusTime, newTime);
                break;

            case BoosterManager.Type.WildDaub:
                wildDoubTime = ResetTimer(wildDoubTime, newTime);
                break;

            case BoosterManager.Type.DoubleXp:
                doubleXpTime = ResetTimer(doubleXpTime, newTime);
                break;
        }

        Save();
    }

    public System.TimeSpan GetTimersByType(BoosterManager.Type type)
    {
        System.DateTime currentTime = hintTime;
        
        switch (type)
        {
            case BoosterManager.Type.Battery:
                currentTime = batteryTime;
                break;
        
            case BoosterManager.Type.AutoBonus:
                currentTime = autoBonusTime;
                break;
        
            case BoosterManager.Type.WildDaub:
                currentTime = wildDoubTime;
                break;
        
            case BoosterManager.Type.DoubleXp:
                currentTime = doubleXpTime;
                break;
        }

        System.TimeSpan timer = currentTime - System.DateTime.Now;
        
        return timer;
    }

    private System.DateTime ResetTimer(System.DateTime timer, double minutes)
    {
        timer = System.DateTime.Now;
        System.DateTime newTimer = timer.AddMinutes(minutes);
        return newTimer;
    }

    private bool CheckTimerActive(System.DateTime endTime)
    {
        bool isActive = false;
        System.TimeSpan remainingTime = endTime - System.DateTime.Now;

        if (remainingTime.Seconds > 0)
            isActive = true;

        return isActive;
    }

    #endregion

    #region Save Load

    private void Save()
    {
        string[] timers = new string[]
        {
            hintTime.ToString(),
            batteryTime.ToString(),
            autoBonusTime.ToString(),
            wildDoubTime.ToString(),
            doubleXpTime.ToString(),
        };

        boosterSave.SaveTimeBoosters(timers);
    }

    private void Load()
    {
        hintTime = System.DateTime.Parse(boosterSave.hintTime);
        batteryTime = System.DateTime.Parse(boosterSave.batteryTime);
        autoBonusTime = System.DateTime.Parse(boosterSave.autoBonusTime);
        wildDoubTime = System.DateTime.Parse(boosterSave.wildDoubTime);
        doubleXpTime = System.DateTime.Parse(boosterSave.doubleXpTime);
    }

    #endregion
}
