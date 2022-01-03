using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RunnerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;

    #region Injects

    private SignalBus signalBus;
    private GameManager gameManager;
    private Config config;
    private RunnerGifts runnerGift;

    [Inject]
    private void Construct(Config _config, 
                           GameManager _gameManager, 
                           SignalBus _signalBus, 
                           RunnerGifts _runnerGift)
    {
        config = _config;        
        gameManager = _gameManager;
        signalBus = _signalBus;
        runnerGift = _runnerGift;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<AccelerationSignal>(AccelerationSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<AccelerationSignal>(AccelerationSignal);
    }

    public void AccelerationSignal()
    {
        anim.speed = gameManager.CurrentSpeed * config.SpeedAnimation;
    }

    #endregion

    private void Start()
    {
        AccelerationSignal();
    }
        

    #region Animation

    /// <summary>
    /// Анимация встать
    /// </summary>
    /// <param name="value"></param>
    public void AnimGetUp()
    {       
        anim.SetTrigger("Continue");
        AnimRunBox(false);
    }


    /// <summary>
    /// Анимация бега
    /// </summary>
    /// <param name="value"></param>
    public void AnimRun(bool value)
    {      
        anim.SetBool("Run", value);

        if (runnerGift.CountActiveGift > 0)
            AnimRunBox(true);
    }

    /// <summary>
    /// Анимация проигрыша
    /// </summary>
    public void AnimLose()
    {
        anim.SetTrigger("Fall");
        anim.speed = 1;
    }

    /// <summary>
    /// Анимация бега с подарками вруках
    /// </summary>
    /// <param name="value"></param>
    public void AnimRunBox(bool value)
    {
        anim.SetBool("Box", value);
    }

    /// <summary>
    /// Анимация перепрыгивания на соседнюю дорожку
    /// </summary>
    public void AnimSidestepRight()
    {
        anim.SetTrigger("Jump");
    }

    public void AnimSidestepLeft()
    {
        anim.SetTrigger("JumpLeft");
    }

    public void AnimSled(bool value)
    {
        anim.SetBool("Sled", value);

    }

    #endregion
}
