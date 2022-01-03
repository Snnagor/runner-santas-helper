using UnityEngine;
using DG.Tweening;

public class Coin : MoveObjects
{
    [SerializeField] private Transform meshCoin;

    private void Update()
    {
        if (magnetBonus.Enable )
        {
            float dist = Vector3.Distance(transform.position, magnetBonus.transform.position);

            if (dist < magnetBonus.DistanseMagnet)
            {               
                MoveToMagnet(magnetBonus.transform.position, magnetBonus.AccelerationMagnet);
            }
            else
            {
                Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
                MoveToMagnet(target, 0);
            }
        }
        else
        {
            Move();
        }
             
        meshCoin.Rotate(Vector3.left * 150f * Time.deltaTime);
     }

    /// <summary>
    /// Движение на магнит
    /// </summary>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    public void MoveToMagnet(Vector3 target, float speed)
    {
        if (gameManager.IsRun && gameObject.activeSelf)
        {            
            transform.position = Vector3.MoveTowards(transform.position, target, (gameManager.CurrentSpeed + speed) * Time.deltaTime);
        }
    }


}
