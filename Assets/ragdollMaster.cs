using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollMaster : MonoBehaviour
{

    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidbodies;


    public bool DisableRagdollAtStart;
    public void Start()
    {
        GetRagdollComponents();
        if (DisableRagdollAtStart)
            RagdollModeOFF();
    }

    public void GetRagdollComponents()
    {
        ragdollColliders = transform.GetComponentsInChildren<Collider>();
        limbsRigidbodies= transform.GetComponentsInChildren<Rigidbody>();
    }


    public void RagdollModeON()
    {
        foreach (var col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (var rig in limbsRigidbodies)
        {
            rig.isKinematic = false;
        }
    }

    public void RagdollModeOFF()
    {
        foreach (var col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (var rig in limbsRigidbodies)
        {
            rig.isKinematic = true;
        }
    }
}
