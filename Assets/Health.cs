using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Health : MonoBehaviour
{
    //visuals

    //stats
    [Header("Stats")]
    public int maxHealth = 1;
    public int currentHealth = 0;

    [Header("Events")]
    public UnityEvent deathEvent;
    public UnityEvent hitEvent;

    [Header("Invulnerable")]
    public bool useInvulnerableFrames = false;
    public float invulnerableTime = 1f;
    private bool isInvulnerable;
    [HideInInspector] public bool isDead;
    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        currentHealth = maxHealth;
    }


    public int damageAmount;

    public void OnCollisionEnter(Collision collision)
    {
        var damageCollider = collision.collider.gameObject.GetComponent<DamageCollider>();

        Debug.Log(collision.transform.name);
        if (damageCollider)
        {
            takeDamage(damageAmount);
        }

    }

    public virtual void takeDamage(int amount)
    {
        if (isInvulnerable || isDead)
            return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        Debug.Log("hit");
        updateVisuals();
        hitEvent.Invoke();

        if (currentHealth <= 0)
        {
            isDead = true;
            deathEvent.Invoke();
            Kill();
        }
        if (useInvulnerableFrames)
            startInvonrableFrames(invulnerableTime);
    }
    public void startInvonrableFrames(float duration)
    {
        StartCoroutine(invulnerableFrames(duration));
    }
    private IEnumerator invulnerableFrames(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }
    private void updateVisuals()
    {

    }
    [Header("audio")]
    public RandomAudioPlayer DamageAudio;
    public void hitAudio()
    {
        DamageAudio.PlayRandomClip();
    }

    public virtual void Kill()
    {
        isDead = true;
        Debug.Log("I died");
    }
}