using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public fondleTarget slong;
    public fondleTarget towel;
    public HVRSocket socket;
    public HVRGrabbable grabbable;

    public bool IsTowelSwordEquipped = false;

    private void Start()
    {
      //  grabbable.Socketed.AddListener(JustSocketed)
    }
    
    #region fondling
    public fondleTarget getFondelTarget()
    {
        if (towel)
        {
            return towel;
        }
        else
            return slong;
    }


    #endregion
    #region player stats

    public int currentHP =2;
  //  public int MaxHP = 2;

    public int TowelBuffer = 1;
   // public int MaxTowelBuffer = 1;



    public void takeDamage(int amount = 1)
    {

        if(currentHP <= 0)
        {
            gameOver();
            return;
        }
        
        
        
        //var damageOverflow = 0;
        if (TowelBuffer > 0)
        {
            TowelBuffer -= amount;

          //  if (TowelBuffer < 0)
          //  {
          //      damageOverflow = TowelBuffer * -1;
          //  }
        }
        else
            currentHP -= amount;

       /* if (damageOverflow != 0)
        {
            currentHP -= TowelBuffer;

        }
        else*/


        //check dead

    }
    public void gameOver()
    {
        //
    }

    #endregion

}
