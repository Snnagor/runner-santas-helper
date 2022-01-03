using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParameterTypeEnum
{
    Float,
    Int,
    String,
    Bool
}

[System.Serializable]
public class SettingsFileDefault
{  
    [SerializeField] private string _name;
    [SerializeField] private ParameterTypeEnum _valueType;
    [SerializeField] private string _defaultValue;
    [SerializeField] private string _description;

    public string Name => _name;
    public ParameterTypeEnum ValueType => _valueType;
    public string DefaultValue => _defaultValue;
    public string Description => _description;
}

[CreateAssetMenu(fileName = "New ConfigFile", menuName = "Configs/ConfigFile")]
public class ConfigFileDefault : ScriptableObject
{           

    [SerializeField] private List<SettingsFileDefault> settingsFileDefaultList;

    public List<SettingsFileDefault> SettingsFileDefaultList { get => settingsFileDefaultList; set => settingsFileDefaultList = value; }
    
}
