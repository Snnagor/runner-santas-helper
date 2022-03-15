using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Configs/Config")]
public class Config : ScriptableObject
{
    [Header("Score Settings")]

    [SerializeField] private int scoreMeter;
    public int ScoreMeter { get => scoreMeter; }

    [SerializeField] private int scoreBonus;
    public int ScoreBonus { get => scoreBonus; }

    [Header("Player Settings")]

    [SerializeField] private int startSpeedPlayer;
    public int StartSpeedPlayer { get => startSpeedPlayer; }

    [SerializeField] private int speedSidestepPlayer;
    public int SpeedSidestepPlayer { get => speedSidestepPlayer; }

    [SerializeField] private int accelerationDistance;
    public int AccelerationDistance { get => accelerationDistance; }

    [SerializeField] private int acceleration;
    public int Acceleration { get => acceleration; }

    [SerializeField] private float speedAnimation;
    public float SpeedAnimation { get => speedAnimation; }

    [SerializeField] private float deltaSwipeMin;
    public float DeltaSwipeMin { get => deltaSwipeMin; }

   
    [Header("Road Settings")]

    [SerializeField] private int countTrack;
    public int CountTrack { get => countTrack; }

    [SerializeField] private int widthTrack;
    public int WidthTrack { get => widthTrack; }

    [SerializeField] private int minCountTrack;
    public int MinCountTrack { get => minCountTrack; }

    [SerializeField] private int maxCountTrack;
    public int MaxCountTrack { get => maxCountTrack; }

    [SerializeField] private float minWidthTrack;
    public float MinWidthTrack { get => minWidthTrack; }

    [SerializeField] private float maxWidthTrack;
    public float MaxWidthTrack { get => maxWidthTrack; }

    [SerializeField] private float startPositionRoad;
    public float StartPositionRoad { get => startPositionRoad; }

    [SerializeField] private float stepPositionRoad;
    public float StepPositionRoad { get => stepPositionRoad; }    


    [Header("Items Settings")]

    [SerializeField] private int deltaDistanceItems;
    public int DeltaDistanceItems { get => deltaDistanceItems; }

    [SerializeField] private int countLineOnScreen;
    public int CountLineOnScreen { get => countLineOnScreen; }

    [Header("Camera Settings")]

    [SerializeField] private int speedCamera;
    public int SpeedCamera { get => speedCamera; }


    [Header("SceneManager Settings")]

    [SerializeField] private bool restart;
    public bool Restart { get => restart; set => restart = value; }
    

    [Header("Bonus Settings")]

    [SerializeField] private int[] chanceBonus;

    public int[] ChanceBonus { get => chanceBonus; set => chanceBonus = value; }

}
