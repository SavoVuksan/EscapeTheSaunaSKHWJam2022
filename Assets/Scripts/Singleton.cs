using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Singleton Class is mean to be inherited by classes where only one of that type should exist in the game
// For example, the game manager should be a Singleton because there can't be 2 managers

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // T stands for the class type
    // As this Singleton class is a 'template', T will be the class that inherits it
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T) this;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
