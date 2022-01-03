using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MoveObjects : MonoBehaviour
{    
    protected SignalBus signalBus;
    protected GameManager gameManager;
    private Config config;
    protected MagnetBonus magnetBonus;

    #region Injects

    [Inject]
    private void Construct(Config _config, 
                           SignalBus _signalBus, 
                           GameManager _gameManager,
                           MagnetBonus _magnetBonus)
    {                
        signalBus = _signalBus;
        gameManager = _gameManager;
        config = _config;
        magnetBonus = _magnetBonus;
    }

    #endregion            

    private void Awake()
    {
       
    }

    //Движение объекта
    public virtual void Move()
    {
        if (gameManager.IsRun && gameObject.activeSelf)
        {           
             Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
             transform.position = Vector3.MoveTowards(transform.position, target, gameManager.CurrentSpeed * Time.deltaTime);
        }
    }


    private  void Update()
    {
        Move();
    }
}
