using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    [SerializeField] private Image iconBonus;
    public Image IconBonus { get => iconBonus; set => iconBonus = value; }

    [SerializeField] private Image iconBonusBack;
    public Image IconBonusBack { get => iconBonusBack; set => iconBonusBack = value; }

    public bool Enable { get; set; } = false;
    public int ActionIndexBonus { get; set; }
    public float MaxTime { get; set; }    
       
    private float currentTime;
    
    private void OnEnable()
    {        
        currentTime = MaxTime;
    }

    public void ReseTime()
    {
        currentTime = MaxTime;
    }

    void Update()
    {     
        if (Enable == true)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {             
                Enable = false;
                gameObject.SetActive(false);                
            }
        }

        iconBonus.fillAmount = currentTime / MaxTime;
    }
}
