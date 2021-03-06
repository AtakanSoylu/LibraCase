using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    public static T Instance;
    public virtual void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this as T;

        DontDestroyOnLoad(this);
    }
}
