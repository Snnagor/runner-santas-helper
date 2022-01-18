using System.Collections;
using UnityEngine;
using Zenject;

public class SledBonus : MonoBehaviour, IBonus
{    
    [SerializeField] private Sprite iconBonus;

    [SerializeField] private bool haveTimer;
    public bool HaveTimer { get => haveTimer;}

    [SerializeField] private GameObject prefabFlyBonus;
    public GameObject PrefabFlyBonus => prefabFlyBonus;

    public Sprite IconBonuse { get => iconBonus; }

    [SerializeField] private Transform player;
    [SerializeField] private Transform gifts;
    [SerializeField] private float timeBonus;
    [SerializeField] private Sled sled;   
    [SerializeField] private float speedSledMultipl;


    public bool Enable { get; set; }
    public float TimeBonus { get => timeBonus; set => timeBonus = value; }
    public float CurrentTimeBonus { get; set; } = -2;
  
    private float oldYPosGifts, oldZPosGifts;

    #region Injects

    private GameManager gameManager;
    private RunnerMove runnerMove;
    private SignalBus signalBus;
    private RunnerGifts runnerGifts;
    private RunnerAnimation runnerAnim;    

    [Inject]
    private void Construct(GameManager _gameManager, 
                           SignalBus _signalBus, 
                           RunnerMove _runnerMove,
                           RunnerGifts _runnerGifts,
                           RunnerAnimation _runnerAnim)
    {
        gameManager = _gameManager;
        signalBus = _signalBus;
        runnerMove = _runnerMove;
        runnerGifts = _runnerGifts;
        runnerAnim = _runnerAnim;        
    }

    #endregion
       
    /// <summary>
    /// Запуск бонуса
    /// </summary>
    public void Init()
    {
        if (!Enable)
        {
            Enable = true;

            gameManager.IsSled = true;

            sled.gameObject.SetActive(true);
            sled.DeltaSpeed = gameManager.Speed;

            gameManager.CurrentSpeed *= speedSledMultipl;

            CurrentTimeBonus = timeBonus;

            runnerMove.CollisionBlock = false;
            runnerMove.AnimRun = false;
            runnerMove.AnimBox = false;

            runnerAnim.AnimSled(true);

            player.localPosition = new Vector3(player.localPosition.x, player.localPosition.y + 3, player.localPosition.z);
            player.localRotation = Quaternion.Euler(Vector3.zero);
            sled.transform.localRotation = Quaternion.Euler(Vector3.zero);


            oldYPosGifts = gifts.localPosition.y;
            oldZPosGifts = gifts.localPosition.z;
            gifts.localPosition = new Vector3(gifts.localPosition.x, -0.69f, 0.97f);
            gifts.localEulerAngles = new Vector3(45, 0, 0);

            runnerMove.MigEnable(false);

        }
    }       

    /// <summary>
    /// Сброс времени при повторном появлении бонуса
    /// </summary>
    public void ResetTimer()
    {
        if (Enable)
        {
            CurrentTimeBonus = timeBonus;
        }
    }

    /// <summary>
    /// Выключение бонуса
    /// </summary>
    public void Disable()
    {
        Enable = false;

        gameManager.IsSled = false;

        sled.gameObject.SetActive(false);

        gameManager.CurrentSpeed = gameManager.Speed;

        StartCoroutine(ActivateCollisionBlock());

        runnerMove.AnimRun = true;
        runnerMove.AnimBox = true;

        runnerAnim.AnimSled(false);

        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.Euler(Vector3.zero);

        gifts.localEulerAngles = Vector3.zero;
        gifts.localPosition = new Vector3(gifts.localPosition.x, oldYPosGifts, oldZPosGifts);
    }

    /// <summary>
    /// Активация колизии игрока
    /// </summary>
    /// <returns></returns>
    IEnumerator ActivateCollisionBlock()
    {
        runnerMove.MigEnable(true);

        yield return new WaitForSeconds(3f);

        if (!Enable)
        {
            runnerMove.CollisionBlock = true;
            runnerMove.MigEnable(false);
        }            
    }

    /// <summary>
    /// Считчик времени действия бонуса
    /// </summary>
    public void CounterTimeBonus()
    {
        if (Enable)
        {
            if (CurrentTimeBonus > 0)
            {
                CurrentTimeBonus -= Time.deltaTime;
            }
            else if (CurrentTimeBonus > -1)
            {
                Disable();
                CurrentTimeBonus = -2;
            }
        }
       
    }

    public void Execute()
    {
        CounterTimeBonus();

        if (Enable)
        {
            player.Rotate(Vector3.up * 50f * Time.deltaTime);
            sled.transform.Rotate(Vector3.up * 50f * Time.deltaTime);
        }       
    }

}
