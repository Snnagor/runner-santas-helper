using Cinemachine;
using UnityEngine;
using Zenject;

public class CamerasManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCameraRun;

    private CinemachineComposer composer;
    private Config config;

    float maxSpeedCam = 5f;

    #region Injects

    [Inject]
    private void Construct(Config _config)
    {             
        config = _config;
    }

    #endregion

    private void Awake()
    {
        composer = virtualCameraRun.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Start()
    {        
        float spedCam = (float) maxSpeedCam - config.SpeedCamera;

        if (spedCam > maxSpeedCam) spedCam = maxSpeedCam;
        if (spedCam < 0) spedCam = 0;

        composer.m_HorizontalDamping = spedCam;
    }

}
