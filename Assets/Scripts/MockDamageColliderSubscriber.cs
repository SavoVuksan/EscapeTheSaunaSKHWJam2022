using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockDamageColliderSubscriber : MonoBehaviour
{
    private DamageCollider _damageCollider;

    // Start is called before the first frame update
    void Start()
    {
        _damageCollider = GetComponentInChildren<DamageCollider>();

    }

    private void OnEnable()
    {
        if(_damageCollider == null){
            _damageCollider = GetComponentInChildren<DamageCollider>();
        }
        _damageCollider.OnCollisionEntered += OncollisionEntered;
    }

    private void OnDisable()
    {
        _damageCollider.OnCollisionEntered -= OncollisionEntered;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OncollisionEntered(Collision collision){
        var hitable = collision.gameObject.GetComponent<IHitable>();
        if(hitable != null){
            print("Hit called from mock player!");
            hitable.OnHit(999);
        }
    }
}
