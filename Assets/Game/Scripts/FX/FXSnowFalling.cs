using UnityEngine;
using Zenject;

public class FXSnowFalling : MonoBehaviour
{
    [SerializeField] private ParticleSystem fallingSnow;

    #region Injects

    private GameManager gameManager;    

    [Inject]
    private void Construct(GameManager _gameManager)
    {
        gameManager = _gameManager;        
    }

    #endregion

    private void Start()
    {
        var vel = fallingSnow.velocityOverLifetime;
        vel.yMultiplier = 50f;
    }

    public void PlaySnow()
    {
        fallingSnow.Play();
    }

    public void StopSnow()
    {
        fallingSnow.Stop();
    }

    private void Update()
    {
        var vel = fallingSnow.velocityOverLifetime;

        if (fallingSnow.isPlaying)
        {
            if (gameManager.IsRun)
            {
                vel.yMultiplier = gameManager.Speed / 2;
            }
            else
            {
                vel.yMultiplier = 3f;
            }
        }        
    }
}
