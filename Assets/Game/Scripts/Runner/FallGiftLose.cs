using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FallGiftLose : MoveObjects
{
    [SerializeField] private MeshRenderer box;
    [SerializeField] private MeshRenderer tape;

    Rigidbody rb;
    Vector3 direction;
        

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {      
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.mass = 10;

        direction = Vector3.forward * 5 * gameManager.Speed;

        Physics.gravity = new Vector3(0, -100f, 0);

        rb.AddForce(direction, ForceMode.Impulse);
    }

    public void ChangeMat(Material _matBox, Material _matTape)
    {
        box.material = _matBox;
        tape.material = _matTape;
    }

    private void Update()
    {
        base.Move();

        if (gameObject.activeSelf)
        {
            Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
            if (position.x < 0 || position.x > 1 || position.y < 0 || position.y > 1)
            {
                OnBecameInvisible();
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
