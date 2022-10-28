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
    public float PlayerReachMarginPercent = 0.1f;
    [SerializeField]
    public float DespawnRadius;
    [SerializeField]
    public Timer BoredTimer;
    [SerializeField]
    public Timer DirtyTalkTimer;
    [SerializeField]
    public Timer DirtyTalkSilenceTimer;

    private StateMachine _moveStateMachine;
    private bool _gotHit;
    private bool _reachedPlayer;
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
        _moveStateMachine = gameObject.AddComponent<StateMachine>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.SetNewState(new Masochist.IdleState());
        _audioHandler = GetComponent<AudioHandler>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        BoredTimer.Init(this);
        DirtyTalkTimer.Init(this);
        DirtyTalkSilenceTimer.Init(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();

    }

    public void Update()
    {
        if (_gotHit && !BoredTimer.TimeOut && _currentDirtyTalkCoroutine == null)
        {
            //Start dirty talk coroutine.
            _currentDirtyTalkCoroutine = StartCoroutine(nameof(DirtyTalk));
        }
        if (!_gotHit && BoredTimer.TimeOut && !(_moveStateMachine.CurrentState is LeaveState))
        {
            // Go away
            _moveStateMachine.SetNewState(new LeaveState());
        }
        if (_reachedPlayer && !BoredTimer.Running)
        {
            BoredTimer.StartTimer();
        }
        if(!_reachedPlayer && BoredTimer.Running){
            BoredTimer.StopTimer();
        }
    }

    public override void OnHit(float damage)
    {
        _gotHit = true;
    }

    private IEnumerator DirtyTalk()
    {
        DirtyTalkTimer.StartTimer();
        DirtyTalkSilenceTimer.TimeLeft = 0;
        while (DirtyTalkTimer.TimeLeft > 0)
        {
            if (DirtyTalkSilenceTimer.TimeLeft <= 0)
            {
                _audioHandler.PlayRandomFromGroup("Dirtytalk");
                DirtyTalkSilenceTimer.ResetTimer();
            }
            yield return new WaitForEndOfFrame();
        }
        // Start Go Away behaviour
        _moveStateMachine.SetNewState(new LeaveState());
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
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, PlayerReachRadius * ( 1 + PlayerReachMarginPercent));
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
            if ((Self as Masochist).GetDistanceToPlayer() < (Self as Masochist).PlayerReachRadius * (1 + (Self as Masochist).PlayerReachMarginPercent)
            && !(Self as Masochist)._reachedPlayer)
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
                Destroy(Self.gameObject);
            }
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }
}
