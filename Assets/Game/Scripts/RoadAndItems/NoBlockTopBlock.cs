using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBlockTopBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {       
        if (other.CompareTag("Block"))
        {           
            other.gameObject.SetActive(false);
            other.transform.position = Vector3.zero;
        }        
    }
}
