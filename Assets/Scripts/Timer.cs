using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Timer
{
    public float Duration = 1;
    public bool AutoStart = false;
    public bool OneShot = false;
    [HideInInspector]
    public float TimeLeft;
    [HideInInspector]
    public bool TimeOut = false;
    private bool _startTimer = false;
    private MonoBehaviour _monoBehaviour;
    private Coroutine _coroutine;
    public event Action TimeOutEvent;
    public bool Running
    {
        get
        {
            return _startTimer;
        }
    }

    public void Init(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        TimeLeft = Duration;
        _startTimer = AutoStart;
        if (_coroutine == null)
        {
            _coroutine = _monoBehaviour.StartCoroutine(Update());
        }
    }

    // Update is called once per frame
    IEnumerator Update()
    {
        while (true)
        {
            if (_startTimer)
            {
                TimeLeft -= UnityEngine.Time.deltaTime;
                TimeLeft = Mathf.Max(0, TimeLeft);
                if (TimeLeft <= 0)
                {
                    if (!OneShot)
                    {
                        TimeLeft = Duration;
                    }
                    else
                    {
                        TimeOut = true;
                        _startTimer = false;
                    }
                    TimeOutEvent?.Invoke();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void ResetTimer()
    {
        TimeOut = false;
        TimeLeft = Duration;
        _startTimer = true;
    }
    public void StartTimer()
    {
        if (!TimeOut && !_startTimer)
        {
            _startTimer = true;
        }
    }

    public void StopTimer(){
        _startTimer = false;
        TimeLeft = Duration;
        TimeOut = false;
    }

    
}
