using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockEnemy : HumanoidEnemy
{
    public override void OnHit(float damage)
    {
        print("Mock Enemy got hit!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
