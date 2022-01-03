using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    public bool HitBlockBool { get; set; } = false;

    public void ResetValue()
    {        
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
        HitBlockBool = false;
    }

    private void Update()
    {
        if (HitBlockBool)
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
        ResetValue();
    }
}
