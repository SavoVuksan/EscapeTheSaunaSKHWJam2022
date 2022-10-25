using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class fondleTarget : MonoBehaviour
{
    public UnityAction startFondelEvent;
    public UnityAction StopFondelEvent;

    public virtual void startFondle()
    {
        startFondelEvent.Invoke();
    }

    public virtual void stopFondle()
    {
        StopFondelEvent.Invoke();

    }

}
