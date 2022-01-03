using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class ShopBarProgress : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    [Header("Text")]
    [SerializeField] private Text titleUpgradeBonus;
    [SerializeField] private Text descUpgradeBonus;
    [SerializeField] private Text priceUpgradeBonus;
    [SerializeField] private Text timeUpgradeBonus;
    [SerializeField] private GameObject panelPrice;
    [SerializeField] private Button buyButton;


    #region Injects

    private ShopSettings shopSettings;
    private DataManager dataManager;

    [Inject]
    private void Construct(ShopSettings _shopSettings, DataManager _dataManager)
    {        
        shopSettings = _shopSettings;
        dataManager = _dataManager;
    }

    #endregion
    private void Awake()
    {
        foreach (var item in items)
        {
            item.SetActive(false);
        }
    }

    public void ShowProgress(int count)
    {       
        if(count <= items.Length)
        {
            for (int i = 0; i < count; i++)
            {
                items[i].SetActive(true);
            }
        }
    }

    public void Buy(int IdBonus, int idBuyProgress)
    {
        var price = shopSettings.UpgradesBonuses[IdBonus].PriceUpgrade[idBuyProgress].Price;

        dataManager.TotalCoin -= price;
    }

    public void UpdateInfo(int IdBonus, int count)
    {
        titleUpgradeBonus.text = shopSettings.UpgradesBonuses[IdBonus].NameBonus;
        descUpgradeBonus.text = shopSettings.UpgradesBonuses[IdBonus].DescriptionBonus;
                
        if (count < shopSettings.UpgradesBonuses[IdBonus].PriceUpgrade.Count - 1)
        {
            var nextLevel = shopSettings.UpgradesBonuses[IdBonus].PriceUpgrade[count + 1];
            priceUpgradeBonus.text = nextLevel.Price.ToString();
            timeUpgradeBonus.text = nextLevel.TimeBonus.ToString() + " sec";

            if (dataManager.TotalCoin < nextLevel.Price)
            {
                buyButton.interactable = false;
            }
            else
            {
                buyButton.interactable = true;
            }

        }            
        else
        {
            panelPrice.SetActive(false);
            buyButton.interactable = false;
        }
    }
    
}
