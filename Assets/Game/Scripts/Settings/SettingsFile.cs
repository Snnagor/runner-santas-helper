using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.IO;
using System.Text;
using System.Globalization;
using System;

public class SettingsFileData
{
    public string Name { get; set; }
    public ParameterTypeEnum ValueType { get; set; }
    public string Value { get; set; }

    public SettingsFileData(string Name, ParameterTypeEnum ValueType, string Value)
    {
        this.Name = Name;
        this.ValueType = ValueType;
        this.Value = Value;
    }
}

public class SettingsFile : MonoBehaviour
{
    private string pathFile = "Assets/Game/SettingsGame.csv";

    public List<SettingsFileData> Settings { get; set; } = new List<SettingsFileData>();

    private bool updateFileDefaul = false;

    #region Injects

    private ConfigFileDefault configFileDefault;

    [Inject]
    private void Construct(ConfigFileDefault _configFileDefault)
    {
        configFileDefault = _configFileDefault;        
    }

    #endregion
    
    public void CreatFile()
    {
        if (!File.Exists(pathFile))
        {            
            SaveDataFileDefault();
        }        

        ReadDataFile();
    }

    /// <summary>
    /// Сохранение даннын в файле
    /// </summary>
    public void SaveDataFileDefault()
    {
        using (StreamWriter sw = new StreamWriter(pathFile, true, Encoding.Unicode))
        {
            foreach (var item in configFileDefault.SettingsFileDefaultList)
            {
                string note = string.Empty;

                if (item.Name != null && item.DefaultValue != null)
                {           
                    note += $"{item.Name}\t{item.ValueType}\t{item.DefaultValue}\t{item.Description}\t";

                    sw.WriteLine(note);
                }
                else
                {
                    Debug.LogError("No Settings in ConfigFile");
                }
            }
        }
    }

    /// <summary>
    /// Чтение данных из файла
    /// </summary>
    public void ReadDataFile()
    {
        try
        {
            using (StreamReader sr = new StreamReader(pathFile, Encoding.Unicode))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split('\t');
                    int index = configFileDefault.SettingsFileDefaultList.FindIndex(value => value.Name == data[0]);

                    if (index >= 0)
                    {
                        ParameterTypeEnum typeParametr = configFileDefault.SettingsFileDefaultList[index].ValueType;

                        Settings.Add(new SettingsFileData(data[0], typeParametr, data[2]));
                    }
                    else
                    {
                        Debug.Log("Variable name has been changed. Settings file has been overwritten by default value");
                        updateFileDefaul = true;
                        return;
                    }

                }
            }
        }
        catch
        {
            Debug.LogError("File open!!");
        }
        
       
      
        Initialization();
    }

    private void Update()
    {
        if (updateFileDefaul)
        {
            File.Delete(pathFile);
            SaveDataFileDefault();

            updateFileDefaul = false;
        }
    }

    #region Parse Type

    private Dictionary<string, object> settingParameters = new Dictionary<string, object>();

    private void Awake()
    {
        CreatFile();
    }

    /// <summary>
    /// Получить данные в нужном типе
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual T GetParameterValue<T>(string name)
    {       
        // Если данные с файла читаются то выводим их в игру
        if (settingParameters.ContainsKey(name))
        {
            var parameterValue = (T) settingParameters[name];

            return parameterValue;
        }
        // Если данные из файла не читаются, то считываем данный по умолчанию из конфига
        else
        {
            Debug.Log("[Setting]: name not found");

            var parameterValue = (T) ParseDefaultSettings(name);

            return parameterValue;
        }

    }

    /// <summary>
    /// Конвертировать данные по умолчанию из конфига
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private object ParseDefaultSettings(string name)    
    {
        int index = configFileDefault.SettingsFileDefaultList.FindIndex(value => value.Name == name);

        var typeValue = configFileDefault.SettingsFileDefaultList[index].ValueType;
        string defaultValue = configFileDefault.SettingsFileDefaultList[index].DefaultValue;

        object parameterValue = null;

        switch (typeValue)
        {
            case ParameterTypeEnum.Float:
                {
                    if (!float.TryParse(defaultValue, out float value))
                    {
                        value = default;
                    }
                    parameterValue = value;
                }
                break;
            case ParameterTypeEnum.Int:
                {
                    if (!int.TryParse(defaultValue, out int value))
                    {
                        value = default;
                    }

                    parameterValue = value;
                }
                break;
            case ParameterTypeEnum.String:
                {
                    parameterValue = defaultValue;
                }
                break;
            case ParameterTypeEnum.Bool:
                {
                    if (!bool.TryParse(defaultValue, out bool value))
                    {
                        value = default;
                    }

                    parameterValue = value;
                }
                break;
        }

        return parameterValue;
    }

    /// <summary>
    /// Конвертировать данные из файла
    /// </summary>
    private void Initialization()
    {    
        for (var i = 0; i < Settings.Count; i++)
        {
            var parameter = Settings[i];

            object parameterValue = null;
         
            switch (parameter.ValueType)
            {
                case ParameterTypeEnum.Float:
                    {       
                        if (!float.TryParse(parameter.Value, out float value))
                        {
                            value = default;
                        }

                        parameterValue = value;
                    }
                    break;
                case ParameterTypeEnum.Int:
                    {
                        if (!int.TryParse(parameter.Value, out int value))
                        {
                            value = default;
                        }

                        parameterValue = value;
                    }
                    break;
                case ParameterTypeEnum.String:
                    {
                        parameterValue = parameter.Value;
                    }
                    break;
                case ParameterTypeEnum.Bool:
                    {
                        if (!bool.TryParse(parameter.Value, out bool value))
                        {
                            value = default;
                        }

                        parameterValue = value;
                    }
                    break;
            }

            settingParameters.Add(parameter.Name, parameterValue);
        }
    }

    #endregion

}
