using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using System;
using UnityEngine.Events;
public class Towel : MonoBehaviour
{

    private Transform _initialParent;

    private UnityAction<HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber,HVRGrabbable> _onGrab;
    private void Start()
    {
        _initialParent = transform.parent;

    }

    private void OnEnable()
    {
        _onGrab = OnGrab;
        GetComponent<HVRGrabbable>().HandGrabbed.AddListener(_onGrab);
    }

    private void OnDisable()
    {
        GetComponent<HVRGrabbable>().HandGrabbed.RemoveListener(_onGrab);
    }

    private void OnGrab(HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber arg1, HVRGrabbable arg2) {
        transform.parent = _initialParent;
    }
}