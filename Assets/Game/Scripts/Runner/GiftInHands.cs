using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GiftInHands : MonoBehaviour
{
    [SerializeField] private MeshRenderer boxMesh;
    [SerializeField] private MeshRenderer tapeMesh;

    public MeshRenderer BoxMesh => boxMesh;
    public MeshRenderer TapeMesh => tapeMesh;

    public bool HitBlock { get; set; } = false;

    private SignalBus signalBus;    
   

    #region Injects

    [Inject]
    private void Construct(SignalBus _signalBus)
    {
        signalBus = _signalBus;        
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TopBlock"))
        {            
            HitBlock = true;

            signalBus.Fire(new HitGiftTopBlockSignal());            
        }
    }

    public void ChangeMaterial(Material _matBox, Material _matTape)
    {
        if(boxMesh != null && tapeMesh != null)
        {
            boxMesh.material = _matBox;
            tapeMesh.material = _matTape;
        }
    }
}
