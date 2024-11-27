
using UnityEngine;

public class UIButtonMain : MonoBehaviour
{
    public virtual void Init() { }

    public void PlayClick1() => SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
    public void PlayClick2() => SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick2);
}
