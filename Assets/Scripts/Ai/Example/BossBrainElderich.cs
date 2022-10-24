using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrainElderich : StateManager
{
    public Attack_SummonDebris DebrisAttackState = new Attack_SummonDebris();
    public Idle idleState = new Idle();
    public fly flyState = new fly();


    public void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        DebrisAttackState.SetBrain(this);
        idleState.SetBrain(this);
        flyState.SetBrain(this);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveToPosition(FlyPosition.position));
       setNewState(idleState);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    #region common functions

    public float moveDuration = 1;

    public Transform FlyPosition;
    private Rigidbody rb;
    IEnumerator MoveToPosition(Vector3 endPoint)
    {
        var startPoint = rb.transform.position;
        var timeElapsed = 0f;
        do
        {
            timeElapsed += Time.deltaTime;
            var prosentage = timeElapsed / moveDuration;

            //move foot here
            var animationPoint = Vector3.Lerp(startPoint, endPoint, prosentage);
            rb.MovePosition( animationPoint);
            yield return null;
        }
        while (timeElapsed <= moveDuration);

        //reached end location
    }

    #endregion

    #region states

    public class Idle : IState
    {
        BossBrainElderich _brain;
        public void SetBrain(BossBrainElderich brain)
        {
            _brain = brain;
        }

        public void enterState(StateManager manager)
        {
            //CoroutineHelper.RunCoroutine(TestWait(manager));
        }
        public void exitState(StateManager manager)
        {
        }
        public void updateState(StateManager manager)
        {
        }
        public void FixedUpdateState(StateManager manager)
        {
        }

        public IEnumerator TestWait(StateManager manager)
        {
                yield return new WaitForSeconds(2f);

            manager.setNewState(_brain.flyState);
        }
    }

    public class fly : IState
    {
        BossBrainElderich _brain;
        public void SetBrain(BossBrainElderich brain)
        {
            _brain = brain;
        }

        public void enterState(StateManager manager)
        {
            _brain.StartCoroutine(_brain.MoveToPosition(_brain.FlyPosition.position));
            //CoroutineHelper.RunCoroutine(TestWait(manager));

        }
        public void exitState(StateManager manager)
        {

        }
        public void updateState(StateManager manager)
        {

        }
        public void FixedUpdateState(StateManager manager)
        {

        }
        public IEnumerator TestWait(StateManager manager)
        {
            yield return new WaitForSeconds(2f);

            manager.setNewState(_brain.DebrisAttackState);
        }

    }

    [System.Serializable]
    public class Attack_SummonDebris : IState
    {
        BossBrainElderich _brain;
        public void SetBrain(BossBrainElderich brain)
        {
            _brain = brain;
        }

        [Header("DebrisAttack")]
        public GameObject Debris;
        public int spawnHeight = 4;
        public int spawnAmount;

        public void enterState(StateManager manager)
        {
           // CoroutineHelper.RunCoroutine(Attack(manager));
        }
        public void exitState(StateManager manager)
        {

        }
        public void updateState(StateManager manager)
        {

        }
        public void FixedUpdateState(StateManager manager)
        {

        }
        public IEnumerator Attack(StateManager manager)
        {
            Debug.Log("attac");
            var attacksDone = 0;
            while (attacksDone < spawnAmount)
            {
                yield return new WaitForSeconds(1f);
            Debug.Log("attac2");

               // SpawnDebris(manager.player.position);
                attacksDone++;

                yield return null;
            }

            yield return new WaitForSeconds(1f);

            manager.setNewState(_brain.idleState);
        }
        public void SpawnDebris(Vector3 position)
        {
           /* var spawnPos = position + Vector3.up * spawnHeight;
            var spawned = PoolManager.Spawn(Debris, spawnPos, Debris.transform.rotation);

            var ConeAnimation = spawned.GetComponent<BossAttack_Cone>();
            ConeAnimation.Animate(spawnPos, position);*/
        }
    }

    #endregion
}