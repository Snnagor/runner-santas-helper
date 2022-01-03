using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItems : MonoBehaviour
{   
    [SerializeField] private Transform meshGift;

    private void Update()
    {        
        meshGift.Rotate(Vector3.up * -150f * Time.deltaTime);
    }
}
