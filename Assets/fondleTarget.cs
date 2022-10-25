using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class fondleTarget : MonoBehaviour
{
    public UnityEvent startFondelEvent;
    public UnityEvent StopFondelEvent;

    public virtual void startFondle()
    {
        startFondelEvent.Invoke();
    }

    public virtual void stopFondle()
    {
        StopFondelEvent.Invoke();
    }

    public bool stealAble = false;
    public virtual bool stealObject(out GameObject obj)
    {
        obj = this.gameObject;
        return stealAble;
    }
}
