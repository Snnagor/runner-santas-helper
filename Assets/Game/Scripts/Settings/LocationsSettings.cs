using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Locations
{
    [SerializeField] private int id;
    public int ID { get => id; }

    [SerializeField] private string name;
    public string Name { get => name; }

    [SerializeField] private int[] idRoadScene;
    public int[] IdRoadScene { get => idRoadScene; }


    [SerializeField] private List<LocationBlocks> locationBlocks;
    public List<LocationBlocks> LocationBlocks { get => locationBlocks; }
}

[System.Serializable]
public class LocationBlocks
{
    [SerializeField] private int id;
    public int Id { get => id; }

    [Tooltip("Вероятность появления преграды")]
    [Range(0, 10)]
    [SerializeField] private int chance;
    public int Chance { get => chance; }

    [SerializeField] private int numberTrack;
    public int NumberTrack { get => numberTrack; }
}

[CreateAssetMenu(fileName = "New LocationsSettings", menuName = "Configs/LocationsSettings")]
public class LocationsSettings : ScriptableObject
{
    [SerializeField] private List<Locations> locations;
    public List<Locations> Locations { get => locations;}
}
