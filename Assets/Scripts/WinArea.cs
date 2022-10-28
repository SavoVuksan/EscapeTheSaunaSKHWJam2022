using HurricaneVR.Framework.Core.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    bool alreadyWon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyWon)
        {
            alreadyWon = true;
            GameManager.Instance.WinGame();
        }
    }

}
