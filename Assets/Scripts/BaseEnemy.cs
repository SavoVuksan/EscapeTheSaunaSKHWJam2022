using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IHitable
{
    internal float _currentHealth;
    [SerializeField]
    internal float _maxHealth;

    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
        }
    }

    public float MaxHealth{
        get{
            return _maxHealth;
        }
        set{
            _maxHealth = value;
        }
    }

    public abstract void OnHit(float damage);
}
