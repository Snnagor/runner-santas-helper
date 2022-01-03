using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DifficultZone : MonoBehaviour
{
    public int CurrentDZone { get; set; }
    public int[] DifficultChance { get; set; } = new int[10];

    private int checkpointMeter;

    #region Injects

    private Config config;
    private DifficultSettings difficultSettings;
    private ScoreManager scoreManager;
    private ActivatorItems activatorItems;
    private GameManager gameManager;

    [Inject]
    private void Construct(Config _config,
                           DifficultSettings _difficultSettings,
                           ScoreManager _scoreManager,
                           ActivatorItems _activatorItems,
                           GameManager _gameManager)
    {
        config = _config;
        difficultSettings = _difficultSettings;
        scoreManager = _scoreManager;
        activatorItems = _activatorItems;
        gameManager = _gameManager;
    }

    #endregion

    private void Start()
    {
        UpdateSettingsInActivator();
        FillChance();

        if (!gameManager.HaveTutorial)
        {
            CurrentDZone = 1;
        }
    }

    private void Update()
    {        
        if (scoreManager.CountMeter < checkpointMeter + difficultSettings.Zones[CurrentDZone].Meter)
            return;

        checkpointMeter = scoreManager.CountMeter;

        CurrentDZone++;

        if (CurrentDZone == difficultSettings.Zones.Count)
        {
            CurrentDZone = 1;
        }

        FillChance();
        UpdateSettingsInActivator();
    }

    /// <summary>
    /// Заполняем массив вероятности на текущую зону
    /// </summary>
    private void FillChance()
    {        
        int counter = 0;
        for (int i = 0; i < difficultSettings.Zones[CurrentDZone].ElementChance.Count; i++)
        {
            for (int j = 0; j < difficultSettings.Zones[CurrentDZone].ElementChance[i]; j++)
            {
                DifficultChance[counter] = i;

                counter++;
            }

        }

        activatorItems.DifficultChance = DifficultChance;
    }

    public void UpdateSettingsInActivator()
    {
       activatorItems.DeltaDistanceItems = (float) difficultSettings.Zones[CurrentDZone].DeltaDistanceItem;
    }
        
}
