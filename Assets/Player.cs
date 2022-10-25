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

    private void Start()
    {
        socket.Grabbed.AddListener(IjustGrabbedX);
        socket.Released.AddListener(IjustReleasedX);


      //  grabbable.Socketed.AddListener(JustSocketed)
    }

    #region socket events
    private void JustSocketed(HVRSocket arg0, HVRGrabbable arg1)
    {
        throw new NotImplementedException();
    }

    //arg0 = socket
    //arg1 = grabbed object
    private void IjustGrabbedX(HVRGrabberBase arg0, HVRGrabbable arg1)
    {
       var towel = arg1.GetComponent<towel>();
        if (towel)
        {
            towel.equipTowelInSlot();
        }

    }

    private void IjustReleasedX(HVRGrabberBase arg0, HVRGrabbable arg1)
    {

        towel = null;
    }
    #endregion

    
    public void equipTowel(fondleTarget newTowel)
    {
        towel = newTowel;
        if(newTowel != null)
        TowelBuffer++;
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
