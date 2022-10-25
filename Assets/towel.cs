using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towel : MonoBehaviour
{
  



    public GameObject towelHang;
    public GameObject towelSword;
    public Player _player;
    public fondleTarget towelTarget;

    public void swordify(bool bo)
    {
        towelSword.SetActive(bo);
        towelHang.SetActive(!bo);
    }

    public void equipTowelInSlot()
    {
        _player.equipTowel(towelTarget);
        swordify(false);
    }
    public void unequipInSlot()
    {

        _player.equipTowel(null);
    }
    public void dropTowel()
    {
        swordify(true);
        //released but not equipped
    }
}