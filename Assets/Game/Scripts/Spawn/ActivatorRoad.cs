using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ActivatorRoad : MonoBehaviour
{
    private Road currentRoad;
    private Road newRoad;

    private float positionNewRoadZ;   

    public int CurrentLocation { get; set; }    

    private int currentIdRoadScene;
    private int[] countRoadActive;

    private bool snowEanble = true;

    #region Injects

    private GameManager gameManager;
    private Spawner spawner;
    private Config config;
    private LocationsSettings locationSettings;
    private ActivatorItems activatorItems;
    private FXSnowFalling fXSnowFalling;
    private DataManager dataManager;

    [Inject]
    private void Construct(Config _config,
                           GameManager _gameManager,
                           Spawner _spawner,
                           LocationsSettings _locationSettings,
                           ActivatorItems _activatorItems,
                           FXSnowFalling _fXSnowFalling,
                           DataManager _dataManager)
    {        
        gameManager = _gameManager;
        spawner = _spawner;        
        config = _config;
        locationSettings = _locationSettings;
        activatorItems = _activatorItems;
        fXSnowFalling = _fXSnowFalling;
        dataManager = _dataManager;
    }

    #endregion

    private List<IMove> moveObjects = new List<IMove>();
    public List<IMove> MoveObjects { get => moveObjects; set => moveObjects = value; }

    void Start()
    {
       countRoadActive = new int[spawner.RoadOnScene.Count];

        if (!gameManager.HaveTutorial || !dataManager.LoadDataTutorial())
        {
            CurrentLocation = 1;            
        }

       ActivateRoad(config.StartPositionRoad);
    }

    /// <summary>
    /// ????????? ??????
    /// </summary>
    /// <param name="positionZ"></param>
    private void ActivateRoad(float positionZ)
    {        
        int indexRoadScene = locationSettings.Locations[CurrentLocation].IdRoadScene[currentIdRoadScene];

        newRoad = spawner.RoadOnScene[indexRoadScene].Roads[countRoadActive[indexRoadScene]];

        newRoad.transform.position = new Vector3(0, 0, positionZ);
        newRoad.gameObject.SetActive(true);

        // ?????????? ? ?????? ?????????? ?????????
        if (newRoad is IMove) moveObjects.Add(newRoad);

        CounterRoadPrefabs(indexRoadScene);

        if (locationSettings.Locations[CurrentLocation].ID == 3)
            snowEanble = false;
        else
            snowEanble = true;

        
    }    

    /// <summary>
    /// ??????????? ??????
    /// </summary>
    /// <param name="_road"></param>
    private void DeactivateRoad(MoveObjects _road)
    {
        _road.gameObject.SetActive(false);
        _road.transform.position = Vector3.zero;

        // ???????? ?? ?????? ?????????? ?????????
        if (_road is IMove) moveObjects.Remove(_road);
    }

    void Update()
    {
        foreach (var item in MoveObjects)
        {
            item.Execute();
        }

        if (newRoad.Begin)
        {            


            if (currentRoad != null)
            {
                DeactivateRoad(currentRoad);
            }

            if (newRoad != null)
            {
                currentRoad = newRoad;

                if (!snowEanble)
                    fXSnowFalling.StopSnow();
                else
                    fXSnowFalling.PlaySnow();
            }

            positionNewRoadZ = currentRoad.transform.position.z + config.StepPositionRoad;

            CounterRoadScene();

            CounterLocation();

            ActivateRoad(positionNewRoadZ);

            currentRoad.Begin = false;           
        }
    }

    private void CounterLocation()
    {
        if(locationSettings.Locations[CurrentLocation].IdRoadScene.Length == currentIdRoadScene)
        {
            currentIdRoadScene = 0;
            CurrentLocation++;
            
        }

        if(CurrentLocation == locationSettings.Locations.Count)
        {
            CurrentLocation = 1;
        }

        //print(CurrentLocation + " CurrentLocation");

        activatorItems.FillChanceBlock();
    }

    /// <summary>
    /// ??????? ????? ???????
    /// </summary>
    private void CounterRoadScene()
    {      
        currentIdRoadScene++;
    }

    /// <summary>
    /// ??????? ???????? ?????????? ??????
    /// </summary>
    private void CounterRoadPrefabs(int indexRoadScene)
    {
        if (countRoadActive[indexRoadScene] == spawner.RoadOnScene[currentIdRoadScene].Roads.Count - 1)
        {
            countRoadActive[indexRoadScene] = 0;
        }
        else
        {
            countRoadActive[indexRoadScene]++;
        }
    }
}
