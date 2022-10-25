using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  

    public fondleTarget slong;
    public fondleTarget towel;


    void Update()
    {
        
    }
 


    public fondleTarget getFondelTarget()
    {
        if (towel)
        {
            return towel;
        }
        else
            return slong;
    }
    
    
    public void equipTowel(fondleTarget newTowel)
    {
        towel = newTowel;
    }
}
