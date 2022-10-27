using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockHumanoidEnemy : HumanoidEnemy
{
    public override void OnHit(float damage)
    {
        print($"Received Hit with {damage} DMG");
    }

}
