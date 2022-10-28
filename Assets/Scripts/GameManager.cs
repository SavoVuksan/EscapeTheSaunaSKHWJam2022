using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public HVRCanvasFade winCanvas;
    public HVRCanvasFade loseCanvas;
    public GameObject TowerSword;
    public HVRSocket WaistSocket;

    [Header("DEBUG")]
    public bool hasTowel = true;

    // Start is called before the first frame update

    public void StartGame()
    {
        StartCoroutine(FogCoroutine());
    }

    IEnumerator FogCoroutine()
    {
        float _time = 0;
        float _fogFadeTime = 3;
        WaitForSecondsRealtime waitTiny;
        waitTiny = new WaitForSecondsRealtime(0.05f);


        while (_time < _fogFadeTime)
        {
            RenderSettings.fogEndDistance = 200 - (_time * 196 / _fogFadeTime);
            yield return waitTiny;
            _time += 0.05f;
        }

    }

    public void LoseGame()
    {
        loseCanvas.Fade(1, 1);
    }

    public void WinGame()
    {
        winCanvas.Fade(1, 1);
    }

    public void LostTowel()
    {
        hasTowel = false;
    }

    public void RecoverTower()
    {
        hasTowel = true;
    }
}
