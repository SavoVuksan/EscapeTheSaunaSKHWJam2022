using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    private bool _isInRange;

    public bool IsInRange{
        get{
            return _isInRange;
        }
        set{
            _isInRange = value;
        }
    }

    void Start(){
        _isInRange = false;
    }
}
