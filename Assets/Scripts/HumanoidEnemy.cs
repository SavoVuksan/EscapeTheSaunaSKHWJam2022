using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidEnemy : BaseEnemy
{
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

    private float _maxMoveSpeed;

}
