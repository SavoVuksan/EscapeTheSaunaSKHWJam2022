using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HandBrain : StateManager
{
    public Idle idleState = new Idle();
    public chase chaseState = new chase();

    public Transform player;

    public Transform IK_target;
    public float _speed =1;
    public float _rotationSpeed=1;

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

        lastPoint = IK_target.position;
    }


    public override void Update()
    {
        base.Update();


        searchForPlayer(out player);

    
        leaveTrail();
        if (!retreaving)
        {
            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(IK_target.position, player.position));

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
        if (Vector3.Distance(IK_target.position, player.transform.position) < 1)
            retreaving = true;


        var step = _speed * Time.deltaTime;
        IK_target.position = Vector3.MoveTowards(IK_target.position, IK_target.position + IK_target.forward, step);


        var heading = _deviatedPrediction - IK_target.position;

        var rotation = Quaternion.LookRotation(heading);
        IK_target.rotation = Quaternion.RotateTowards(IK_target.rotation, rotation, _rotationSpeed * Time.deltaTime);
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
        var deviationY = new Vector3( 0, Mathf.Cos(Time.time * _deviationSpeedY), 0);

        var predictionOffsetX = IK_target.TransformDirection(deviationX) * _deviationAmountX * leadTimePercentage;
        var predictionOffsetY = IK_target.TransformDirection(deviationY) * _deviationAmountY * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffsetX+ predictionOffsetY;
    }


    public bool searchForPlayer(out Transform player)
    {
        var players = Physics.OverlapSphere(IK_target.position, 101, playerLayer);

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
            IK_target.position = transform.position;
            return;
        }
        var target = myPoints[addedPoints - 1];
        var step = retriveSpeed * Time.deltaTime;
        IK_target.position = Vector3.MoveTowards(IK_target.position, target, step);


        var heading =  IK_target.position- target ;

        var rotation = Quaternion.LookRotation(heading);
        IK_target.rotation = Quaternion.RotateTowards(IK_target.rotation, rotation, retriveRotationSpeed * 3 * Time.deltaTime);

        pickUpTrail();
    }

    public void pickUpTrail()
    {

        var distance = Vector3.Distance(myPoints[addedPoints - 1], IK_target.position);

        if (distance < 0.1f)
        {
            myPoints.RemoveAt(addedPoints - 1);
            addedPoints--;
        }
    }

    public LineRenderer lineRenderer;
    public float minVertaxDistance =0.2f;

    private int startingPoints = 50;
    private int addedPoints = 0;
    private Vector3 lastPoint;
    public void leaveTrail()
    {
        var distance = Vector3.Distance(lastPoint, IK_target.position);

        if (distance > minVertaxDistance)
        {

            lastPoint = IK_target.position;
            //add new point linerender

            myPoints.Add(IK_target.position);
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
        Ctx.IK_target.position = Vector3.MoveTowards(Ctx.IK_target.position, Ctx.transform.position, step);
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
    public void chasePlayer ()
    {


        var step = Ctx._speed * Time.deltaTime;
        Ctx.IK_target.position = Vector3.MoveTowards(Ctx.IK_target.position, Ctx.IK_target.position+Ctx.IK_target.forward, step);


        var heading = Ctx.player.position- Ctx.IK_target.position ;

        var rotation = Quaternion.LookRotation(heading);
        Ctx.IK_target.rotation= Quaternion.RotateTowards(Ctx.IK_target.rotation, rotation, Ctx._rotationSpeed * Time.deltaTime);
    }

}
#endregion