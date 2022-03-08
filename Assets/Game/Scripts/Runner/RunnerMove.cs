using UnityEngine;
using Zenject;

[RequireComponent(typeof(RunnerGifts))]
[RequireComponent(typeof(RunnerAnimation))]
public class RunnerMove : MonoBehaviour
{
    [SerializeField] private ParticleSystem snowBoom;
    [SerializeField] private ParticleSystem blockBoom;
    [SerializeField] private ParticleSystem takeCoinFX;

    [Header("SHaderMig")]
    [SerializeField] private SkinnedMeshRenderer[] playersMeshes;
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material migMaterial;

    public bool CollisionBlock { get; set; } = true;
    public bool AnimRun { get; set; } = true;
    public bool AnimBox { get; set; } = true;

    private RunnerAnimation runnerAnim;
    private float directionX = 0;

    private RunnerGifts runnerGifts;

    #region Injects

    private SignalBus signalBus;
    private InputHandler input;
    private GameManager gameManager;
    private Config config;
    private LocationsSettings locationSettings;
    private SoundManager soundManager;
    private ActivatorRoad activatorRoad;

    [Inject]
    private void Construct(Config _config,
                           SignalBus _signalBus,
                           InputHandler _input,
                           GameManager _gameManager,
                           SoundManager _soundManager,
                           ActivatorRoad _activatorRoad)
    {
        config = _config;
        signalBus = _signalBus;
        input = _input;
        gameManager = _gameManager;
        soundManager = _soundManager;
        activatorRoad = _activatorRoad;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<ContinueSignal>(ContinueSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<ContinueSignal>(ContinueSignal);
    }

    private void ContinueSignal()
    {
        snowBoom.gameObject.SetActive(false);
        runnerAnim.AnimGetUp();
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        TakeCoin(other);
        TakeGift(other);

        if (CollisionBlock)
        {
            Lose(other);
        }
    }

    private void Awake()
    {
        runnerGifts = GetComponent<RunnerGifts>();
        runnerAnim = GetComponent<RunnerAnimation>();
    }

    public void MigEnable(bool value)
    {
        if (value)
        {
            for (int i = 0; i < playersMeshes.Length; i++)
            {
                playersMeshes[i].material = migMaterial;
            }
        }
        else
        {
            for (int i = 0; i < playersMeshes.Length; i++)
            {
                playersMeshes[i].material = mainMaterial;
            }
        }
    }

    /// <summary>
    /// Движение игрока
    /// </summary>
    private void Run()
    {
        if (gameManager.IsRun)
        {
            if (AnimRun)
                runnerAnim.AnimRun(true);

            Vector3 target = new Vector3(Sidestep(), transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, gameManager.SpeedSidestep * Time.deltaTime);
        }
    }

    /// <summary>
    /// Шаг в сторону
    /// </summary>
    /// <returns></returns>
    private float Sidestep()
    {
        if (input.IsInput() && gameManager.IsRun)
        {
            directionX = input.InputX();

            directionX *= gameManager.WidthTrack;

            // soundManager.SlideStep();
        }

        return directionX;
    }

    /// <summary>
    /// Взять бонус
    /// </summary>
    /// <param name="other"></param>
    private void TakeCoin(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            takeCoinFX.Play();
            signalBus.Fire(new TakeCoinSignal());
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Взять подарок
    /// </summary>
    /// <param name="other"></param>
    private void TakeGift(Collider other)
    {
        if (other.gameObject.CompareTag("Gift"))
        {
            soundManager.TakeGift();

            int colorIndexGift = 0;

            Gift takenGift = other.GetComponent<Gift>();
            if (takenGift)
            {
                colorIndexGift = takenGift.ColorGift;
            }

            runnerGifts.ActiveGift(other.transform.position, colorIndexGift);

            if (runnerGifts.CountActiveGift > 0 && AnimBox)
            {
                runnerAnim.AnimRunBox(true);
            }

            other.gameObject.SetActive(false);

        }
    }


    /// <summary>
    /// Врезался в препятствие
    /// </summary>
    /// <param name="other"></param>
    private void Lose(Collider other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            soundManager.HitBlock();

            if (runnerGifts.CountActiveGift > 0)
                soundManager.HitBlockGifts();

            // runnerGifts.Lose();
            runnerGifts.Continue();
            blockBoom.Play();
            runnerAnim.AnimLose();
            signalBus.Fire(new LoseSignal());
        }

        //if (other.gameObject.CompareTag("Snowman"))
        //{
        //    soundManager.HitSnowman();

        //    if (runnerGifts.CountActiveGift > 0)
        //        soundManager.HitBlockGifts();

        //    //runnerGifts.Lose();
        //    runnerGifts.Continue();

        //    if (!snowBoom.gameObject.activeSelf)
        //    {
        //        snowBoom.gameObject.SetActive(true);
        //    }
        //    snowBoom.Play();
        //    runnerAnim.AnimLose();
        //    signalBus.Fire(new LoseSignal());
        //}
    }

    private void Update()
    {
        Run();
    }
}
