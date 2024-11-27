using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    [SerializeField] SoundData soundData;

    [Header("Start Settings")]

    [SerializeField] float musicVolume = 0.8f;
    [SerializeField] float effectsVolume = 1f;

    [Header("Sources")]

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectsSource;
    [SerializeField] AudioSource effectsSource2;

    private bool musicIsOn;
    private bool effectsIsOn;

    public void Init()
    {
        Instance = this;
    }

    #region Switches

    public void SwitchMusic(bool isOn)
    {
        musicIsOn = isOn;
        musicSource.volume = isOn ? musicVolume : 0f;
    }

    public void SwitchEffects(bool isOn)
    {
        effectsIsOn = isOn;
        effectsSource.volume = isOn ? effectsVolume : 0f;
    }

    #endregion

    public void SwitchMainMenuMusic(bool isOn)
    {
        if (isOn && !musicSource.isPlaying)
            musicSource.Play();

        if (!isOn && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PlayStart() { effectsSource.PlayOneShot(soundData.startSound); }

    public void PlayNumber(int num) { effectsSource.PlayOneShot(soundData.numbersArray[num - 1]); }

    public void PlayBeforeTimer(int num) { effectsSource.PlayOneShot(soundData.beforeTimerSounds[num - 1]); }

    public void PlayBidSelect(int num) { effectsSource.PlayOneShot(soundData.bidSounds[num]); }

    public void PlayBingoSound(int num) { effectsSource.PlayOneShot(soundData.bingoSounds[num - 1]); }

    public void PlayJackPotSound() { effectsSource.PlayOneShot(soundData.jackPotSound); }

    public IEnumerator StartTimerAlarm()
    {
        effectsSource.PlayOneShot(soundData.timerSound);

        yield return new WaitForSeconds(5f);

        effectsSource.Stop();
        effectsSource.PlayOneShot(soundData.timerEnd);
    }

    public void PlaySound(Sound sound)
    {
        if (effectsIsOn)
        {
            effectsSource.volume = effectsVolume;

            switch (sound)
            {
                case Sound.OpenChip:
                    effectsSource.PlayOneShot(soundData.chipOpen);
                    break;

                case Sound.OpenChest:
                    effectsSource.PlayOneShot(soundData.chestOpen);
                    break;

                case Sound.SetChest:
                    effectsSource.PlayOneShot(soundData.chestSet);
                    break;

                case Sound.BonusReady:
                    effectsSource.PlayOneShot(soundData.bonusReady);
                    break;

                case Sound.BonusUse:
                    effectsSource.PlayOneShot(soundData.bonusUsage);
                    break;

                case Sound.RoundIsOver:
                    effectsSource2.PlayOneShot(soundData.roundOverSound);
                    break;

                case Sound.RewardScreen:
                    effectsSource.PlayOneShot(soundData.rewardSound);
                    break;

                case Sound.PileOfMoney:
                    effectsSource.PlayOneShot(soundData.moneyPileSound);
                    break;

                case Sound.ButtonClick1:
                    effectsSource.PlayOneShot(soundData.buttonClick1);
                    break;

                case Sound.ButtonClick2:
                    effectsSource.PlayOneShot(soundData.buttonClick2);
                    break;

                case Sound.PuzzlePart:
                    effectsSource.volume = 0.3f;
                    effectsSource.PlayOneShot(soundData.puzzlePart);
                    break;

                case Sound.PuzzleFull:
                    effectsSource.volume = 0.7f;
                    effectsSource.PlayOneShot(soundData.puzzleFull);
                    break;

                case Sound.GachaStart:
                    effectsSource.PlayOneShot(soundData.gachaStart);
                    break;

                case Sound.GachaMix:
                    effectsSource.PlayOneShot(soundData.gachaMix);
                    break;

                case Sound.CapsuleAppearing:
                    effectsSource.PlayOneShot(soundData.capsuleAppearing);
                    break;
            }
        }
    }

    public enum Sound
    {
        OpenChip,
        OpenChest,
        SetChest,
        BonusReady,
        BonusUse,

        RoundIsOver,
        RewardScreen,

        PileOfMoney,

        ButtonClick1,
        ButtonClick2,

        PuzzlePart,
        PuzzleFull,

        GachaStart,
        GachaMix,
        CapsuleAppearing,
    }
}
