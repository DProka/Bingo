using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public float downTime = 1800f;
    private string pauseStartTime;

    public void Start()
    {
        pauseStartTime = DateTime.Now.ToString();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            SetNewPauseTime();
        }
        else
        {
            Time.timeScale = 1;

            if (pauseStartTime != null)
                CheckPauseTime();
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Time.timeScale = 1;
            SetNewPauseTime();
        }
        else
        {
            Time.timeScale = 0;

            if (pauseStartTime != null)
                CheckPauseTime();
        }
    }

    public void OnApplicationQuit()
    {
        
    }

    private void SetNewPauseTime()
    {
        pauseStartTime = DateTime.Now.ToString();
    }

    private void CheckPauseTime()
    {
        DateTime savedTime = DateTime.Parse(pauseStartTime);
        TimeSpan timeDifference = DateTime.Now - savedTime;

        if (timeDifference.TotalSeconds > downTime)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
