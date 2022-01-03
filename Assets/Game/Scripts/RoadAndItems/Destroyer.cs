using UnityEngine;
using Zenject;

public class Destroyer : MonoBehaviour
{
    #region Injects

    private SignalBus signalBus;

    [Inject]
    private void Construct(SignalBus _signalBus)
    {        
        signalBus = _signalBus;       
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
