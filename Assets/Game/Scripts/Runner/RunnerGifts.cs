using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(RunnerMove))]
[RequireComponent(typeof(RunnerAnimation))]
public class RunnerGifts : MonoBehaviour
{
    [SerializeField] private GiftInHands[] gifts;
    [SerializeField] private FallGift fallGiftPrefab;
    [SerializeField] private FallGiftLose fallGiftLosePrefab;
    [SerializeField] private Material[] matBox;
    [SerializeField] private Material[] matTape;
    [SerializeField] private ParticleSystem[] particalExplosion;
   
    private RunnerAnimation runnerAnim;    

    private int[] arrayColor;

    private bool hitTopBlock = false;

    public int CountActiveGift { get; set;} = 0;
    public Vector3 PosBonusObject { get; set; }

    #region Injects

    private GameManager gameManager;
    private SignalBus signalBus;
    private DiContainer diContainer;
    private SoundManager soundManager;

    [Inject]
    private void Construct(GameManager _gameManager, 
                           SignalBus _signalBus, 
                           DiContainer _diContainer, 
                           SoundManager _soundManager)
    {       
        gameManager = _gameManager;
        signalBus = _signalBus;
        diContainer = _diContainer;
        soundManager = _soundManager;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<HitGiftTopBlockSignal>(HitGiftTopBlockSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<HitGiftTopBlockSignal>(HitGiftTopBlockSignal);
    }

    private void HitGiftTopBlockSignal()
    {
        if (!hitTopBlock)
        {
            hitTopBlock = true;   // ограничение количетва сигналов
            SearchHitGift();
        }
    }

    #endregion

    private void Awake()
    {
        runnerAnim = GetComponent<RunnerAnimation>();
    }

    private void Start()
    {
        arrayColor = new int[gifts.Length];
    }

    #region Подбор подарков

    /// <summary>
    /// Активация подарков в руках
    /// </summary>
    /// <param name="pos">Позиция подобронрого подарка</param>
    /// <param name="colorGift">Цвет подобранного подарка</param>
    public void ActiveGift(Vector3 pos, int colorGift)
    {
        if(CountActiveGift < gifts.Length)
        {
            CountActiveGift++;

            // Записываем в массив
            arrayColor[CountActiveGift - 1] = colorGift;

            // Стваим позицию
            if (CountActiveGift > 1)
            {
                gifts[CountActiveGift - 1].transform.position = new Vector3(pos.x, pos.y + CountActiveGift, pos.z + 5f);

                int rotY = Random.Range(-70, 70);
                gifts[CountActiveGift - 1].transform.rotation = Quaternion.Euler(new Vector3(gifts[CountActiveGift - 2].transform.rotation.x, (float) rotY, gifts[CountActiveGift - 2].transform.rotation.z));
            }

            //Меняем материал
            ChangeMaterial(colorGift);

            // Делаем активным
            gifts[CountActiveGift - 1].gameObject.SetActive(true);

            //Проверяем предыдущие цвета
            if (CountActiveGift > 2 && arrayColor[CountActiveGift - 1] == arrayColor[CountActiveGift - 2] && arrayColor[CountActiveGift - 1] == arrayColor[CountActiveGift - 3])
            {                
                StartCoroutine(DeleteCoroutine(3));
            }                        
        }
        else
        {
            if (arrayColor[gifts.Length - 1] == arrayColor[gifts.Length - 2] && arrayColor[gifts.Length - 1] == colorGift)
            {                
                StartCoroutine(DeleteCoroutine(2));
            }
        }
    }

    /// <summary>
    /// Изменение материала подарков
    /// </summary>
    /// <param name="colorGift"></param>
    private void ChangeMaterial(int colorGift)
    {      

        gifts[CountActiveGift - 1].ChangeMaterial(matBox[colorGift-1], matTape[colorGift-1]);
    }

    IEnumerator DeleteCoroutine(int qtyDel)
    {
        yield return new WaitForSeconds(0.15f);
        DeleteGiftOneColor(qtyDel);
    }

    /// <summary>
    /// Удаление одинаковых подарков
    /// </summary>
    private void DeleteGiftOneColor(int qtyDel)
    {        
        for (int i = 1; i <= qtyDel; i++)
        {
            if (CountActiveGift - i >= 0)
            {
                particalExplosion[CountActiveGift - i].transform.position = gifts[CountActiveGift - i].transform.position;
                particalExplosion[CountActiveGift - i].Play();
                gifts[CountActiveGift - i].gameObject.SetActive(false);
                gifts[CountActiveGift - i].transform.rotation = Quaternion.Euler(Vector3.zero);
                arrayColor[CountActiveGift - i] = 0;

                if(i == 2)
                {
                    PosBonusObject = gifts[CountActiveGift - i].transform.position;                    
                }

                //if (i == 3)
                //{
                //    soundManager.Gift3Line();
                //}
            }
        }

        TakeBonus();

        CountActiveGift -= 3;

        if (CountActiveGift < 0)
            CountActiveGift = 0;

        if (CountActiveGift == 0)
            runnerAnim.AnimRunBox(false);
            
    }

    #endregion

    #region Падение с подарками

    /// <summary>
    /// Поведение подарков при врезании
    /// </summary>
    public void Lose()
    {       
        int counter = 0;
        Vector3 direction;

        foreach (var n in gifts)
        {
            if (n.gameObject.activeSelf)
            {
                #region Joint

                ConfigurableJoint joint = n.GetComponent<ConfigurableJoint>();
                if(joint != null)
                joint.enableCollision = true;
                Destroy(joint);

                #endregion

                #region Rigidbody

                Rigidbody rb = n.GetComponent<Rigidbody>();

                if (counter != 0)
                {
                    rb.drag = 0;
                    rb.angularDrag = 0;
                    rb.mass = 10;

                    direction = Vector3.forward * 5 * gameManager.Speed;
                }
                else
                {
                    rb.isKinematic = false;
                    direction = Vector3.back  * 2;
                }

                Physics.gravity = new Vector3(0, -100f, 0);

                rb.AddForce(direction, ForceMode.Impulse);

                #endregion
            }

            counter++;
        }
    }

    #endregion

    #region Возобновение игры с подарками

    /// <summary>
    /// Поведение подарков при возобновении игры
    /// </summary>
    public void Continue()
    {      
        for (int i = 0; i < CountActiveGift; i++)
        {            
            var newFallGift = diContainer.InstantiatePrefabForComponent<FallGiftLose>(fallGiftLosePrefab, gifts[i].transform.position, Quaternion.identity, null);
            newFallGift.ChangeMat(gifts[i].BoxMesh.material, gifts[i].TapeMesh.material);

            gifts[i].HitBlock = false;
            gifts[i].gameObject.SetActive(false);
            gifts[i].transform.rotation = Quaternion.Euler(Vector3.zero);
            arrayColor[i] = 0;
           // CountActiveGift--;
        }

        CountActiveGift = 0;
    }

    #endregion

    #region Сбивание подарков

    /// <summary>
    /// Поиск коробок которые ударились
    /// </summary>
    private void SearchHitGift()
    {
        bool tmpBottomHitGift = false;

        for (int i = 0; i < gifts.Length; i++)
        {
            if (gifts[i].HitBlock && !tmpBottomHitGift && gifts[i].gameObject.activeSelf)
            {
                tmpBottomHitGift = true;
                HitGift(i);
            }

            if (tmpBottomHitGift && gifts[i].gameObject.activeSelf)
            {
                HitGift(i);
            }
        }

        hitTopBlock = false;
    }

    /// <summary>
    /// Действия над сбитыми подарками
    /// </summary>
    /// <param name="indexGift"></param>
    private void HitGift(int indexGift)
    {
        var newFallGift = diContainer.InstantiatePrefabForComponent<FallGift>(fallGiftPrefab, gifts[indexGift].transform.position, Quaternion.identity, null);
        newFallGift.ChangeMat(gifts[indexGift].BoxMesh.material, gifts[indexGift].TapeMesh.material);

        gifts[indexGift].HitBlock = false;
        gifts[indexGift].gameObject.SetActive(false);
        gifts[indexGift].transform.rotation = Quaternion.Euler(Vector3.zero);
        arrayColor[indexGift] = 0;
        CountActiveGift--;
    }

    #endregion

    #region Бонусы

    /// <summary>
    /// Взяли бонус
    /// </summary>
    private void TakeBonus()
    {        
        signalBus.Fire(new TakeBonusSignal());
    }

    #endregion
}
