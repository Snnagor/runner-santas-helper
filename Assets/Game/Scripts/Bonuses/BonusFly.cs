using UnityEngine;
using DG.Tweening;
using Zenject;

public class BonusFly : MonoBehaviour
{
    public int IdBonus { get; set; }

    #region Injects

    private RunnerBonuses runnerBonuses;    

    [Inject]
    private void Construct(RunnerBonuses _runnerBonuses)
    {        
        runnerBonuses = _runnerBonuses;       
    }

    #endregion

    private void Start()
    {
        Begin();
    }

    public void Begin()
    {                       
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 30f));

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOJump(new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z), 0.3f, 1, 0.25f));
            seq.Append(transform.DORotate(new Vector3(0, 180f, 0), 0.35f));
            seq.Append(transform.DOMove(pos, 0.3f));
            seq.Join(transform.DOScale(0f, 0.15f));
            seq.OnComplete(Deactivate);        
    }

    private void Deactivate()
    {      
        runnerBonuses.StartBonusTimer(IdBonus);
        Destroy(gameObject);
    }

   

}
