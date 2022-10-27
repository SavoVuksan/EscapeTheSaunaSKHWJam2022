using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageCollider : MonoBehaviour
{
    public event Action<Collision> OnCollisionEntered;

    private void OnCollisionEnter(Collision other) {
        OnCollisionEntered?.Invoke(other);
    }

}
