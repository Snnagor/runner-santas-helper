using UnityEngine;

public class Gift : MoveObjects
{
    [SerializeField] private int colorGift;

    [SerializeField] private Transform meshGift;

    public int ColorGift { get => colorGift; }

    //private void Update()
    //{
    //    Move();

    //    meshGift.Rotate(Vector3.up * -150f * Time.deltaTime);
    //}

    public override void Execute()
    {
        Move();

        meshGift.Rotate(Vector3.up * -150f * Time.deltaTime);
    }
}
