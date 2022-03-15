using UnityEngine;
using Zenject;

public class ScoreManager : MonoBehaviour
{
   public int CountMeter { get; set; }

   public int DisctanseScore { get; set; }

   public int CountCoin { get; set; }

   private int countDuck;
   public int CountDuck
    { 
        get => countDuck;
        set 
        {
            countDuck = value;
            viewModel.Ducks = countDuck.ToString();
        } 
    }

   public int CoinScore { get; set; }

   public int GiftQty { get; set; }

    public int TotalScore 
    { 
       get
        {
            return DisctanseScore;
        }
    }

    private int scoreMeter;
    private int scoreCoin;
    private int accelerationDistance;

    #region Injects

    private SignalBus signalBus;
    private Config config;
    private DoubleCoinBonus doubleCoinBonus;
    private ViewModel viewModel;

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           DoubleCoinBonus _doubleCoinBonus,
                           ViewModel _viewModel)
    {
        signalBus = _signalBus;
        config = _config;
        doubleCoinBonus = _doubleCoinBonus;
        viewModel = _viewModel;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {       
        signalBus.Subscribe<MeterSignal>(MeterSignal);
        signalBus.Subscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Subscribe<TakeBonusSignal>(TakeBonusSignal);
    }

    private void OnDisable()
    {       
        signalBus.Unsubscribe<MeterSignal>(MeterSignal);
        signalBus.Unsubscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Unsubscribe<TakeBonusSignal>(TakeBonusSignal);
    }
    
    private void TakeCoinSignal()
    {       
         CountCoin++;
         CoinScore += scoreCoin;
        
    }

    /// <summary>
    /// ѕодсчет дистанции и очков за пройденую дистанцию
    /// </summary>
    private void MeterSignal()
    {
        CountMeter++;

        if (doubleCoinBonus.Enable)
        {
            DisctanseScore += scoreMeter * doubleCoinBonus.MultipleCoin;
        }
        else
        {
            DisctanseScore += scoreMeter;
        }        

        if(CountMeter > 0 && CountMeter % accelerationDistance == 0)
        {
            signalBus.Fire(new AccelerationSignal());
        }
    }

    private void TakeBonusSignal()
    {        
            GiftQty += 3;
       
    }

    #endregion

    private void Start()
    {
        scoreMeter = config.ScoreMeter;
        scoreCoin = config.ScoreBonus;
        accelerationDistance = config.AccelerationDistance;
    }

}
