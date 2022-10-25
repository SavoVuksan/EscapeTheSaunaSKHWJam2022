using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HandBrain : StateManager
{
    public Idle idleState = new Idle();
    public chase chaseState = new chase();

    public Transform player;

    public Transform hand;
    public float _speed = 1;
    public float _rotationSpeed = 1;

    public float retriveSpeed;
    public float retriveRotationSpeed;
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
            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(hand.position, player.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);
            if (player) chasePlayer();

        }
        else
            retrieve();

    }
    public bool retreaving;
    public override void FixedUpdate()
    {
        base.FixedUpdate();


    }
    List<Vector3> myPoints = new List<Vector3>();


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
    public void chasePlayer()
    {
        if (Vector3.Distance(hand.position, player.transform.position) < 1)
            retreaving = true;


        var step = _speed * Time.deltaTime;
        hand.position = Vector3.MoveTowards(hand.position, hand.position + hand.forward, step);


        var heading = _deviatedPrediction - hand.position;

        var rotation = Quaternion.LookRotation(heading);
        hand.rotation = Quaternion.RotateTowards(hand.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }
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

        _standardPrediction = player.transform.position; // rb.velocity* predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviationX = new Vector3(Mathf.Cos(Time.time * _deviationSpeedX), 0, 0);
        var deviationY = new Vector3(0, Mathf.Cos(Time.time * _deviationSpeedY), 0);

        var predictionOffsetX = hand.TransformDirection(deviationX) * _deviationAmountX * leadTimePercentage;
        var predictionOffsetY = hand.TransformDirection(deviationY) * _deviationAmountY * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffsetX + predictionOffsetY;
    }


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

    public void retrieve()
    {

        if (addedPoints == 0)
        {
            retreaving = false;
            hand.position = transform.position;
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


        var heading = Ctx.player.position - Ctx.hand.position;

        var rotation = Quaternion.LookRotation(heading);
        Ctx.hand.rotation = Quaternion.RotateTowards(Ctx.hand.rotation, rotation, Ctx._rotationSpeed * Time.deltaTime);
    }

}
#endregion