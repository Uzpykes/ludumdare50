using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> ZombieIdleClips = new List<AudioClip>();
    public List<AudioClip> ZombieAttackClips = new List<AudioClip>();
    public List<AudioClip> AtmosphereClips = new List<AudioClip>();
    public List<AudioClip> HandgunClips = new List<AudioClip>();
    public List<AudioClip> MachineGunClips = new List<AudioClip>();
    public List<AudioClip> RocketClips = new List<AudioClip>();
    public List<AudioClip> ExplosionClips = new List<AudioClip>();

    public AudioSource EffectsSource;

    public static AudioManager Instance;
    private void OnEnable()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    public void PlayRandomZombieIdleClip()
    {
        var clip = GetRandomClip(ZombieIdleClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomZombieAttackClip()
    {
        var clip = GetRandomClip(ZombieAttackClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomAtmosphereClip()
    {
        var clip = GetRandomClip(AtmosphereClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomHandgunClip()
    {
        var clip = GetRandomClip(HandgunClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomRocketClip()
    {
        var clip = GetRandomClip(RocketClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomMachinegunClip()
    {
        var clip = GetRandomClip(MachineGunClips);

        EffectsSource.PlayOneShot(clip);
    }

    public void PlayRandomExplossionClip()
    {
        var clip = GetRandomClip(ExplosionClips);

        EffectsSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(List<AudioClip> clips)
    {
        if (clips.Count == 1)
            return clips[0];

        var id = Random.Range(0, clips.Count - 1);
        var result = clips[id];
        var tmp = clips[clips.Count - 1];
        clips[clips.Count - 1] = clips[id];
        clips[id] = tmp;

        return result;
    }

}