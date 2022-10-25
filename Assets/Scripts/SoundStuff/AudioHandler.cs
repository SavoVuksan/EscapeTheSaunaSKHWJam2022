using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum SoundType
{
    SFX,
    Music
}

public class AudioHandler : MonoBehaviour
{
    public SoundType soundType;

    public AudioMixerGroup mixerGroup;
    public string playOnStart;

    public Sound[] sounds;
    public SoundGroup[] soundGroups;
    private Dictionary<string, SoundGroup> soundGroupsDict;

    protected float volumeLevel = 1f;

    private GameObject audioSourcesOrigin;
    private GameObject audioSourcesParent;

    public float slowDownInfluence = 1f;

    // Auxiliary bool that will just prevent a few errors from occuring on application quit because Unity is dumb and runs Destroy an 'OnDisable' of everything 
    bool quitting = false;

    public float timeUntilAudioSourcesDestroyed = 2.0f;

    void Awake()
    {
        audioSourcesParent = new GameObject("AudioSourcesParent");
        audioSourcesParent.transform.parent = gameObject.transform;
        audioSourcesParent.transform.localPosition = new Vector3(0, 0, 0);

        audioSourcesOrigin = new GameObject("AudioSources - " + gameObject.name);
        ResetAudioSourcesTransform();

        //volumeLevel = soundType == SoundType.Music ?
        //    GameManager.Instance.savedData.musicVolumeLevel :
        //    GameManager.Instance.savedData.sfxVolumeLevel;

        foreach (Sound s in sounds)
        {
            s.source = audioSourcesOrigin.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
            s.source.dopplerLevel = s.dopplerLevel;
            s.source.spatialBlend = s.spatialBlend;
            s.source.spread = 10;
            s.source.minDistance = 2;
            s.source.maxDistance = 50;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.playOnAwake = false;
        }

        soundGroupsDict = new Dictionary<string, SoundGroup>();
        if (soundGroups != null)
        {
            foreach (SoundGroup g in soundGroups)
            {
                g.source = audioSourcesOrigin.AddComponent<AudioSource>();
                g.source.clip = null;
                g.source.loop = g.loop;

                g.source.outputAudioMixerGroup = mixerGroup;
                g.source.dopplerLevel = g.dopplerLevel;
                g.source.spatialBlend = g.spatialBlend;
                g.source.spread = 10;
                g.source.minDistance = 2;
                g.source.maxDistance = 50;
                g.source.pitch = g.pitch;
                g.source.volume = g.volume;
                g.source.playOnAwake = false;

                if (soundGroupsDict.ContainsKey(g.name))
                {
                    Debug.LogError("[AudioHandler] The name of sound groups must be unique!! " + g.name + " is a duplicate!");
                }
                else
                {
                    soundGroupsDict.Add(g.name, g);
                }
            }
        }
    }

    private void OnEnable()
    {
        //volumeLevel = soundType == SoundType.Music ?
        //    GameManager.Instance.savedData.musicVolumeLevel :
        //    GameManager.Instance.savedData.sfxVolumeLevel;

        ResetAudioSourcesTransform();
    }

    private void OnDisable()
    {
        if (!quitting) UnparentAudioSources();
    }

    private void UnparentAudioSources()
    {
        if (audioSourcesOrigin != null)
            audioSourcesOrigin.transform.parent = null;
    }

    private void ResetAudioSourcesTransform()
    {
        audioSourcesOrigin.transform.parent = audioSourcesParent.transform;
        audioSourcesOrigin.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        //GameManager gameManager = GameManager.Instance;
        //foreach (AudioSource source in GetComponents<AudioSource>())
        //{
        //    gameManager.OnPause.AddListener(source.Pause);
        //    gameManager.OnUnPause.AddListener(source.UnPause);
        //}

        if (!string.IsNullOrEmpty(playOnStart))
        {
            if (GetSound(playOnStart) != null)
            {
                Play(playOnStart);
            }
            else
            {
                PlayRandomFromGroup(playOnStart);
            }
        }

        //GameManager.Instance.timeManager.TimedSlowed.AddListener(PitchDownSounds);
        //GameManager.Instance.timeManager.TimeResumed.AddListener(ResetSoundsPitch);
    }

    float SlowDownMultiplier()
    {
        return 1 - (1 - Time.timeScale) * slowDownInfluence;
    }

    public void Play(string sound)
    {
        //Debug.Log("Playing " + sound);
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found in " + name);
            return;
        }

