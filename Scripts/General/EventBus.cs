
using UnityEngine;
using System;

public static class EventBus
{
    #region Game Region

    public static Action<int> onRoundStarted;
    public static Action onBallsIsEmpty;
    public static Action onRoundEnded;
    public static Action onRewardRecieved;
    public static Action onBoosterTimerEnded;

    #endregion

    #region Table Region

    public static Action<int, bool> onChipClosed;
    public static Action onPuzzleOpened;

    #endregion

    #region Menu Region

    public static Action onWindowOpened;
    public static Action onWindowClosed;

    #endregion

    #region AD Region

    public static Action<string> onRewardedADClosed;

    #endregion
}
