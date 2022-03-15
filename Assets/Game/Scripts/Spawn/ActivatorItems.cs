using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ActivatorItems : MonoBehaviour
{
    public List<GameObject[]> lines = new List<GameObject[]>();
    public int[] DifficultChance { get; set; } = new int[10];

    private int countCoin, countEmpty, countGift, countEmptyFlag;
    private int[] countBlock;
    private float stepLine;
    public float DeltaDistanceItems { get; set; }

    #region Injects

    private GameManager gameManager;
    private Spawner spawner;
    private Config config;
    private SignalBus signalBus;
    private LocationsSettings locationSettings;
    private ActivatorRoad activationRoad;
    private DiContainer diContainer;
    private DataManager dataManager;

    [Inject]
    private void Construct(Config _config,
                           GameManager _gameManager,
                           Spawner _spawner,
                           SignalBus _signalBus,
                           LocationsSettings _locationSettings,
                           ActivatorRoad _activationRoad,
                           DiContainer _diContainer,
                           DataManager _dataManager)
    {
        gameManager = _gameManager;
        spawner = _spawner;
        config = _config;
        signalBus = _signalBus;
        locationSettings = _locationSettings;
        activationRoad = _activationRoad;
        diContainer = _diContainer;
        dataManager = _dataManager;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<ChangeCountTrackSignal>(ChangeCountTrack);
        signalBus.Subscribe<ChangeWidthTrackSignal>(ChangeCountTrack);
        signalBus.Subscribe<CreateLineSignal>(CreateLineSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<ChangeCountTrackSignal>(ChangeCountTrack);
        signalBus.Unsubscribe<ChangeWidthTrackSignal>(ChangeCountTrack);
        signalBus.Unsubscribe<CreateLineSignal>(CreateLineSignal);
    }

    private void ChangeCountTrack()
    {
        ClearRoadOfLines();
        StartAddLine();
    }

    private void CreateLineSignal()
    {
        Deactivate();
        AddLine();
    }

    #endregion

    void Start()
    {
        countBlock = new int[spawner.BlockOnScene.Count];

        if (!gameManager.HaveTutorial || !dataManager.LoadDataTutorial())
        {
            StartAddLine();
        }
        else
        {
            StartAddLineEmpty();
        }
    }

    #region Tutorial

    /// <summary>
    /// Стартовое расположение линий
    /// </summary>
    private void StartAddLineEmpty()
    {
        for (int i = 0; i < config.CountLineOnScreen; i++)
        {
            float posZ;

            if (i > 0)
                posZ = lines[i - 1][3].transform.position.z + DeltaDistanceItems;
            else
                posZ = DeltaDistanceItems;

            //Добавляется линия
            lines.Add(CreateLineEmpty(posZ));

        }
    }

    /// <summary>
    /// Создание линии
    /// </summary>
    /// <param name="positionZ"></param>
    /// <returns></returns>
    public GameObject[] CreateLineEmpty(float positionZ)
    {
        GameObject[] line = new GameObject[gameManager.CountTrack + 1];
                
        line = FillLineEmpty(line);

        int rightPos = (int)Mathf.Floor(gameManager.CountTrack / 2);
        int leftPos = rightPos - gameManager.CountTrack + 1;

        for (int i = 0; i < line.Length; i++)
        {
            stepLine = leftPos * gameManager.WidthTrack + i * gameManager.WidthTrack;

            if (line[i] != null)
            {
                line[i].SetActive(true);

                line[i].transform.position = new Vector3(stepLine, 5, positionZ);
            }
        }

        return line;
    }

    /// <summary>
    /// Рандомное заполнение пустых линий
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private GameObject[] FillLineEmpty(GameObject[] line)
    {
        
        for (int i = 0; i < line.Length - 1; i++)
        {            
            line[i] = AddIteminLine(0, i);
        }

        line[line.Length - 1] = AddIteminLine(4);

        return line;
    }

    #endregion

    /// <summary>
    /// Стартовое расположение линий
    /// </summary>
    private void StartAddLine()
    {
        for (int i = 0; i < config.CountLineOnScreen; i++)
        {            
            float posZ;

            if (i > 0)
                posZ = lines[i - 1][3].transform.position.z + DeltaDistanceItems;
            else
                posZ = DeltaDistanceItems;

            //Добавляется линия
            lines.Add(CreateLine(posZ));

        }
    }

    /// <summary>
    /// Добавление линии
    /// </summary>
    private void AddLine()
    {       
        float posZ = lines[lines.Count - 1][3].transform.position.z + DeltaDistanceItems;

        lines.Add(CreateLine(posZ));
    }

    /// <summary>
    /// Очитска дороги от всех линий
    /// </summary>
    private void ClearRoadOfLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                lines[i][j].gameObject.SetActive(false);
            }
        }

        lines.Clear();
    }

    /// <summary>
    /// Рандомное заполнение линии
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private GameObject[] FillLine(GameObject[] line)
    {
        int minRand = 0;
        int maxRand = 4;

        int countEmptyInLine = 0;
        int countBlockInLine = 0;


        for (int i = 0; i < line.Length - 1; i++)
        {
            int index;

            if (lines.Count < 1)
            {
                index = 0;
            }
            else if (lines.Count < 6)
            {               
                maxRand = 3;  // без блоков
                index = Random.Range(minRand, maxRand);

                // если в линии уже есть два пустых места, то пустое место убирается из рандома
                if (countEmptyInLine == gameManager.CountTrack - 1 && index == 0)
                {
                    minRand = 1;
                }

                if (index == 0) countEmptyInLine++;
            }
            else
            {                
                int id = Random.Range(0, DifficultChance.Length);

                index = DifficultChance[id];

                // если в линии уже есть два пустых препятствия, то препятствие убирается из рандома
                if (countBlockInLine == gameManager.CountTrack - 1 && index == 3)
                {
                    maxRand = 3;
                    index = Random.Range(minRand, maxRand);
                }

                if (index == 0) countEmptyInLine++;
                if (index == 3) countBlockInLine++;
            }            

            line[i] = AddIteminLine(index, i);
        }

        line[line.Length - 1] = AddIteminLine(4);

        return line;
    }

    /// <summary>
    /// Создание линии
    /// </summary>
    /// <param name="positionZ"></param>
    /// <returns></returns>
    public GameObject[] CreateLine(float positionZ)
    {
        GameObject[] line = new GameObject[gameManager.CountTrack + 1];
                
        line = FillLine(line);

        int rightPos = (int)Mathf.Floor(gameManager.CountTrack / 2);
        int leftPos = rightPos - gameManager.CountTrack + 1;

        for (int i = 0; i < line.Length; i++)
        {
            stepLine = leftPos * gameManager.WidthTrack + i * gameManager.WidthTrack;

            if (line[i] != null)
            {
                line[i].SetActive(true);

                line[i].transform.position = new Vector3(stepLine, 5, positionZ);
            }
        }

        return line;
    }

    /// <summary>
    /// Добавление элемента в линию по индексу
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameObject AddIteminLine(int index, int numberTrack = 0)
    {
        if (index == 1)
        {
            return AddCoin().gameObject;
        }

        if (index == 2)
        {
            return AddGift().gameObject;
        }

        if (index == 3)
        {
            return AddBlock().gameObject;
        }

        if (index == 4)
        {
            return AddEmptyFlag().gameObject;
        }

        return AddEmpty().gameObject;
    }

    /// <summary>
    /// Добавление бонуса
    /// </summary>
    /// <returns></returns>
    private Coin AddCoin()
    {
        Coin newCoin = spawner.CoinOnScene[countCoin];
        countCoin++;

        // добавление в список движущихся предметов
        if (newCoin is IMove move) activationRoad.MoveObjects.Add(newCoin);

        if (countCoin == spawner.CoinOnScene.Count)
        {
            countCoin = 0;
        }

        return newCoin;
    }


    [SerializeField] private Gift[] startGifts;
    [SerializeField] private Transform parentStartGift;
    private int startCountGift = 0;
    private int startColorGift;    

    /// <summary>
    /// Добавление подарка
    /// </summary>
    /// <returns></returns>
    private Gift AddGift()
    {
        Gift newGift;

        if (startCountGift == 0 || startCountGift > 2)
        {           
            newGift = spawner.GiftOnScene[countGift];

            countGift++;


            // добавление в список движущихся предметов
            if (newGift is IMove move) activationRoad.MoveObjects.Add(newGift);


            if (countGift == spawner.GiftOnScene.Count)
            {
                countGift = 0;
            }

            if (startCountGift == 0)
            {
                startColorGift = newGift.ColorGift;
            }
        }
        else
        {
            newGift = diContainer.InstantiatePrefabForComponent<Gift>(startGifts[startColorGift - 1], Vector3.zero, Quaternion.identity, parentStartGift);

            // добавление в список движущихся предметов
            if (newGift is IMove move) activationRoad.MoveObjects.Add(newGift);
        }

        if(startCountGift < 5)
        {
            startCountGift++;
        }

        return newGift;
    }    

    /// <summary>
    /// Добавление пустоты
    /// </summary>
    /// <returns></returns>
    private Empty AddEmpty()
    {
        Empty newEmpty = spawner.EmptyOnScene[countEmpty];
        newEmpty.transform.localScale = new Vector3(gameManager.WidthTrack / 10, gameManager.WidthTrack / 10, newEmpty.transform.localScale.z);
        countEmpty++;

        // добавление в список движущихся предметов
        if (newEmpty is IMove move) activationRoad.MoveObjects.Add(newEmpty);

        if (countEmpty == spawner.EmptyOnScene.Count)
        {
            countEmpty = 0;
        }

        return newEmpty;
    }

    private Empty AddEmptyFlag()
    {
        Empty newEmpty = spawner.EmptyFlagOnScene[countEmptyFlag];
        newEmpty.transform.localScale = new Vector3(gameManager.WidthTrack / 10, gameManager.WidthTrack / 10, newEmpty.transform.localScale.z);
        countEmptyFlag++;

        // добавление в список движущихся предметов
        if (newEmpty is IMove move) activationRoad.MoveObjects.Add(newEmpty);

        if (countEmptyFlag == spawner.EmptyFlagOnScene.Count)
        {
            countEmptyFlag = 0;
        }

        return newEmpty;
    }

    /// <summary>
    /// Удаление линии
    /// </summary>
    private void Deactivate()
    {       
        lines.RemoveAt(0);
    }

    #region AddBlock

    int[] allChanceBlock = new int[10];

    /// <summary>
    /// Количество блоков в текущей локации
    /// </summary>
    /// <returns></returns>
    private int QtrBlocInLOcation()
    {
        return locationSettings.Locations[activationRoad.CurrentLocation].LocationBlocks.Count;
    }

    /// <summary>
    /// Добавление препятствия
    /// </summary>
    /// <returns></returns>
    private Block AddBlock()
    {
        int indexBlockScene = 0;
                
        if (QtrBlocInLOcation() > 1)
        {
            int idRandomBlockChance = Random.Range(0, allChanceBlock.Length);

            indexBlockScene = allChanceBlock[idRandomBlockChance];
        }
        else
        {
            indexBlockScene = locationSettings.Locations[activationRoad.CurrentLocation].LocationBlocks[0].Id;
        }

        Block newBlock = spawner.BlockOnScene[indexBlockScene].Blocks[countBlock[indexBlockScene]];
        newBlock.transform.localScale = new Vector3(gameManager.WidthTrack / 10, newBlock.transform.localScale.y, newBlock.transform.localScale.z);

        countBlock[indexBlockScene]++;

        // добавление в список движущихся предметов
        if (newBlock is IMove move) activationRoad.MoveObjects.Add(newBlock);


        if (countBlock[indexBlockScene] == spawner.BlockOnScene[indexBlockScene].Blocks.Count)
        {
            countBlock[indexBlockScene] = 0;
        }

        return newBlock;
    }

    /// <summary>
    /// Заполнение массива вероятностей блоков для текущей локации
    /// </summary>
    /// <returns></returns>
    public void FillChanceBlock()
    {       
        if(QtrBlocInLOcation() > 1)
        {
            int qtrBlockInLoc = QtrBlocInLOcation();

            if (qtrBlockInLoc > 1)
            {
                int qtrFillChance = 0;

                for (int i = 0; i < qtrBlockInLoc; i++)
                {
                    int valueChanceBlock = locationSettings.Locations[activationRoad.CurrentLocation].LocationBlocks[i].Chance;
                    int idBlockScene = locationSettings.Locations[activationRoad.CurrentLocation].LocationBlocks[i].Id;

                    for (int j = 0; j < valueChanceBlock; j++)
                    {
                        if (qtrFillChance == allChanceBlock.Length)
                        {
                            Debug.LogWarning("Привышен массиы вероятности выбора блоков 2");
                            break;
                        }

                        allChanceBlock[qtrFillChance] = idBlockScene;

                        qtrFillChance++;
                    }
                }
            }
        }        
    }

    #endregion
}
