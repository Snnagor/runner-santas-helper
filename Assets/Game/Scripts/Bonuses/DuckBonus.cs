using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DuckBonus : MonoBehaviour, IBonus
{
    [SerializeField] private bool haveTimer;
    public bool HaveTimer { get => haveTimer; }
        

    public GameObject PrefabFlyBonus { get; }

    [SerializeField] private DuckBonusFly prefabDuck;
       
    public Sprite IconBonuse { get; }

    public bool Enable { get; set; }
    public float TimeBonus { get; set; }
    public float CurrentTimeBonus { get; set; }

    #region Injects

    private RunnerGifts runnerGift;
    private DiContainer diContainer;

    [Inject]
    private void Construct(RunnerGifts _runnerGift, DiContainer _diContainer)
    {
        runnerGift = _runnerGift;
        diContainer = _diContainer;
    }

    #endregion

    public void CounterTimeBonus()
    {

    }

    public void Disable()
    {
        Enable = false;
    }

    public void Init()
    {
        Enable = true;

        var newDuckFly = diContainer.InstantiatePrefabForComponent<DuckBonusFly>(prefabDuck, runnerGift.PosBonusObject, Quaternion.identity, null);
        
        Disable();
    }

    public void ResetTimer()
    {

    }

    public void Execute()
    {

    }
}
