using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeBonus
{
    [SerializeField] private int id;
    public int Id { get => id; }

    [SerializeField] private string saveIdentifier;
    public string SaveIdentifier { get => saveIdentifier; }

    [SerializeField] private string nameBonus;
    public string NameBonus { get => nameBonus; }

    [TextArea]
    [SerializeField] private string descriptionBonus;
    public string DescriptionBonus { get => descriptionBonus; }

    [SerializeField] private List<UpgradePrice> priceUpgrade;
    public List<UpgradePrice> PriceUpgrade { get => priceUpgrade; }
}

[System.Serializable]
public class UpgradePrice
{
    [SerializeField] private int price;
    public int Price { get => price; }
        
    [SerializeField] private float timeBonus;
    public float TimeBonus { get => timeBonus; }
}

[CreateAssetMenu(fileName = "New ShopSettings", menuName = "Configs/ShopSettings")]
public class ShopSettings : ScriptableObject
{
    [SerializeField] private List<UpgradeBonus> upgradesBonuses;
    public List<UpgradeBonus> UpgradesBonuses { get => upgradesBonuses; }
}
