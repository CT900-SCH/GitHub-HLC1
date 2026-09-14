using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;
    public static T Instance => _instance;

    protected virtual void InitSingleton()
    {
        if(_instance != null)
        {
            throw new System.Exception($"{this.GetType()} singleton has been created before.");
        }
        else
        {
            _instance = (T)(MonoBehaviour)this;
        }
    }

    protected virtual void UninitSingleton()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void Awake()
    {
        InitSingleton();
    }

    protected virtual void OnDestroy()
    {
        UninitSingleton();
    }
}