using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastTester : MonoBehaviour
{
    public Camera camera;
    public towel target;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var _towel = hit.transform.GetComponent<towel>();
                if (_towel)
                {
                    target = _towel;
                    mZCoord = Camera.main.WorldToScreenPoint(target.transform.position).z;
                    mOffset = target.transform.position - GetMouseAsWorldPoint();

                    target.swordify(true);
                }
            }
        }
        if (Input.GetButton("Fire1"))
        {

            if (target)
            {
                target.transform.position = GetMouseAsWorldPoint() + mOffset;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            target.swordify(false);
            target = null;
        }
    }
    private Vector3 mOffset;
    private float mZCoord;

    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = mZCoord;



        // Convert it to world points
        Debug.Log(Camera.main.ScreenToWorldPoint(mousePoint));

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
}