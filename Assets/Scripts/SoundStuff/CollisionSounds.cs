using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioHandler))]
/// <summary>
/// This script assumes that there is a sound Handler with "DefaultSFX", "WoodSFX", "MetalSFX" etc
/// </summary>
public class CollisionSounds : MonoBehaviour
{
    public enum materialTypes
    {
        Wood,
        Metal,
        Glass,
        Boomerang,
        Organic,
        Floor
    }

    [Tooltip("How strong the collision has to be to make a sound, 40 is a nice value for boomerangs, may depend on weight")]
    public float minimumColisionStrengh = 40;

    [Tooltip("How strong the collision has to cap the max volume, 400 is a nice value for boomerangs")]
    public float maximumColisionStrengh = 400;

    public materialTypes materialType;
    public int audioPriority;
    //public float audioVolume;
    private AudioHandler audioHandler;
    private List<string> availableSounds = new List<string>();

    private void Start()
    {
        audioHandler = GetComponent<AudioHandler>();
        foreach (Sound s in audioHandler.sounds)
        {
            availableSounds.Add(s.name);
        }
    }

    /// <summary>
    /// gotta fix this for readability. obviously TODO
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<CollisionSounds>(out CollisionSounds otherCollisionSound))
        {
            if (otherCollisionSound.materialType != materialType)
            {
                PlayCollisionSound(otherCollisionSound.materialType.ToString() + "SFX", collision);
            }
            else
            {
                if (audioPriority >= otherCollisionSound.audioPriority)
                {
                    PlayCollisionSound(materialType.ToString() + "SFX", collision);
                }
            }

        }
        else
        {
            PlayCollisionSound("DefaultSFX", collision);
        }

    }

    void PlayCollisionSound (string sound, Collision collision)
    {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        float soundMultiplier = Mathf.Min(collisionForce / maximumColisionStrengh, 1);

        if (collisionForce > minimumColisionStrengh && audioHandler) //dont do it on death
        {
            string soundName = availableSounds.Contains(sound) ? sound : "DefaultSFX";
            audioHandler.Play(soundName, soundMultiplier);
        }
    }

}
