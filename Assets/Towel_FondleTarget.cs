using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towel_FondleTarget : fondleTarget
{
    public override void startFondle()
    {
        base.startFondle();
    }

    public override void stopFondle()
    {
        base.stopFondle();

    }

    public override bool stealObject(out GameObject obj)
    {
        obj = this.gameObject;
        return stealAble;
    }

}
