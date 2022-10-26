using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masochist : HumanoidEnemy
{
    [SerializeField]
    public float MoveActivationRadiusToPlayer;
    [SerializeField]
    public float PlayerReachRadius;
    [SerializeField]
    public float DespawnRadius;
    [SerializeField]
    public float BoredTime = 10;
    [SerializeField]
    public float DirtyTalkTime = 20;
    [SerializeField]
    public float DirtyTalkSilenceTime = 2;


    private StateManager _moveStateMachine;
    private bool _gotHit;
    private bool _reachedPlayer;
    private float _boredTimer;
    private float _dirtyTalkTimer;
    private float _dirtyTalkSilenceTimer;
    private Coroutine _currentDirtyTalkCoroutine;

    public bool ReachedPlayer
    {
        get
        {
            return _reachedPlayer;
        }
        set
        {
            _reachedPlayer = value;
        }
    }
    public override void Start()
    {
        base.Start();
        _gotHit = false;
        _boredTimer = BoredTime;
        _dirtyTalkTimer = DirtyTalkTime;
        _dirtyTalkSilenceTimer = 0;
        _moveStateMachine = gameObject.AddComponent<StateManager>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.setNewState(new Masochist.IdleState());
    }

    public void Update()
    {
        if (_gotHit && _boredTimer > 0 && _currentDirtyTalkCoroutine == null)
        {
            //Start dirty talk coroutine.
            _currentDirtyTalkCoroutine = StartCoroutine(nameof(DirtyTalk));
        }
        if (!_gotHit && _boredTimer <= 0 && !(_moveStateMachine.CurrentState is LeaveState))
        {
            // Go away
            _moveStateMachine.setNewState(new LeaveState());
        }
        if (_reachedPlayer)
        {
            _boredTimer -= Time.deltaTime;
            _boredTimer = Mathf.Max(_boredTimer, 0);
        }
    }

    public override void OnHit(float damage)
    {
        _gotHit = true;
    }

    private IEnumerator DirtyTalk()
    {
        while (_dirtyTalkTimer > 0)
        {
            if (_dirtyTalkSilenceTimer <= 0)
            {
                print("Insert dirty talk");
                _dirtyTalkSilenceTimer = DirtyTalkSilenceTime;
            }
            _dirtyTalkSilenceTimer -= Time.deltaTime;
            _dirtyTalkTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // Start Go Away behaviour
        yield return null;
    }

    public void OnDrawGizmos()
    {
        if (DrawGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, MoveActivationRadiusToPlayer);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PlayerReachRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DespawnRadius);
        }
    }



    private class IdleState : HumanoidState
    {
        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
            if ((Self as Masochist).GetDistanceToPlayer() < (Self as Masochist).MoveActivationRadiusToPlayer)
            {
                manager.setNewState(new MoveState());
            }
        }

        public override void updateState(StateManager manager)
        {
        }
    }

    private class MoveState : HumanoidState
    {
        public override void exitState(StateManager manager)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
        }

        public override void FixedUpdateState(StateManager manager)
        {
            if ((Self as Masochist).GetDistanceToPlayer() < (Self as Masochist).PlayerReachRadius && !(Self as Masochist)._reachedPlayer)
            {
                (Self as Masochist).ReachedPlayer = true;
            }
            Vector3 playerToEnemy = Self.transform.position - Self.Player.transform.position;
            Vector3 targetPos = Self.Player.transform.position + playerToEnemy.normalized * (Self as Masochist).PlayerReachRadius;
            Self.NavMeshAgent.SetDestination(targetPos);
        }

        public override void updateState(StateManager manager)
        {
        }
    }

    private class LeaveState : HumanoidState
    {
        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
            Vector3 playerToEnemy = Self.transform.position - Self.Player.transform.position;
            Self.NavMeshAgent.SetDestination(Self.transform.position + playerToEnemy.normalized);
            if ((Self as Masochist).GetDistanceToPlayer() >= (Self as Masochist).DespawnRadius)
            {
                // Despawn Masochist.
            }
        }

        public override void updateState(StateManager manager)
        {
        }
    }
}
