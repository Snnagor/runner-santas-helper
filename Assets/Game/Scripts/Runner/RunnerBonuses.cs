using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RunnerBonuses : MonoBehaviour
{    
    [SerializeField] private MonoBehaviour[] bonuses;    

    public float CurrentTimeBonus { get; set; } = -2;

    private List<IBonus> ibonuses = new List<IBonus>();
    private IBonus actionBonus;

    private int actionIndexBonus;

    #region Injects

    private GameManager gameManager;
    private SignalBus signalBus;
    private UIcontroller uiControlle;
    private RunnerGifts runnerGift;
    private DataManager dataManager;
    private ShopSettings shopSettings;
    private DiContainer diContainer;

    [Inject]
    private void Construct(GameManager _gameManager, 
                           SignalBus _signalBus, 
                           UIcontroller _uiControlle, 
                           RunnerGifts _runnerGift,
                           DataManager _dataManager,
                           ShopSettings _shopSettings,
                           DiContainer _diContainer)
    {
        gameManager = _gameManager;
        signalBus = _signalBus;
        uiControlle = _uiControlle;
        runnerGift = _runnerGift;
        dataManager = _dataManager;
        shopSettings = _shopSettings;
        diContainer = _diContainer;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<TakeBonusSignal>(TakeBonusSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<TakeBonusSignal>(TakeBonusSignal);
    }

    private void TakeBonusSignal()
    {        
        ChangeBonus();
    }

    #endregion    

    private void Start()
    {
        foreach (var item in bonuses)
        {
            if(item is IBonus bonus)
            {
                ibonuses.Add(bonus);
            }
        }
    }

    /// <summary>
    /// Выбор бонуса
    /// </summary>
    private void ChangeBonus()
    {
        if (!gameManager.IsRun) return;

        // рандоманый выбор бонуса из массива
        actionIndexBonus = Random.Range(0, bonuses.Length);
        actionBonus = ibonuses[actionIndexBonus];

        BonusFly newBonusFly;

        //Вылетающий бонус
        if (actionBonus.HaveTimer)
        {
            newBonusFly = diContainer.InstantiatePrefabForComponent<BonusFly>(actionBonus.PrefabFlyBonus,
                                                                              runnerGift.PosBonusObject,
                                                                              Quaternion.identity, null);
            newBonusFly.IdBonus = actionIndexBonus;
        }                
        else
        {
            // старт бонуса без таймера
            actionBonus.Init();
        }       
    }    

    /// <summary>
    /// Старт бонуса с таймером
    /// </summary>
    public void StartBonusTimer(int currentIdBonus)
    {
        if (!gameManager.IsRun) return;

        IBonus currentBonus = ibonuses[currentIdBonus];

        float timeBonus = QueryTimeBonus(currentIdBonus);

        currentBonus.TimeBonus = timeBonus;

          if (!currentBonus.Enable)
            {
               
                uiControlle.UpdateBonusInfoStart(currentBonus.IconBonuse, timeBonus, currentIdBonus);
            
                currentBonus.Init();
            }
            else
            {                
               uiControlle.ResetTimer(timeBonus, currentIdBonus);
               currentBonus.ResetTimer();              
            }   
    }

    /// <summary>
    /// Запрос времни бонуса из магазина
    /// </summary>
    private float QueryTimeBonus(int idBonus)
    {
        int indexTimeBonus = dataManager.LoadDataUpgrade(idBonus);       

        return shopSettings.UpgradesBonuses[idBonus].PriceUpgrade[indexTimeBonus].TimeBonus;
    }

}
