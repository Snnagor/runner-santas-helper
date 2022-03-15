using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChanceGenerator : MonoBehaviour 
{
    private int[] bonusDropChance = new int[10];

    #region Injects
    
    private Config config;

    [Inject]
    private void Construct(Config _config)
    {       
        config = _config;
    }

    #endregion

    public void Start()
    {
        FillChanceBonuses();
    }
     

    /// <summary>
    /// Заполнение массива вероятностей появление бонусов
    /// </summary>
    /// <returns></returns>
    public void FillChanceBonuses()
    {        
        int indexTmpArray = 0;

       // print(config.ChanceBonus.Length + " config.ChanceBonus.Length");
               

        for (int j = 0; j < config.ChanceBonus.Length; j++)
        {
            for (int i = 0; i < config.ChanceBonus[j]; i++)
            {
                bonusDropChance[indexTmpArray] = j;

                indexTmpArray++;                 

                if (indexTmpArray > bonusDropChance.Length)
                    break;
               
            }
        }

    }    

    public int DropChanceBonuses()
    {
        int id = Random.Range(0, bonusDropChance.Length);

        return bonusDropChance[id];
    }

    
}
