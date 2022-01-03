using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LidersUIcontroll : MonoBehaviour
{
    [SerializeField] private Text[] topValueText; 

    #region Injects

    private DataManager dataManager;    

    [Inject]
    private void Construct(DataManager _dataManager)
    {
        dataManager = _dataManager;       
    }

    #endregion

    private void OnEnable()
    {
        LoadData();
    }

    private void LoadData()
    {
        int[] tmpArray = dataManager.LoadTopData();

        for (int i = 0; i < tmpArray.Length; i++)
        {
            int number = i + 1;

            topValueText[i].text = number.ToString() + ". " + tmpArray[i].ToString();
        }
    }
}
