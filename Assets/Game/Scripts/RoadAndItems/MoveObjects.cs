using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class MoveObjects : MonoBehaviour, IMove
{    
    protected SignalBus signalBus;
    protected GameManager gameManager;
    private Config config;
    protected MagnetBonus magnetBonus;
    private ActivatorRoad activationRoad;

    #region Injects

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           GameManager _gameManager,
                           MagnetBonus _magnetBonus,
                           ActivatorRoad _activationRoad)
    {                
        signalBus = _signalBus;
        gameManager = _gameManager;
        config = _config;
        magnetBonus = _magnetBonus;
        activationRoad = _activationRoad;
    }

    #endregion            

    

    //Движение объекта
    public virtual void Move()
    {
        if (gameManager.IsRun && gameObject.activeSelf)
        {           
             Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
             transform.position = Vector3.MoveTowards(transform.position, target, gameManager.CurrentSpeed * Time.deltaTime);
        }
    }


    //private  void Update()
    //{
    //    Move();
    //}

    public virtual void Execute()
    {
        Move();
    }

    private void OnDisable()
    {
        activationRoad.MoveObjects.Remove(this);
    }

    private void OnDestroy()
    {
        activationRoad.MoveObjects.Remove(this);
    }
}
