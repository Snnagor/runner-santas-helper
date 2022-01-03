using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Text totalCoinText;

    [SerializeField] private ShopBarProgress[] slotBonus;

    private int[] levelBonus;

    #region Injects

    private SignalBus signalBus;
    private ScoreManager scoreManager;
    private DataManager dataManager;
    private Config config;
    private ShopSettings shopSettings;
    private SoundManager soundManager;

    [Inject]
    private void Construct(Config _config,
                           SignalBus _signalBus,
                           ScoreManager _scoreManager,
                           DataManager _dataManager,
                           ShopSettings _shopSettings,
                           SoundManager _soundManager)
    {
        signalBus = _signalBus;
        scoreManager = _scoreManager;
        dataManager = _dataManager;        
        config = _config;
        shopSettings = _shopSettings;
        soundManager = _soundManager;
    }

    #endregion

    private void Start()
    {
        levelBonus = new int[shopSettings.UpgradesBonuses.Count];
        LoadDataProgress();
        UpdateBonusUpgradeInfo();
    }

    private void OnEnable()
    {
        LoadDataProgress();
    }

    /// <summary>
    /// Обновление шкалы прогресса
    /// </summary>
    public void LoadDataProgress()
    {      

        if (levelBonus != null)
        {
            for (int i = 0; i < slotBonus.Length; i++)
            {
                levelBonus[i] = dataManager.LoadDataUpgrade(i);
                slotBonus[i].ShowProgress(levelBonus[i]);
            }        
        }
    }

    /// <summary>
    /// Обновление информации о бонусах
    /// </summary>
    public void UpdateBonusUpgradeInfo()
    {
        if (levelBonus != null)
        {           

            for (int i = 0; i < slotBonus.Length; i++)
            {               

                slotBonus[i].UpdateInfo(i, levelBonus[i]) ;
            }
        }

        UpdateInfoTotalCoin();
    }

    /// <summary>
    /// Кнопка купить
    /// </summary>
    /// <param name="idBonus"></param>
    public void BuyBtn(int idBonus)
    {       

        if (levelBonus[idBonus] < (shopSettings.UpgradesBonuses[idBonus].PriceUpgrade.Count - 1))
        {
            soundManager.UpgradeBonus();
            levelBonus[idBonus] += 1;
            slotBonus[idBonus].Buy(idBonus, levelBonus[idBonus]);
            slotBonus[idBonus].ShowProgress(levelBonus[idBonus]);
            dataManager.SaveDataUpgrade(idBonus, levelBonus[idBonus]);
            UpdateBonusUpgradeInfo();
        }
    }

    /// <summary>
    /// Обновление информации об обще количестве монет
    /// </summary>
    private void UpdateInfoTotalCoin()
    {
        totalCoinText.text = dataManager.TotalCoin.ToString();
    } 
}
