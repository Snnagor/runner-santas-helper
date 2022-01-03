using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class DifficultZoneClass
//{
//    [SerializeField] private string nameZone;
//    public string NameZone { get => nameZone; }

//    [SerializeField] private int meter;
//    public int Meter { get => meter; }

//    [SerializeField] private int deltaDistanceItem;
//    public int DeltaDistanceItem { get => deltaDistanceItem; }


//    [Tooltip("¬еро€тность по€влени€ елементов на дороге")]
//    [Range(0, 10)]
//    [SerializeField] private List<int> elementChance;
//    public List<int> ElementChance { get => elementChance; }
//}

[CreateAssetMenu(fileName = "New DifficultSettings", menuName = "Configs/DifficultSettings")]
public class DifficultSettings : ScriptableObject
{
    [SerializeField] private List<DifficultZoneClass> zones;
    public List<DifficultZoneClass> Zones { get => zones; }
}
