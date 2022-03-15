using UnityEngine;
using Zenject;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string[] topName;
    public string[] TopName { get => topName; }

    public const string SCORE = "score";
    public const string COINS = "coin";    
    public const string SOUND = "sound";
    public const string MUSIC = "music";
    public const string TUTORIAL = "tutorial";
    public const string TUTORIAL2 = "tutorialTop";
    //public const string TOP1 = "top1";
    //public const string TOP2 = "top2";
    //public const string TOP3 = "top3";

    public int TotalScore { get; set; }

    private int totalCoin;
    public int TotalCoin 
    {
        get
        {
            return totalCoin;
        }
        
        set
        {
            totalCoin = value;
            SaveDataCoin();
        }
    }
    public int TotalSled { get; set; }
    public int TotalDoubleScore { get; set; }
    public int TotalMagnet { get; set; }


    #region Injects

    private SignalBus signalBus;
    private ScoreManager scoreManager;
    private GameManager gameManager;
    private ShopSettings shopSettings;

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           ScoreManager _scoreManager, 
                           GameManager _gameManager,
                           ShopSettings _shopSettings)
    {
        signalBus = _signalBus;
        scoreManager = _scoreManager;
        gameManager = _gameManager;
        shopSettings = _shopSettings;
       // settingsFile = _settingsFile;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<LoseSignal>(LoseSignal);        
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<LoseSignal>(LoseSignal);        
    }

    private void LoseSignal()
    {
        SaveDataScore();
        TotalCoin += scoreManager.CountCoin;

        SaveDataTop(scoreManager.TotalScore);
    }
    #endregion


    private void Start()
    {
        totalCoin = LoadTotalCoinData();
    }

    /// <summary>
    /// Сохранить лучший результат
    /// </summary>
    private void SaveDataTop(int scoreResult)
    {
        int tmpResult = 0;
        bool isSave = false;

        for (int i = 0; i < topName.Length; i++)
        {
            int saveResult = PlayerPrefs.GetInt(topName[i]);

            if (scoreResult > saveResult && !isSave)
            {
                isSave = true;

                tmpResult = saveResult;
                PlayerPrefs.SetInt(topName[i], scoreResult);
                continue;

            }
            
            if(tmpResult > 0)
            {      
                if (i + 1 < topName.Length)
                {
                    PlayerPrefs.SetInt(topName[i + 1], saveResult);             
                }

                PlayerPrefs.SetInt(topName[i], tmpResult);

                break;
            }
        }
    }

    /// <summary>
    /// Загрузить лучшие результаты
    /// </summary>
    /// <returns></returns>
    public int[] LoadTopData()
    {
        int[] tmpArray = new int[topName.Length];

        for (int i = 0; i < topName.Length; i++)
        {          
            tmpArray[i] = PlayerPrefs.GetInt(topName[i]);
        }

        return tmpArray;
    }

    /// <summary>
    /// ЗАгрузить один лучший результат
    /// </summary>
    /// <returns></returns>
    public int LoadFirstTopData()
    {        
        return PlayerPrefs.GetInt(topName[0]);
    }


    /// <summary>
    /// Сохранить количество очков
    /// </summary>
    private void SaveDataScore()
    {
        PlayerPrefs.SetInt(SCORE, TotalScore + scoreManager.TotalScore);
    }

    /// <summary>
    /// Сохранить количество монет
    /// </summary>
    private void SaveDataCoin()
    {
        PlayerPrefs.SetInt(COINS, totalCoin);
    }

    /// <summary>
    /// Сохранить купленного апгрейда
    /// </summary>
    public void SaveDataUpgrade(int idBonus, int levelBonus)
    {
        PlayerPrefs.SetInt(shopSettings.UpgradesBonuses[idBonus].SaveIdentifier, levelBonus);    
    }

    /// <summary>
    /// ЗАгрузить общее количество очков
    /// </summary>
    /// <returns></returns>
    public int LoadTotalScoreData()
    {
        if(TotalScore == 0)
        TotalScore = PlayerPrefs.GetInt(SCORE);

        return TotalScore;
    }

    /// <summary>
    /// Загрузить общее количество монет
    /// </summary>
    /// <returns></returns>
    public int LoadTotalCoinData()
    {
        if (TotalCoin == 0)
            TotalCoin = PlayerPrefs.GetInt(COINS);

        return TotalCoin;
    }

    public int LoadDataUpgrade(int id)
    {        
       return PlayerPrefs.GetInt(shopSettings.UpgradesBonuses[id].SaveIdentifier);
    }

    /// <summary>
    /// Сохранить музыку
    /// </summary>
    public void SaveDataMusic(int value)
    {
      PlayerPrefs.SetInt(MUSIC, value);
    }

    /// <summary>
    /// Загрузить вкл/выкл музыку
    /// </summary>
    public bool LoadDataMusic()
    {
        if (PlayerPrefs.GetInt(MUSIC) == 0)
            return true;

        return false;       
    }


    /// <summary>
    /// Сохранить звуки
    /// </summary>
    public void SaveDataSound(int value)
    {
        PlayerPrefs.SetInt(SOUND, value);
    }

    /// <summary>
    /// Загрузить вкл/выкл звуки
    /// </summary>
    public bool LoadDataSound()
    {
        if (PlayerPrefs.GetInt(SOUND) == 0)
            return true;

        return false;
    }

    private void OnApplicationQuit()
    {
        //if (gameManager.IsRun)
        //{
        //    SaveDataScore();
        //    SaveDataCoin();
        //}
    }

    /// <summary>
    /// Сохранить туториал
    /// </summary>
    public void SaveDataTutorial()
    {
        PlayerPrefs.SetInt(TUTORIAL, 1);
    }

    /// <summary>
    /// Загрузить туториал
    /// </summary>
    public bool LoadDataTutorial()
    {
        if (PlayerPrefs.GetInt(TUTORIAL) == 0)
            return true;

        return false;
    }

    /// <summary>
    /// Сохранить туториал
    /// </summary>
    public void SaveDataTutorial2()
    {
        PlayerPrefs.SetInt(TUTORIAL2, 1);
    }

    /// <summary>
    /// Загрузить туториал
    /// </summary>
    public bool LoadDataTutorial2()
    {
        if (PlayerPrefs.GetInt(TUTORIAL2) == 0)
            return true;

        return false;
    }
}
