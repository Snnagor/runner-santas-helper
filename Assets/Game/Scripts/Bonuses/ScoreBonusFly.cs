using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class ScoreBonusFly : MonoBehaviour
{
    [SerializeField] private Transform panel;

    [SerializeField] private TextMeshProUGUI valueScore;
    [SerializeField] private Image iconCoin;

    public int BonusScoreValue { get; set; }

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
        valueScore.text = "+" + BonusScoreValue.ToString();
        Begin();

    }

    public void Begin()
    {
        if (gameObject.activeSelf)
        {
            DefinePosition();

            Sequence seq = DOTween.Sequence();            
            
            seq.Append(panel.DOJump(new Vector3(panel.position.x, panel.position.y + 5f, panel.position.z), 0.3f, 1, 0.3f));           
            seq.AppendInterval(0.5f);            
            seq.Append(panel.DOMove(pos, 0.3f));
            seq.Join(panel.DOScale(0f, 0.15f));          
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
        AnimIconScore();
        scoreManager.DisctanseScore += BonusScoreValue;
        Destroy(gameObject);
    }
}
