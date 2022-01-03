using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager: MonoBehaviour
{
    [SerializeField] private bool haveTutorial;
    public bool HaveTutorial { get => haveTutorial; set => haveTutorial = value; }

    [SerializeField] private int qtyLife;
    public int QtyLife { get => qtyLife; set => qtyLife = value; }

    public float Speed { get; set; }
    public float CurrentSpeed { get; set; }    
    public bool IsSled { get; set; }
    public float SpeedSidestep { get; set; }
    public bool IsRun { get; set; }
    public int CountTrack { get; set; }
    public float WidthTrack { get; set; }

    private float acceleration;

    private SignalBus signalBus;
    private Config config;
    //private SettingsFile settingsFile;

    private bool speedContinue;

    #region Injects

    private RunnerAnimation runnerAnimation;
    private RunnerMove runnerMove;

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           RunnerAnimation _runnerAnimation,
                           RunnerMove _runnerMove)
    {        
        signalBus = _signalBus;      
       // settingsFile = _settingsFile; 
        config = _config;
        runnerAnimation = _runnerAnimation;
        runnerMove = _runnerMove;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<StartSignal>(StartSignal);
        signalBus.Subscribe<LoseSignal>(LoseSignal);
        signalBus.Subscribe<AccelerationSignal>(AccelerationSignal);
        signalBus.Subscribe<ContinueSignal>(ContinueSignal);
        signalBus.Subscribe<MeterSignal>(MeterSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<StartSignal>(StartSignal);
        signalBus.Unsubscribe<LoseSignal>(LoseSignal);
        signalBus.Unsubscribe<AccelerationSignal>(AccelerationSignal);
        signalBus.Unsubscribe<ContinueSignal>(ContinueSignal);
        signalBus.Unsubscribe<MeterSignal>(MeterSignal);
    }

    private void StartSignal()
    {
        IsRun = true;
    }

    private void ContinueSignal()
    {        
        StartCoroutine(ContinueCoroutine());
    }


    IEnumerator ContinueCoroutine()
    {
        yield return new WaitForSeconds(0.65f);
        runnerMove.MigEnable(true);
        runnerMove.CollisionBlock = false;
        speedContinue = true;
        CurrentSpeed = 40f;
        IsRun = true;
    }

    private void MeterSignal()
    {
        IncreaseSpeed();
    }

    /// <summary>
    /// Плавное увелечение скорости после падерния
    /// </summary>
    private void IncreaseSpeed()
    {
        if (speedContinue)
        {
            if (CurrentSpeed < Speed)
            {
                CurrentSpeed += 10f;

                runnerAnimation.AccelerationSignal();
            }
            else
            {
                CurrentSpeed = Speed;
                runnerMove.MigEnable(false);
                runnerMove.CollisionBlock = true;
                speedContinue = false;
            }
        }
    }

    private void LoseSignal()
    {
        IsRun = false;
    }

    private void AccelerationSignal()
    {       
        Speed += acceleration;

        SpeedSidestep += acceleration/2;

        if(!IsSled)
         CurrentSpeed = Speed;
    }

    #endregion

    private void Start()
    {
     #if UNITY_STANDALONE_WIN

       Application.targetFrameRate = 100;

     #endif

        InitSettingsFromFile();
    }

    private void InitSettingsFromFile()
    {
        Speed = (float)config.StartSpeedPlayer;
        CurrentSpeed = Speed;
        acceleration = (float)config.Acceleration;
        SpeedSidestep = (float)config.SpeedSidestepPlayer;
        CountTrack = config.CountTrack;
        if (CountTrack < config.MinCountTrack)
            CountTrack = config.MinCountTrack;
        if (CountTrack > config.MaxCountTrack)
            CountTrack = config.MaxCountTrack;

        WidthTrack = (float)config.WidthTrack;
        if (WidthTrack < config.MinWidthTrack)
            WidthTrack = config.MinWidthTrack;
        if (WidthTrack > config.MaxWidthTrack)
            WidthTrack = config.MaxWidthTrack;
    }

}
