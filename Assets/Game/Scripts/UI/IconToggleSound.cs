using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class IconToggleSound : MonoBehaviour
{
    [SerializeField] protected Sprite offIcon;
    [SerializeField] protected Sprite onIcon;

    protected Image btn;

    protected bool iconBool = true;

    #region Injects

    private SignalBus signalBus;
    protected SoundManager soundManager;

    [Inject]
    private void Construct(SignalBus _signalBus, SoundManager _soundManager)
    {
        signalBus = _signalBus;
        soundManager = _soundManager;
    }

    #endregion   

    #region Signals

    private void OnEnable()
    {        
        signalBus.Subscribe<SoundSignal>(SoundSignal);
    }

    private void OnDisable()
    {        
        signalBus.Unsubscribe<SoundSignal>(SoundSignal);
    }

    private void MusicSignal()
    {
        IconBtn();
    }

    private void SoundSignal()
    {
        IconBtn();
    }

    #endregion

    private void Awake()
    {
        btn = GetComponent<Image>();
    }

    private void Start()
    {
        iconBool = soundManager.EnableSound;

        if (iconBool)
            btn.sprite = onIcon;
        else
            btn.sprite = offIcon;
    }

    public void IconBtn()
    {
        if (iconBool)
        {
            iconBool = false;
            btn.sprite = offIcon;
        }
        else
        {
            iconBool = true;
            btn.sprite = onIcon;
        }
    }


    
}
