using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Sled : MonoBehaviour
{    
    public float DeltaSpeed { get; set; }

    #region Injects

    private SoundManager soundManager;

    [Inject]
    private void Construct(SoundManager _soundManager)
    {
        soundManager = _soundManager;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {        
        HitBlock(other);
    }

    private void HitBlock(Collider other)
    {       
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Deactivator block = other.GetComponent<Deactivator>();
            if (rb != null && block != null)
            {               
                rb.isKinematic = false;

                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized ;

                float dirX = Random.Range(-0.5f, 0.5f);
                float dirY = Random.Range(0.3f, 0.6f);

                rb.AddForce(new Vector3(dirX, dirY, 1f) * (40f + DeltaSpeed/3), ForceMode.Impulse);
                block.HitBlockBool = true;
            }        
    }
}
