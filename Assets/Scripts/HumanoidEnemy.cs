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

    [SerializeField]
    private float _maxMoveSpeed;

    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public float GetDistanceToPlayer()
    {
        return (transform.position - Player.transform.position).magnitude;
    }
}
