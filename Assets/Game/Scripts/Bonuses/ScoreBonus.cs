using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreBonus : MonoBehaviour, IBonus
{
    [SerializeField] private bool haveTimer;
    public bool HaveTimer { get => haveTimer;}

    [SerializeField] private int scorePlus;

    public GameObject PrefabFlyBonus { get; }

    [SerializeField] private ScoreBonusFly prefabScore;
    
    public int ScorePlus { get=> scorePlus; }
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

        var newScoreFly = diContainer.InstantiatePrefabForComponent<ScoreBonusFly>(prefabScore, runnerGift.PosBonusObject, Quaternion.identity, null);        
        newScoreFly.BonusScoreValue = scorePlus;

        Disable();
    }

    public void ResetTimer()
    {

    }

}
