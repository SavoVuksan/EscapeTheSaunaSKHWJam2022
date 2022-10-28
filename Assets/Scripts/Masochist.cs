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


    private StateMachine _moveStateMachine;
    private bool _gotHit;
    private bool _reachedPlayer;
    private float _boredTimer;
    private float _dirtyTalkTimer;
    private float _dirtyTalkSilenceTimer;
    private Coroutine _currentDirtyTalkCoroutine;
    private AudioHandler _audioHandler;

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
        _moveStateMachine = gameObject.AddComponent<StateMachine>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.SetNewState(new Masochist.IdleState());
        _audioHandler = GetComponent<AudioHandler>();
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
            _moveStateMachine.SetNewState(new LeaveState());
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
                _audioHandler.PlayRandomFromGroup("Dirtytalk");
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
        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
            if ((Self as Masochist).GetDistanceToPlayer() < (Self as Masochist).MoveActivationRadiusToPlayer)
            {
                stateMachine.SetNewState(new MoveState());
            }
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    private class MoveState : HumanoidState
    {
        public override void ExitState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
            if ((Self as Masochist).GetDistanceToPlayer() < (Self as Masochist).PlayerReachRadius && !(Self as Masochist)._reachedPlayer)
            {
                (Self as Masochist).ReachedPlayer = true;
            }
            Vector3 playerToEnemy = Self.transform.position - Self.Player.transform.position;
            Vector3 targetPos = Self.Player.transform.position + playerToEnemy.normalized * (Self as Masochist).PlayerReachRadius;
            Self.NavMeshAgent.SetDestination(targetPos);
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    private class LeaveState : HumanoidState
    {
        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
            Vector3 playerToEnemy = Self.transform.position - Self.Player.transform.position;
            Self.NavMeshAgent.SetDestination(Self.transform.position + playerToEnemy.normalized);
            if ((Self as Masochist).GetDistanceToPlayer() >= (Self as Masochist).DespawnRadius)
            {
                // Despawn Masochist.
            }
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }
}
