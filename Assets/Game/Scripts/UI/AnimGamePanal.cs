using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimGamePanal : MonoBehaviour
{
    [SerializeField] private Transform scoreIcon;

    public void AnimScoreIcon()
    {       
        Sequence seq = DOTween.Sequence();        
        seq.Append(scoreIcon.DOScale(1.7f, 0.1f));      
        seq.Append(scoreIcon.DOScale(1f, 0.2f));
    }
}
