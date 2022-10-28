using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DamageCollider : MonoBehaviour
{
    public event Action<Collision> OnCollisionEnterEvent;
    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEnterEvent?.Invoke(other);
    }
}
