using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] AudioSource lightAudioSource;
    [SerializeField] AudioSource heavyAudioSource;

    [SerializeField] List<AudioClip> gunFireClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> pressClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> collideClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> dashClips = new List<AudioClip>();
    [SerializeField] AudioClip explosionClip;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayGunFireSFX()
    {
        lightAudioSource.PlayOneShot(gunFireClips[Random.Range(0, gunFireClips.Count)]);
    }

    public void PlayPressSFX()
    {
        lightAudioSource.PlayOneShot(pressClips[Random.Range(0, pressClips.Count)]);
    }

    public void PlayCollideSFX()
    {
        lightAudioSource.PlayOneShot(collideClips[Random.Range(0, collideClips.Count)]);
    }

    public void PlayDashSFX()
    {
        lightAudioSource.PlayOneShot(dashClips[Random.Range(0, dashClips.Count)]);
    }

    public void PlayExplosionSFX()
    {
        heavyAudioSource.PlayOneShot(explosionClip);
    }
}
