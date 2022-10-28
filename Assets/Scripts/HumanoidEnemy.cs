using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class HumanoidEnemy : BaseEnemy
{
    [SerializeField]
    public bool DrawGizmos = true;
    [HideInInspector]
    public NavMeshAgent NavMeshAgent;
    [HideInInspector]
    public GameObject Player;
    public Timer SoundTimer;
    [SerializeField]
    private float _maxMoveSpeed;
    public float MaxMoveSpeed
    {
        get
        {
            return _maxMoveSpeed;
        }
        set
        {
            _maxMoveSpeed = value;
        }
    }
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public float GetDistanceToPlayer()
    {
        return (transform.position - Player.transform.position).magnitude;
    }

    public virtual void OnEnable() {
        SoundTimer.Init(this);
    }

    public virtual void OnDisable() {
        
    }
}
