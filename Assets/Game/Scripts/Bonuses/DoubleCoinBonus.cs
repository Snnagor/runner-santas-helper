using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCoinBonus : MonoBehaviour, IBonus
{   
    [SerializeField] private Sprite iconBonus;
    [SerializeField] private bool haveTimer;
    public bool HaveTimer { get => haveTimer;}

    [SerializeField] private GameObject prefabFlyBonus;
    public GameObject PrefabFlyBonus => prefabFlyBonus;

    public Sprite IconBonuse { get => iconBonus; }

    [SerializeField] private int multipleCoin;
    [SerializeField] private float timeBonus;

    public int MultipleCoin { get => multipleCoin; set => multipleCoin = value; }

    public bool Enable { get; set; }
    public float TimeBonus { get => timeBonus; set => timeBonus = value; }
    public float CurrentTimeBonus { get; set; } = -2;

    public void CounterTimeBonus()
    {
        if (Enable)
        {
            if (CurrentTimeBonus > 0)
            {
                CurrentTimeBonus -= Time.deltaTime;
            }
            else if (CurrentTimeBonus > -1)
            {
                Disable();
                CurrentTimeBonus = -2;
            }
        }
    }

    public void Disable()
    {
        Enable = false;
    }

    public void Init()
    {
        Enable = true;

        
        CurrentTimeBonus = TimeBonus;
    }

    public void ResetTimer()
    {

    }

    private void Update()
    {
        CounterTimeBonus();
    }
}
