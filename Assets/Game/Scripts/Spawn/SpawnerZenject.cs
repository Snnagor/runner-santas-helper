using System;
using UnityEngine;
using Zenject;

public class SpawnerZenject : MonoBehaviour
{

    protected IMemoryPool _pool;

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public void Dispose()
    {
        _pool = null;
    }
}
