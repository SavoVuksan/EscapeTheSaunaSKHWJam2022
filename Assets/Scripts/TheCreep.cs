using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TheCreep : HumanoidEnemy
{
    [SerializeField]
    public float MoveActivationRadiusToPlayer;
    [SerializeField]
    public float AttackRadius;
    [HideInInspector]
    public NavMeshAgent NavMeshAgent;
    [HideInInspector]
    public GameObject Player;
    private StateManager _moveStateMachine;
    private AttackState _attackState;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        NavMeshAgent = GetComponent<NavMeshAgent>();
        _moveStateMachine = gameObject.AddComponent<StateManager>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.setNewState(new IdleState());
    }

    void Update(){
        if(GetDistanceToPlayer() < AttackRadius && _attackState == null){
            _attackState = new AttackState();
            _attackState.enterState(null);
        }
    }

    public override void OnHit(float damage)
    {
        _moveStateMachine.setNewState(new HitState());
    }
    public float GetDistanceToPlayer()
    {
        return (transform.position - Player.transform.position).magnitude;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MoveActivationRadiusToPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}