using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HandBrain : StateManager
{
    public Idle idleState = new Idle();
    public chase chaseState = new chase();

    public Player player;
    public fondleTarget myTarget;
    public Transform hand;
    public float _speed = 1;
    public float _rotationSpeed = 1;

    public float retriveSpeed;
    public float retriveRotationSpeed;
    private bool updateTarget = true;
    public bool retreaving;
    private GameObject stealObj;
    private List<Vector3> myPoints = new List<Vector3>();
    public void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        idleState.SetBrain(this);
        chaseState.SetBrain(this);
    }
    void Start()
    {
        setNewState(idleState);

        lastPoint = hand.position;
    }


    public override void Update()
    {
        base.Update();


        //   searchForPlayer(out player);


        leaveTrail();

        if (!retreaving)
        {
            if(updateTarget)
            myTarget = player.getFondelTarget();
            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(hand.position, myTarget.transform.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);
            if (player) chasePlayer();

        }
        else
        {
            updateTarget = true;
            retrieve();
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Invoke("chasePlayer", 1);
    }
    public void chasePlayer()
    {
        if(lingering)
        {
            return;
        }
        if (Vector3.Distance(hand.position, myTarget.transform.position) < reachRadius)
        {
            StartCoroutine(linger());
            lingering = true;
        }


        var step = _speed * Time.deltaTime;
        hand.position = Vector3.MoveTowards(hand.position, hand.position + hand.forward, step);


        var heading = _deviatedPrediction - hand.position;

        var rotation = Quaternion.LookRotation(heading);
        hand.rotation = Quaternion.RotateTowards(hand.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }

    [Header("hand Linger")]
    public float lingerTime = 3;
    public float reachRadius = 0.5f;
    private bool lingering;

    public void hit()
    {
        lingering = false;
        retreaving= true;

    }
    private IEnumerator linger()
    {
        //queue sound

        yield return new WaitForSeconds(lingerTime);
        if (Vector3.Distance(hand.position, myTarget.transform.position) > reachRadius +1)
        {
            lingering = false;
            yield break;
        }

        retreaving = true;
        //fondle
        if (myTarget.stealObject(out stealObj))
        {
            pickUpObject(true);

        }
        else
            stealObj = null;

        myTarget.startFondle();
        myTarget.stopFondle();
    }

    public void retrieve()
    {

        if (addedPoints == 0)
        {
            retreaving = false;
            hand.position = transform.position;


            if(stealObj)
            {
                pickUpObject(false);
            }
            return;
        }
        var target = myPoints[addedPoints - 1];
        var step = retriveSpeed * Time.deltaTime;
        hand.position = Vector3.MoveTowards(hand.position, target, step);


        var heading = hand.position - target;

        var rotation = Quaternion.LookRotation(heading);
        hand.rotation = Quaternion.RotateTowards(hand.rotation, rotation, retriveRotationSpeed * 3 * Time.deltaTime);

        pickUpTrail();
    }

    #region interact
    public bool searchForPlayer(out Transform player)
    {
        var players = Physics.OverlapSphere(hand.position, 101, playerLayer);

        //closest player
        if (players.Length > 0)
            player = players[0].transform;
        else
            player = null;

        return player != null ? true : false;
    }


    public void pickUpObject(bool bo)
    {
        var rb = stealObj.GetComponent<Rigidbody>();


        if (!bo)
        {
            stealObj.transform.parent = null;
            rb.isKinematic = false;
            stealObj = null;
        }
        else
        {
            stealObj.transform.position = hand.transform.position;
            stealObj.transform.parent = hand;
            rb.isKinematic = true;

        }
    }
    #endregion

    #region trail
    public void updateTrail()
    {
        if (myPoints != null)
        {//myPoints != null
            lineRenderer.SetVertexCount(myPoints.Count);
            for (int i = 0; i < myPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, myPoints[i]);
            }
        }
    }

    public void pickUpTrail()
    {

        var distance = Vector3.Distance(myPoints[addedPoints - 1], hand.position);

        if (distance < 0.1f)
        {
            myPoints.RemoveAt(addedPoints - 1);
            addedPoints--;
        }
    }

    public LineRenderer lineRenderer;
    public float minVertaxDistance = 0.2f;

    private int startingPoints = 50;
    private int addedPoints = 0;
    private Vector3 lastPoint;
    public void leaveTrail()
    {
        var distance = Vector3.Distance(lastPoint, hand.position);

        if (distance > minVertaxDistance)
        {

            lastPoint = hand.position;
            //add new point linerender

            myPoints.Add(hand.position);
            addedPoints++;

            updateTrail();

        }
    }
    #endregion

    #region diviation/sway
    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmountX = 50;
    [SerializeField] private float _deviationAmountY = 50;
    [SerializeField] private float _deviationSpeedX = 2;
    [SerializeField] private float _deviationSpeedY = 2;
    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = myTarget.transform.position; // rb.velocity* predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviationX = new Vector3(Mathf.Cos(Time.time * _deviationSpeedX), 0, 0);
        var deviationY = new Vector3(0, Mathf.Cos(Time.time * _deviationSpeedY), 0);

        var predictionOffsetX = hand.TransformDirection(deviationX) * _deviationAmountX * leadTimePercentage;
        var predictionOffsetY = hand.TransformDirection(deviationY) * _deviationAmountY * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffsetX + predictionOffsetY;
    }
    #endregion

}



#region states

public class Idle : IState
{
    Enemy_HandBrain Ctx;
    public void SetBrain(Enemy_HandBrain brain)
    {
        Ctx = brain;
    }

    public void enterState(StateManager manager)
    {
    }
    public void exitState(StateManager manager)
    {
    }
    public void updateState(StateManager manager)
    {
        if (Ctx.player != null)
            Ctx.setNewState(Ctx.chaseState);


        resetArm();
    }
    public void FixedUpdateState(StateManager manager)
    {
    }
    public void resetArm()
    {
        var step = Ctx._speed * Time.deltaTime;
        Ctx.hand.position = Vector3.MoveTowards(Ctx.hand.position, Ctx.transform.position, step);
    }
}
public class chase : IState
{
    Enemy_HandBrain Ctx;
    public void SetBrain(Enemy_HandBrain brain)
    {
        Ctx = brain;
    }

    public void enterState(StateManager manager)
    {
    }
    public void exitState(StateManager manager)
    {
    }
    public void updateState(StateManager manager)
    {
        if (Ctx.player == null)
            Ctx.setNewState(Ctx.idleState);


        // chasePlayer();
    }
    public void FixedUpdateState(StateManager manager)
    {
    }
    public void chasePlayer()
    {


        var step = Ctx._speed * Time.deltaTime;
        Ctx.hand.position = Vector3.MoveTowards(Ctx.hand.position, Ctx.hand.position + Ctx.hand.forward, step);


        var heading = Ctx.player.transform.position - Ctx.hand.position;

        var rotation = Quaternion.LookRotation(heading);
        Ctx.hand.rotation = Quaternion.RotateTowards(Ctx.hand.rotation, rotation, Ctx._rotationSpeed * Time.deltaTime);
    }

}
#endregion