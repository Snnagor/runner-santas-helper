using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [Header("AudioClips")]
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip takeCoin;
    [SerializeField] private AudioClip takeGift;
    [SerializeField] private AudioClip gift3Line;
    [SerializeField] private AudioClip slideStep;
    [SerializeField] private AudioClip hitBlock;
    [SerializeField] private AudioClip hitBlockGifts;
    [SerializeField] private AudioClip hitSnowman;
    [SerializeField] private AudioClip upgradeBonus;
    //[Space]
    //[Header("AudioSource")]
    // [SerializeField] private AudioSource backgroundMusic;
    [Header("Volume")]
    [Range(0, 1)]
    [SerializeField] private float volumeBackgrooundMusic;
    [Range(0, 1)]
    [SerializeField] private float volumeTakeCoin;
    [Range(0, 1)]
    [SerializeField] private float volumeClick;

    public bool EnableMusic { get; set; } = true;
    public bool EnableSound { get; set; } = true;

    private float volume = 1f;

    AudioSource _soundSource;

    #region Injects

    private SignalBus signalBus;
    private DataManager dataManager;    

    [Inject]
    private void Construct(SignalBus _signalBus, DataManager _dataManager)
    {        
        signalBus = _signalBus;
        dataManager = _dataManager;
        
    }

    #endregion

    #region Signals

    private void OnEnable()
    {              
        signalBus.Subscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Subscribe<TakeBonusSignal>(TakeBonusSignal);
        signalBus.Subscribe<MusicSignal>(MusicSignal);
        signalBus.Subscribe<SoundSignal>(SoundSignal);
    }

    private void OnDisable()
    {              
        signalBus.Unsubscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Unsubscribe<TakeBonusSignal>(TakeBonusSignal);
        signalBus.Unsubscribe<MusicSignal>(MusicSignal);
        signalBus.Unsubscribe<SoundSignal>(SoundSignal);
    }

    private void MusicSignal()
    {
        UpdateEnableMusic();
    }

    private void UpdateEnableMusic()
    {
        if (EnableMusic)
        {
            EnableMusic = false;
            _soundSource.Pause();
            dataManager.SaveDataMusic(1);

        }
        else
        {
            EnableMusic = true;
            _soundSource.Play();
            dataManager.SaveDataMusic(0);
        }
    }

    private void SoundSignal()
    {
        UpdateEnableSound();
    }

    private void UpdateEnableSound()
    {
        if (EnableSound)
        {
            EnableSound = false;           
            dataManager.SaveDataSound(1);

        }
        else
        {
            EnableSound = true;            
            dataManager.SaveDataSound(0);
        }
    }

    private void TakeCoinSignal()
    {
        TakeCoin();
    }

    IEnumerator TakeBonusSignalCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        Gift3Line();
    }

    private void TakeBonusSignal()
    {

        StartCoroutine(TakeBonusSignalCoroutine());
    }

    #endregion


    private void Awake()
    {        
        _soundSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EnableMusic = dataManager.LoadDataMusic();

        if(EnableMusic ) _soundSource.Play();
        else _soundSource.Stop();

        EnableSound = dataManager.LoadDataSound();

        _soundSource.volume = volumeBackgrooundMusic;
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip && _soundSource && EnableMusic)
        {           
            _soundSource.clip = clip;
            _soundSource.volume = volume;
            _soundSource.Play();
        }
    }

    private void PlaySoundShot(AudioClip clip, float volume)
    {
        if (clip && _soundSource && EnableSound)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }

    public void Click()
    {
        PlaySoundShot(click, volumeClick);
    }

    public void TakeCoin()
    {
        PlaySoundShot(takeCoin, volumeTakeCoin);
    }

    public void TakeGift()
    {
        PlaySoundShot(takeGift, volume);
    }

    public void Gift3Line()
    {
        PlaySoundShot(gift3Line, volume);
    }

    public void SlideStep()
    {
        PlaySoundShot(slideStep, volume);
    }

    public void HitBlock()
    {
        PlaySoundShot(hitBlock, volume);
    }

    public void HitSnowman()
    {
        PlaySoundShot(hitSnowman, volume);
    }

    public void HitBlockGifts()
    {
        PlaySoundShot(hitBlockGifts, volume);
    }

    public void UpgradeBonus()
    {
        PlaySoundShot(upgradeBonus, volume);
    }


    private void Update()
    {
        
    }

}
