using System.Collections.Generic;
using UnityEngine;

public class Road : MoveObjects
{
    [SerializeField] private Transform leftGround;
    public Transform LeftGround { get => leftGround; set => leftGround = value; }

    [SerializeField] private Transform rightGround;
    public Transform RightGround { get => rightGround; set => rightGround = value; }

    [SerializeField] private Transform leftTrack;
    public Transform LeftTrack { get => leftTrack; set => leftTrack = value; }

    [SerializeField] private Transform rightTrack;
    public Transform RightTrack { get => rightTrack; set => rightTrack = value; }

    [SerializeField] private Transform track;
    public Transform Track { get => track; set => track = value; }

    [SerializeField] private int countCreateTrack = 10;

    private float widthTrack;    
    private int rightPos;
    private int leftPos;

    private float startXPostionLeftGround, startXPostionRightGround, startWidthTrack = 10;    

    private int countTrackFromLeft;    

    private List<Transform> createdTracks = new List<Transform>();    

    //Флаг активации следующей дороги
    public bool Begin { get; set; } = false;


    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<ChangeCountTrackSignal>(ChangeCountTrack);
        signalBus.Subscribe<ChangeWidthTrackSignal>(ChangeWidthTrack);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<ChangeCountTrackSignal>(ChangeCountTrack);
        signalBus.Unsubscribe<ChangeWidthTrackSignal>(ChangeWidthTrack);
    }
    #endregion   

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Begin = true;
        }
    }

    private void Awake()
    {
        CreateTrack();
    }

    private void Start()
    {
        startXPostionLeftGround = leftGround.position.x;
        startXPostionRightGround = rightGround.position.x;
        ChangeCountTrack();
    }

    /// <summary>
    /// Изменение количества дорожек
    /// </summary>
    private void ChangeCountTrack()
    {
        ChangeWidthTrack();

        DeactivateAllTracks();

        if (gameManager.CountTrack > 3)
        {
            countTrackFromLeft = 2;

            ActivateTrack(gameManager.CountTrack);
        }

        if (gameManager.CountTrack == 3)
        {
            track.gameObject.SetActive(true);
        }

        if (gameManager.CountTrack == 2)
        {
            track.gameObject.SetActive(false);
        }

        
    }

    /// <summary>
    /// Изменение ширины дорожек
    /// </summary>
    private void ChangeWidthTrack()
    {      
        rightPos = (int) Mathf.Floor(gameManager.CountTrack / 2);
        leftPos = rightPos - gameManager.CountTrack + 1;

        widthTrack = gameManager.WidthTrack / 10;

        WidthTrack();
        PositionTrack();
        PositionGround();
        ChangeWidthActivateTracks();
    }

    /// <summary>
    /// Создание дорожек
    /// </summary>
    private void CreateTrack()
    {
        for (int i = 0; i < countCreateTrack; i++)
        {
            Transform newTrack = Instantiate(track, Vector3.zero, Quaternion.identity, transform);

            createdTracks.Add(newTrack);
        }
    }

    /// <summary>
    /// Активация дорожек
    /// </summary>
    /// <param name="count"></param>
    private void ActivateTrack(int count)
    {
        int addCount = count - 3;

        for (int i = 0; i < addCount; i++)
        {       
            createdTracks[countTrackFromLeft + i - 2].localScale = new Vector3(widthTrack, createdTracks[countTrackFromLeft + i - 2].localScale.y, createdTracks[countTrackFromLeft + i - 2].localScale.z);

            createdTracks[countTrackFromLeft + i - 2].position = new Vector3(gameManager.WidthTrack * (leftPos + countTrackFromLeft + i), track.position.y, track.position.z);

            createdTracks[countTrackFromLeft + i - 2].gameObject.SetActive(true);

            //countTrackFromLeft++;
        }
    }

    /// <summary>
    /// Деактивация всех дорожек
    /// </summary>
    private void DeactivateAllTracks()
    {
        for (int i = 0; i < createdTracks.Count; i++)
        {
            createdTracks[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Изменение ширины добавленных дорожек
    /// </summary>
    private void ChangeWidthActivateTracks()
    {
        for (int i = 0; i < createdTracks.Count; i++)
        {
            if (createdTracks[i].gameObject.activeSelf) 
            {
                createdTracks[i].localScale = new Vector3(widthTrack, createdTracks[i].localScale.y, createdTracks[i].localScale.z);
                createdTracks[i].position = new Vector3(gameManager.WidthTrack * (leftPos + countTrackFromLeft + i), track.position.y, track.position.z);
            }              
        }
    }

    /// <summary>
    /// Изменение ширины дорожек по умолчанию
    /// </summary>
    private void WidthTrack()
    {
        leftTrack.localScale = new Vector3(widthTrack, leftTrack.localScale.y, leftTrack.localScale.z);
        rightTrack.localScale = new Vector3(widthTrack, rightTrack.localScale.y, rightTrack.localScale.z);
        track.localScale = new Vector3(widthTrack, track.localScale.y, track.localScale.z);
    }
    
    /// <summary>
    ///  Позиция дорожек по умолчанию
    /// </summary>
    private void PositionTrack()
    {       
        leftTrack.position = new Vector3(gameManager.WidthTrack * leftPos, leftTrack.position.y, leftTrack.position.z);
        rightTrack.position = new Vector3(gameManager.WidthTrack * rightPos, rightTrack.position.y, rightTrack.position.z);
        track.position = new Vector3(gameManager.WidthTrack * leftPos + gameManager.WidthTrack, track.position.y, track.position.z);
    }

    /// <summary>
    /// Позиция объектов на фоне
    /// </summary>
    private void PositionGround()
    {        
        float deltaWidthTrack = gameManager.WidthTrack - startWidthTrack;

        float posXLeft = startXPostionLeftGround - deltaWidthTrack + gameManager.WidthTrack * (leftPos + 1); 
        float posXRight = startXPostionRightGround + deltaWidthTrack + gameManager.WidthTrack * (rightPos - 1);

        leftGround.position = new Vector3(posXLeft, leftGround.position.y, leftGround.position.z);
        rightGround.position = new Vector3(posXRight, rightGround.position.y, rightGround.position.z);
    }

}
