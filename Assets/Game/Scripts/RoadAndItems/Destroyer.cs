using UnityEngine;
using Zenject;

public class Destroyer : MonoBehaviour
{    

    #region Injects

    private SignalBus signalBus;
    private ActivatorRoad activatorRoad;

    [Inject]
    private void Construct(SignalBus _signalBus, ActivatorRoad _activatorRoad)
    {        
        signalBus = _signalBus;
        activatorRoad = _activatorRoad;
    }

    #endregion        

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flag"))
        {            
            signalBus.Fire(new CreateLineSignal());
        }

        if (!other.CompareTag("Road"))
        {
            other.gameObject.SetActive(false);            
            other.gameObject.transform.position = Vector3.zero;

            //if (other.CompareTag("Gift"))
            //{
                
            //   var coin = other.GetComponent<Gift>();      
            //    if (coin is IMove) activatorRoad.MoveObjects.Remove(coin);
            //}

            //if (other.CompareTag("Empty"))
            //{

            //    var coin = other.GetComponent<Empty>();
            //    if (coin is IMove) activatorRoad.MoveObjects.Remove(coin);
            //}

            //if (other.CompareTag("Flag"))
            //{

            //    var coin = other.GetComponent<Empty>();
            //    if (coin is IMove) activatorRoad.MoveObjects.Remove(coin);
            //}

            //if (other.CompareTag("Block"))
            //{

            //    var coin = other.GetComponent<Block>();
            //    if (coin is IMove) activatorRoad.MoveObjects.Remove(coin);
            //}

            //if (other.CompareTag("Coin"))
            //{

            //    var coin = other.GetComponent<Coin>();
            //    if (coin is IMove) activatorRoad.MoveObjects.Remove(coin);
            //}

        }

        
    }

    
}
