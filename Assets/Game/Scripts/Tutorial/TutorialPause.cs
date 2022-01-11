using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TutorialPause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private bool topBlockTutorial;    

    #region Injects

    private GameManager gameManager;
   
    [Inject]
    private void Construct(GameManager _gameManager)
    {
        gameManager = _gameManager;
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
                if (topBlockTutorial)
                {
                    RunnerGifts runnerGifts = other.GetComponent<RunnerGifts>();
                    if (runnerGifts?.CountActiveGift < 6) return;
                    gameManager.DisableTutorial();
                }
                
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ButtonOk()
    {
        Time.timeScale = 1;       
        pausePanel.SetActive(false);
    }
}
