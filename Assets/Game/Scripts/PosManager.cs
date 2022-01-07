using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PosManager : MonoBehaviour
{
    [SerializeField] private bool enableDark;

    [SerializeField] private Volume lightVolume;
    [SerializeField] private Volume darkVolume;

    private void Start()
    {
        darkVolume.enabled = false;
    }

    public void EnableDark(bool value)
    {
        if (enableDark)
        {
            if (value)
            {               
                lightVolume.priority = 0;
                darkVolume.enabled = true;
            }
            else
            {               
                lightVolume.priority = 2;
                darkVolume.enabled = false;
            }
        }
    }
}
