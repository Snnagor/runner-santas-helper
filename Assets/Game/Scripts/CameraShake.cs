using UnityEngine;
using Cinemachine;
using Zenject;

public class CameraShake : MonoBehaviour
{    
    [SerializeField] private NoiseSettings _explosionNoise;

    private float shakeElapsedTime = -2;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    #region Injects
   
    private SignalBus signalBus;
    
    [Inject]
    private void Construct(SignalBus _signalBus)
    {      
        signalBus = _signalBus;
    }

    #endregion

    #region Signals

    private void OnEnable()
    {
        signalBus.Subscribe<LoseSignal>(LoseSignal);
    }

    private void OnDisable()
    {
        signalBus.Unsubscribe<LoseSignal>(LoseSignal);
    }

    private void LoseSignal()
    {
        Explosion();
    }

    #endregion

    private void Awake()
    {        
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.m_NoiseProfile = null;
    }

    void Update()
    {
        CameraTop();
    }

    private void CameraTop()
    {
        if (shakeElapsedTime > 0)
        {
            shakeElapsedTime -= Time.deltaTime;
        }
        else if (shakeElapsedTime > -1)
        {
            virtualCameraNoise.m_AmplitudeGain = 0;
            shakeElapsedTime = 0;
        }
    }

    public void Explosion()
    {
        virtualCameraNoise.m_NoiseProfile = _explosionNoise;
        virtualCameraNoise.m_AmplitudeGain = 10;
        virtualCameraNoise.m_FrequencyGain = 0.8f;
        shakeElapsedTime = 0.3f;
    }


}
