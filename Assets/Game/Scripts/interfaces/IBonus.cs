using UnityEngine;

public interface IBonus
{  
    public bool Enable { get; set; }      

    public bool HaveTimer { get;}

    public Sprite IconBonuse { get; }

    public GameObject PrefabFlyBonus { get; }

    public float TimeBonus { get; set; }

    public float CurrentTimeBonus { get; set; }

    public void Init();

    public void Disable();

    public void ResetTimer();

    public void CounterTimeBonus();
}
