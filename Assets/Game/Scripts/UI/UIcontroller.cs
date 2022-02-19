using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class UIcontroller : MonoBehaviour
{
    [Header("Start Panel")]
    [SerializeField] private Text startTotalScoreValueText;
    [SerializeField] private Text startTotalCoinValueText;

    [Header("Game Panel")]
    [SerializeField] private Text scoreValueText;
    [SerializeField] private Text distanceValueText;
    [SerializeField] private Text coinQtyText;
    [SerializeField] private Text giftQtyText;

    [Header("Bonus Panel")]    
    [SerializeField] private ImageTimer[] bonusTimer;    
   // [SerializeField] private GameObject[] bonusTimeItems;

    private int currentBonusPanel;

    [Header("Lose Panel")]
    [SerializeField] private Text distanceValue;    
    [SerializeField] private Text countBonusValue;
    [SerializeField] private Text scoreGiftValue;
    [SerializeField] private Text scoreBonusValue;
    

    [Header("UI Settings")]
    [SerializeField] private Text countTrackValue;
    [SerializeField] private Text widthTrackValue;    

    [Header("UI Canvases")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pausePanel;    
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject topPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject continuePanel;

    private GameObject currentPanel;

    #region Injects

    private SignalBus signalBus;
    private ScoreManager scoreManager;
    private DataManager dataManager;
    private GameManager gameManager;
    private Config config;
    private SoundManager soundManager;
    // private SettingsFile settingsFile;

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           ScoreManager _scoreManager, 
                           DataManager _dataManager, 
                           GameManager _gameManager,
                           SoundManager _soundManager)
    {       
        signalBus = _signalBus;
        scoreManager = _scoreManager;
        dataManager = _dataManager;
        gameManager = _gameManager;
        config = _config;
        soundManager = _soundManager;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<LoseSignal>(LoseSignal);
        signalBus.Subscribe<MeterSignal>(MeterSignal);
        signalBus.Subscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Subscribe<TakeBonusSignal>(TakeBonusSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<LoseSignal>(LoseSignal);
        signalBus.Unsubscribe<MeterSignal>(MeterSignal);
        signalBus.Unsubscribe<TakeCoinSignal>(TakeCoinSignal);
        signalBus.Unsubscribe<TakeBonusSignal>(TakeBonusSignal);
    }
    private void LoseSignal()
    {
        StartCoroutine(LoseCoroutine());       
    }

    private void MeterSignal()
    {
        if(distanceValueText != null)
        distanceValueText.text = scoreManager.CountMeter.ToString();
        UpdateScoreInfo();
    }

    private void TakeCoinSignal()
    {
        UpdateCoinInfo();
    }

    private void TakeBonusSignal()
    {
        UpdateGiftInfo();
    }

    #endregion

    private void Start()
    {
        if (!config.Restart)
        {
            StartPanel();
        }
        else
        {
            StartBtn();
            config.Restart = false;
        }

        if(countTrackValue != null)
           countTrackValue.text = gameManager.CountTrack.ToString();

        if (widthTrackValue != null)
            widthTrackValue.text = gameManager.WidthTrack.ToString();

        UpdateCoinInfo();
        UpdateGiftInfo();
        UpdateScoreInfo();

    }

    /// <summary>
    /// Изменение UI
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(GameObject state)
    {      
        if (currentPanel != null)
        {
            if(state != pausePanel)
               currentPanel.SetActive(false);
        }
                  
        
        currentPanel = state;

        if(currentPanel != null)
        currentPanel.SetActive(true);
    }

    #region Update Info

    private void UpdateScoreInfo()
    {
        if (scoreValueText != null)
            scoreValueText.text = scoreManager.TotalScore.ToString();
    }

    private void UpdateCoinInfo()
    {
        if(coinQtyText != null)
          coinQtyText.text = scoreManager.CountCoin.ToString();
    }

    private void UpdateGiftInfo()
    {
        if(giftQtyText != null)
          giftQtyText.text = scoreManager.GiftQty.ToString();
    }

    #endregion

    #region BonusTimerPanel

    /// <summary>
    /// Запуск таймера бонуса
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="time"></param>
    /// <param name="actionIndexBonus"></param>
    public void UpdateBonusInfoStart(Sprite _sprite, float time, int actionIndexBonus)
    {        
        for (int i = 0; i < bonusTimer.Length; i++)
        {
            if (bonusTimer[i].Enable)
            {                
                continue;
            }
            else
            {
                bonusTimer[i].IconBonus.sprite = _sprite;
                bonusTimer[i].IconBonusBack.sprite = _sprite;
                bonusTimer[i].MaxTime = time;
                bonusTimer[i].gameObject.SetActive(true);                
                bonusTimer[i].ActionIndexBonus = actionIndexBonus;
                bonusTimer[i].Enable = true;                
                break;
            }
        }                     
    }

    /// <summary>
    /// Перезапуск времени при взятии повторного бонуса
    /// </summary>
    /// <param name="time"></param>
    /// <param name="actionIndexBonus"></param>
    public void ResetTimer(float time, int actionIndexBonus)
    {       

        for (int i = 0; i < bonusTimer.Length; i++)
        {
            if(bonusTimer[i].ActionIndexBonus == actionIndexBonus)
            {
                bonusTimer[i].ReseTime();
                break;
            }
        }
    }

    /// <summary>
    /// Выключение таймера бонуса
    /// </summary>
    public void UpdateBonusInfoEnd()
    {
        bonusTimer[currentBonusPanel].gameObject.SetActive(false);

        if(currentBonusPanel > 0)
        currentBonusPanel--;
    }



    //public void BonusTimeTiems(int id)
    //{
    //    bonusTimeItems[id].SetActive(false);
    //}

    //private void ActiveBonusTimeItems()
    //{
    //    foreach (var item in bonusTimeItems)
    //    {
    //        if(!item.activeSelf)
    //            item.SetActive(true);
    //    }
    //}

    #endregion

    #region Игровые панели

    /// <summary>
    /// Стартовый UI
    /// </summary>
    private void StartPanel()
    {
        ChangeState(startPanel);

        if(startTotalScoreValueText != null)
        startTotalScoreValueText.text = dataManager.LoadFirstTopData().ToString();

        if (startTotalCoinValueText != null)
            startTotalCoinValueText.text = dataManager.LoadTotalCoinData().ToString();
    }

    /// <summary>
    /// Игровой UI
    /// </summary>
    private void GamePanel()
    {
        Time.timeScale = 1;
        ChangeState(gamePanel);
    }

    /// <summary>
    /// Пауза UI
    /// </summary>
    private void PausePanel()
    {
        Time.timeScale = 0;
        ChangeState(pausePanel);
    }

   IEnumerator LoseCoroutine()
    {
        yield return new WaitForSeconds(2f);

    #if UNITY_ANDROID

        if(gameManager.QtyLife > 0)
        {
            ContinuePanel();
        }
        else
        {
            LosePanel();
        }
    #else

         LosePanel();

    #endif
    }

    /// <summary>
    /// Коней игры UI
    /// </summary>
    private void LosePanel()
    {
        ChangeState(losePanel);
        UpdateInfoLoasePanel();
    }

    /// <summary>
    /// Продолжить игру UI
    /// </summary>
    private void ContinuePanel()
    {
        gameManager.QtyLife--;
        ChangeState(continuePanel);        
    }

    /// <summary>
    /// Настройки UI
    /// </summary>
    private void SettingsPanel()
    {        
        ChangeState(settingsPanel);
    }

    /// <summary>
    /// Лучшие результаты UI
    /// </summary>
    private void TopPanel()
    {
        ChangeState(topPanel);
    }

    /// <summary>
    /// Информация UI
    /// </summary>
    private void InfoPanel()
    {
        ChangeState(infoPanel);
    }

    /// <summary>
    /// Магазин UI
    /// </summary>
    private void ShopPanel()
    {
        ChangeState(shopPanel);
    }

    /// <summary>
    /// Обновление информации на панели проиграша
    /// </summary>
    private void UpdateInfoLoasePanel()
    {
        distanceValue.text = scoreManager.CountMeter.ToString();
        countBonusValue.text = scoreManager.CountCoin.ToString();
        scoreGiftValue.text = scoreManager.GiftQty.ToString();
        scoreBonusValue.text = scoreManager.TotalScore.ToString();        
    }

#endregion

#region Buttons


    /// <summary>
    /// Кнопка закончить игру
    /// </summary>
    public void LoseBtn()
    {
        SoundClick();
        LosePanel();
    }

    /// <summary>
    /// Кнопка продолжить игру
    /// </summary>
    public void ContinueBtn()
    {
        SoundClick();
        signalBus.Fire(new ContinueSignal());
        GamePanel();
    }

    /// <summary>
    /// Кнопка старт
    /// </summary>
    public void StartBtn()
    {
        SoundClick();
        signalBus.Fire(new StartSignal());
        GamePanel();
    }

    /// <summary>
    /// Кнопка главного меню
    /// </summary>
    public void MainMenuBtn()
    {
        Time.timeScale = 1;
        SoundClick();
        config.Restart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Кнопка рестарт
    /// </summary>
    public void RestartBtn()
    {
        Time.timeScale = 1;
        SoundClick();
        config.Restart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Кнопка паузы
    /// </summary>
    public void PauseBtn()
    {
        SoundClick();
        PausePanel();
    }

    /// <summary>
    /// Кнопка продолжить
    /// </summary>
    public void ResumeBtn()
    {
        SoundClick();
        GamePanel();
    }

    /// <summary>
    /// Кнопка назад
    /// </summary>
    public void BackBtn()
    {
        SoundClick();
        StartPanel();
    }

    /// <summary>
    /// Кнопка настройки
    /// </summary>
    public void SettingsBtn()
    {
        SoundClick();
        SettingsPanel();
    }

    /// <summary>
    /// Кнопка лидеры
    /// </summary>
    public void TopBtn()
    {
        SoundClick();
        TopPanel();
    }

    /// <summary>
    /// Кнопка инормации
    /// </summary>
    public void InfoBtn()
    {
        SoundClick();
        InfoPanel();
    }

    /// <summary>
    /// Кнопка магазин
    /// </summary>
    public void ShopBtn()
    {
        SoundClick();
        ShopPanel();
    }

    /// <summary>
    /// Кнопка выхода
    /// </summary>
    public void QuitBtn()
    {
        SoundClick();
        Application.Quit();
    }

    /// <summary>
    /// Кнопка музки
    /// </summary>
    public void MusicBtn()
    {
        SoundClick();

        signalBus.Fire(new MusicSignal());
        
    }

    /// <summary>
    /// Кнопка звуков
    /// </summary>
    public void SoundBtn()
    {
        SoundClick();

        signalBus.Fire(new SoundSignal());

    }

    /// <summary>
    /// Кнопка уменьшения количества дорожек
    /// </summary>
    public void LeftArrowCountTrack()
    {
        if(gameManager.CountTrack > config.MinCountTrack)
        {
            gameManager.CountTrack --;
            countTrackValue.text = gameManager.CountTrack.ToString();
            signalBus.Fire(new ChangeCountTrackSignal());
        }
           
    }

    /// <summary>
    /// Кнопка увеличения количества дорожек
    /// </summary>
    public void RightArrowCountTrack()
    {
        if (gameManager.CountTrack < config.MaxCountTrack)
        {
            gameManager.CountTrack ++;
            countTrackValue.text = gameManager.CountTrack.ToString();
            signalBus.Fire(new ChangeCountTrackSignal());
        }            
    }

    /// <summary>
    /// Кнопка уменьшения ширины дорожек
    /// </summary>
    public void LeftArrowWidthTrack()
    {
        if (gameManager.WidthTrack > config.MinWidthTrack)
        {
            gameManager.WidthTrack -= 5;
            widthTrackValue.text = gameManager.WidthTrack.ToString();
            signalBus.Fire(new ChangeWidthTrackSignal());
        }            
    }

    /// <summary>
    /// Кнопка увеличения ширины дорожек
    /// </summary>  
    public void RightArrowWidthTrack()
    {
        if (gameManager.WidthTrack < config.MaxWidthTrack)
        {
            gameManager.WidthTrack += 5;
            widthTrackValue.text = gameManager.WidthTrack.ToString();
            signalBus.Fire(new ChangeWidthTrackSignal());
        }            
    }

    private void SoundClick()
    {
        soundManager.Click();
    }

#endregion

    private void OnApplicationQuit()
    {
        config.Restart = false;
    }

}
