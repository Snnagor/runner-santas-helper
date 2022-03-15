using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGift : MoveObjects
{
    [SerializeField] private MeshRenderer box;
    [SerializeField] private MeshRenderer tape;
   
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();       
    }

    private void OnEnable()
    {
        float dirX = Random.Range(-1.1f, 1.1f);

        Vector3 direction = new Vector3(dirX, -0.05f, 0);

        rb.AddForce(direction * 40f, ForceMode.Impulse);

        Destroy(gameObject, 1f);
    }

    public void ChangeMat(Material _matBox, Material _matTape)
    {
        box.material = _matBox;
        tape.material = _matTape;
    }    
}
