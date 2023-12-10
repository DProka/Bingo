
using UnityEngine;
using TMPro;

public class UITimerScript : UIMenuGeneral
{
    [SerializeField] GameObject timerScreen;
    [SerializeField] TextMeshProUGUI timerText;

    public void SetTimerIsLoading() { timerText.text = "Loading..."; }

    public void SetTimerIsWaiting() { timerText.text = "Waiting For Players..."; }

    public void UpdateStartTimer(int time) { timerText.text = "Game starts in : " + time + "..."; }

    public override void OpenMain()
    {
        base.OpenMain();
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }

}
