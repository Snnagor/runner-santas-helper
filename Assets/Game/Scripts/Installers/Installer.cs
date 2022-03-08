using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{    

    public override void InstallBindings()
    {
        BindInput();
        BindSignals();        
    }

    private void BindInput()
    {
        Container.Bind<InputHandler>().AsSingle().NonLazy();
    }

    private void BindSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<StartSignal>();
        Container.DeclareSignal<LoseSignal>();
        Container.DeclareSignal<MeterSignal>();
        Container.DeclareSignal<TakeCoinSignal>();
        Container.DeclareSignal<AccelerationSignal>();
        Container.DeclareSignal<ChangeCountTrackSignal>();
        Container.DeclareSignal<ChangeWidthTrackSignal>();
        Container.DeclareSignal<HitGiftTopBlockSignal>();
        Container.DeclareSignal<CreateLineSignal>();
        Container.DeclareSignal<TakeBonusSignal>();
        Container.DeclareSignal<StartBonusSignal>();
        Container.DeclareSignal<MusicSignal>();
        Container.DeclareSignal<SoundSignal>();
        Container.DeclareSignal<ContinueSignal>();
        Container.DeclareSignal<TopTutorialSignal>();
    }
}
