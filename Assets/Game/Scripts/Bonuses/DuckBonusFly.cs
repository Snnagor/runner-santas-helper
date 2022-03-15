using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class DuckBonusFly : MonoBehaviour
{    
    Vector3 pos;

    #region Injects

    private AnimGamePanal animGamePanal;
    private ScoreManager scoreManager;

    [Inject]
    private void Construct(AnimGamePanal _animGamePanal, ScoreManager _scoreManager)
    {
        animGamePanal = _animGamePanal;
        scoreManager = _scoreManager;
    }

    #endregion

    private void Start()
    {        
        Begin();
    }

    public void Begin()
    {
        if (gameObject.activeSelf)
        {
            // Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 30f));

            DefinePosition();

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOJump(new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z), 0.3f, 1, 0.25f));
            seq.Append(transform.DORotate(new Vector3(0, 180f, 0), 0.35f));
            seq.Append(transform.DOMove(pos, 0.3f));
            seq.Join(transform.DOScale(0f, 0.15f));
            seq.OnComplete(Deactivate); 
        }
    }

    private void DefinePosition()
    {
        pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 30f));
    }

    private void AnimIconScore()
    {
        animGamePanal.AnimScoreIcon();
    }

    private void Deactivate()
    {
       // AnimIconScore();
       // scoreManager.CountDuck ++;
        Destroy(gameObject);
    }
}
