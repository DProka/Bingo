
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NoMoneyScreen : UIMenuGeneral
{
    [SerializeField] Image girlImage;

    public override void OpenMain()
    {
        girlImage.color = new Color(255, 255, 255, 0);

        base.OpenMain();
        EventBus.onWindowOpened?.Invoke();

        girlImage.DOFade(1, 0.3f);
    }

    public override void CloseMain()
    {
        base.CloseMain();
        EventBus.onWindowClosed?.Invoke();
    }

    public void CallJoinGame() => UIController.Instance.CallScreen(UIController.Menu.JoinGameMenu);
}
