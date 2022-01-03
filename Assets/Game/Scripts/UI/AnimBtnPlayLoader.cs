using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimBtnPlayLoader : MonoBehaviour
{
    private bool flag = true;

    private void OnEnable()
    {
        flag = false;
    }

    void Update()
    {        
        if (!flag)
        {            
            flag = true;

            Sequence seq = DOTween.Sequence();
            seq.Append(gameObject.transform.DOScale(1.1f, Time.deltaTime * 125f));
            seq.Append(gameObject.transform.DOScale(1f, Time.deltaTime * 35f));
            seq.OnComplete(OnFlag);
        }
    }

    private void OnFlag()
    {
        flag = false;        
    }
}
