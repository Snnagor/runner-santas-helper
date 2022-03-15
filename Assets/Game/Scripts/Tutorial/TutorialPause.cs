using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialPause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private bool topBlockTutorial;
    [SerializeField] private bool matchTutorial;
    [SerializeField] private bool LeftRightTutorial;

    private bool clickScreen;

    #region Injects

    private GameManager gameManager;
    private DataManager dataManager;
    private SignalBus signalBus;
   
    [Inject]
    private void Construct(GameManager _gameManager, DataManager _dataManager, SignalBus _signalBus)
    {
        gameManager = _gameManager;
        dataManager = _dataManager;
        signalBus = _signalBus;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<TopTutorialSignal>(TopTutorialSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<TopTutorialSignal>(TopTutorialSignal);
    }

    private void TopTutorialSignal()
    {       
        if (topBlockTutorial && dataManager.LoadDataTutorial2())
        {
            gameManager.DisableTutorial();
            dataManager.SaveDataTutorial2();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    #endregion

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.HaveTutorial)
        {
            if (other.CompareTag("Player"))
            {                
                //if (topBlockTutorial && dataManager.LoadDataTutorial2())
                //{
                //    RunnerGifts runnerGifts = other.GetComponent<RunnerGifts>();
                //    if (runnerGifts?.CountActiveGift < 6) return;
                //    gameManager.DisableTutorial();
                //    dataManager.SaveDataTutorial2();
                //    pausePanel.SetActive(true);
                //    Time.timeScale = 0;
                //}               

                if (dataManager.LoadDataTutorial() && !topBlockTutorial)
                {
                    pausePanel.SetActive(true);
                    Time.timeScale = 0;

                    if (LeftRightTutorial)
                        clickScreen = true;
                }

                if (matchTutorial)
                {
                    dataManager.SaveDataTutorial();
                }
            }
        }
    }

    public void ButtonOk()
    {
        Time.timeScale = 1;       
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (LeftRightTutorial && clickScreen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ButtonOk();
                clickScreen = false;
            }
        }
    }
}
