using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, .5f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	[HideInInspector]
	public float dopplerLevel = 1f;

    [HideInInspector]
    public float spread = 20f;

	[Tooltip("0 is 2D omnipresent sound, 1 is 3d localized sound")]
    [Range(0, 1f)]
    public float spatialBlend = 1f;

	public bool loop = false;


	[HideInInspector]
	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}


[System.Serializable]
public class SoundGroup
{

	public string name;

	public AudioClip[] clips;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, .5f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	[HideInInspector]
	public float dopplerLevel = 1f;

	[HideInInspector]
	public float spread = 20f;

	[Tooltip("0 is 2D omnipresent sound, 1 is 3d localized sound")]
	[Range(0, 1f)]
	public float spatialBlend = 1f;

	public bool loop = false;


	[HideInInspector]
	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}