        s.source.volume = s.volume * volumeLevel * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f)) * SlowDownMultiplier();
        s.source.Play();
    }

    public IEnumerator PlayCoroutine(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s != null)
        {
            s.source.Play();
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => !s.source.isPlaying);
        }
        else
        {
            Debug.LogWarning("Sound: " + sound + " not found in " + name);
            yield return null;
        }
    }

    public void Play(string sound, float volumneMultiplier = 1f, float pitchMultiplier = 1f)
    {
        //Debug.Log("Playing " + sound);
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found in " + name);
            return;
        }

        s.source.volume = s.volume * volumeLevel * volumneMultiplier * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * pitchMultiplier * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f)) * SlowDownMultiplier();
        s.source.Play();
    }

    /// <summary>
    /// Untested, delete this coment once testes
    /// </summary>
    /// <param name="sounds"></param>
    public string PlayRandom(string[] sounds, float volumneMultiplier = 1f, float pitchMultiplier = 1f)
    {
        int soundToPlay = UnityEngine.Random.Range(0, sounds.Length);
        Play(sounds[soundToPlay], volumneMultiplier, pitchMultiplier);
        return sounds[soundToPlay];
    }

    public void PlayRandomWithPrefix(string prefix, float volumneMultiplier = 1f, float pitchMultiplier = 1f)
    {
        List<Sound> soundsToPlay = new List<Sound>();
        foreach (Sound sound in sounds)
        {
            if (sound.name.StartsWith(prefix)) soundsToPlay.Add(sound);
        }

        if (soundsToPlay.Count == 0)
        {
            Debug.LogWarning("[AudioHandler] Sound starting with " + prefix + " not found");
            return;
        }

        Sound s = soundsToPlay[UnityEngine.Random.Range(0, soundsToPlay.Count)];

        s.source.volume = s.volume * volumeLevel * volumneMultiplier * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * pitchMultiplier * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f)) * SlowDownMultiplier();
        s.source.Play();
    }

    public void PlayRandomFromGroup(string groupName, float volumneMultiplier = 1f, float pitchMultiplier = 1f)
    {
        if (!soundGroupsDict.ContainsKey(groupName))
        {
            Debug.LogWarning("[AudioHandler] Sound group " + groupName + " not found");
            return;
        }
        SoundGroup g = soundGroupsDict[groupName];

        if (g.clips.Length == 0)
        {
            Debug.LogWarning("[AudioHandler] Sound group " + groupName + " does not contain any audio files");
            return;
        }

        g.source.clip = g.clips[UnityEngine.Random.Range(0, g.clips.Length)];
        g.source.volume = g.volume * volumeLevel * volumneMultiplier * (1f + UnityEngine.Random.Range(-g.volumeVariance / 2f, g.volumeVariance / 2f));
        g.source.pitch = g.pitch * pitchMultiplier * (1f + UnityEngine.Random.Range(-g.pitchVariance / 2f, g.pitchVariance / 2f)) * SlowDownMultiplier();
        g.source.Play();
    }

    public void PlayFromGroupWithDelay(string sound, float delay)
    {
        StartCoroutine(PlayFromGroupWithDelayCoroutine(sound, delay));
    }

    IEnumerator PlayFromGroupWithDelayCoroutine(string sound, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomFromGroup(sound);
    }

    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public Sound GetSound(string sound)
    {
        return Array.Find(sounds, item => item.name == sound);
    }

    public AudioSource GetAudioSource(string sound)
    {
        return GetSound(sound).source;
    }

    public void UpdateVolumeLevelOfPlayingSounds(float multiplier)
    {
        volumeLevel = multiplier;
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * volumeLevel;
        }

        foreach (SoundGroup g in soundGroups)
        {
            g.source.volume = g.volume * volumeLevel;
        }
    }

    public void UpdateVolumeLevel(float multiplier)
    {
        volumeLevel = multiplier;
    }

    void OnApplicationQuit()
    {
        quitting = true;
    }

    private void OnDestroy()
    {
        // On destroy of the audio handler, the audio sources will get 'un-parented' (see OnDisable) and then destroyed after 2 seconds
        try
        {
            Destroy(audioSourcesOrigin, timeUntilAudioSourcesDestroyed);
        }
        catch (System.Exception e)
        {
            Debug.Log("[AudioHandler] Could not destroy audioSources, probably because they were already destroyed on level load. Error was: " + e.Message);
        }
    }

    public void ResetSoundsPitch(float timeToResume)
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                StartCoroutine(UpdatePitchCoroutine(s.source, timeToResume, s.source.pitch, s.pitch));
            }
        }
        foreach (SoundGroup g in soundGroups)
        {
            if (g.source.isPlaying)
            {
                StartCoroutine(UpdatePitchCoroutine(g.source, timeToResume, g.source.pitch, g.pitch));
            }
        }
    }


    public void PitchDownSounds(float timeToSlow)
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                StartCoroutine(UpdatePitchCoroutine(s.source, timeToSlow, s.source.pitch));
            }
        }
        foreach (SoundGroup g in soundGroups)
        {
            if (g.source.isPlaying)
            {
                StartCoroutine(UpdatePitchCoroutine(g.source, timeToSlow, g.source.pitch));
            }
        }
    }

    IEnumerator UpdatePitchCoroutine(AudioSource source, float timeToTransition, float startingPitch, float targetPitch)
    {
        float time = 0;
        while (time <= timeToTransition)
        {
            source.pitch = Mathf.Lerp(startingPitch, targetPitch, time / timeToTransition);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void LerpSoundVolume(string soundName, float timeToTransition, float targetVolumeMultipler)
    {
        Sound s = GetSound(soundName);
        StartCoroutine(UpdateVolumeCoroutine(s.source, timeToTransition, s.source.volume, s.volume * volumeLevel * targetVolumeMultipler));
    }

    IEnumerator UpdateVolumeCoroutine(AudioSource source, float timeToTransition, float startingVolume, float targetVolume)
    {
        float time = 0;
        while (time <= timeToTransition)
        {
            source.volume = Mathf.Lerp(startingVolume, targetVolume, time / timeToTransition);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    IEnumerator UpdatePitchCoroutine(AudioSource source, float timeToTransition, float startingPitch)
    {
        // If no targetPitch is inputted, then use timeScale
        float time = 0;
        while (time <= timeToTransition)
        {
            source.pitch = startingPitch * Time.timeScale;
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
