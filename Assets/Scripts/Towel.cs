using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using System;
using UnityEngine.Events;
public class Towel : MonoBehaviour
{
    [SerializeField]
    public float MinVelocityToHit = 5;
    private Transform _initialParent;

    private UnityAction<HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber, HVRGrabbable> _onGrab;

    private DamageCollider _damageCollider;
    private void Start()
    {
        _initialParent = transform.parent;
        _damageCollider = GetComponentInChildren<DamageCollider>(true);
    }

    private void OnEnable()
    {
        _onGrab = OnGrab;
        GetComponent<HVRGrabbable>().HandGrabbed.AddListener(_onGrab);
        if(_damageCollider == null){
            _damageCollider = GetComponentInChildren<DamageCollider>(true);
        }
        _damageCollider.OnCollisionEnterEvent += OnHit;
    }

    private void OnDisable()
    {
        GetComponent<HVRGrabbable>().HandGrabbed.RemoveListener(_onGrab);
        _damageCollider.OnCollisionEnterEvent -= OnHit;

    }

    private void OnGrab(HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber arg1, HVRGrabbable arg2)
    {
        ReparentOriginal();
    }

    private void OnHit(Collision collision){
        var hitable = collision.transform.parent.gameObject.GetComponent<IHitable>();
        print(collision.relativeVelocity.magnitude);
        if(hitable != null && collision.relativeVelocity.magnitude >= MinVelocityToHit){
            hitable.OnHit(999);
        }
    }

    public void ReparentOriginal()
    {
        transform.parent = _initialParent;
    }
}