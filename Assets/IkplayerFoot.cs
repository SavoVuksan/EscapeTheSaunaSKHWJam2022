using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkplayerFoot : MonoBehaviour
{

    public Transform rightFoot;
    public Transform leftFoot;
    public Vector3 startOffsetRight;
    public Vector3 startOffsetLeft;

    void Update()
    {
        updateFeet(rightFoot, startOffsetRight);
        updateFeet(leftFoot, startOffsetLeft);
    }
    public Vector3 footOffset;
    public LayerMask layer;
    public void updateFeet(Transform foot, Vector3 offset)
    {
        var myOffset = transform.TransformDirection(offset);
        var origin = transform.position + myOffset + Vector3.up * 10;
        RaycastHit hit; // declare the RaycastHit variable
        if (Physics.Raycast(origin, Vector3.down, out hit, 10, layer))
        {
            foot.position = hit.point + footOffset;
        }
    }
}
