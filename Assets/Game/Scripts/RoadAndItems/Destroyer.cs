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
        }
    }
}
