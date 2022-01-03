using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "InstallerSO", menuName = "Installers/InstallerSO")]
public class InstallerSO : ScriptableObjectInstaller<InstallerSO>
{
    [SerializeField] private Config _config;
    [SerializeField] private LocationsSettings _locationsSettings;
    [SerializeField] private DifficultSettings _difficultSettings;
    [SerializeField] private ConfigFileDefault _configFileDefault;
    [SerializeField] private ShopSettings _shopSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(_config);
        Container.BindInstances(_locationsSettings);
        Container.BindInstances(_difficultSettings);
        Container.BindInstances(_configFileDefault);
        Container.BindInstances(_shopSettings);
    }
}