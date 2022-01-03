using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CounterStep : MonoBehaviour
{
    private SignalBus signalBus;
   
    #region Injects

    [Inject]
    private void Construct(SignalBus _signalBus)
    {        
        signalBus = _signalBus;       
    }

    #endregion        

    /// <summary>
    /// —четчик шагов
    /// </summary>
    public void CountMeter()
    {
        signalBus.Fire(new MeterSignal());
    }
}
