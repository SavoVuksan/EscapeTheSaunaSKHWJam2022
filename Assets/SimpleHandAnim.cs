using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class finger
{
    public GameObject root;
    public GameObject middle;
    public GameObject end;
}

public class SimpleHandAnim : MonoBehaviour
{
    public finger finger1;
    public finger finger2;
    public finger finger3;
    public finger finger4;

    public void Start()
    {
        animateFinger(finger1);
        animateFinger(finger2);
        animateFinger(finger3);
        animateFinger(finger4);
    }
    public float ammount;
    public float time;
    public void animateFinger(finger _finger)
    {
        LeanTween.rotateX(_finger.root, ammount , time *Random.Range(0.8f, 1.2f)).setLoopPingPong().setEaseInOutBack();
        LeanTween.rotateX(_finger.middle, ammount*0.5f, time * Random.Range(0.8f, 1.2f)).setLoopPingPong();
        LeanTween.rotateX(_finger.end, ammount*0.2f, time * Random.Range(0.8f, 1.2f)).setLoopPingPong();


    }


}
