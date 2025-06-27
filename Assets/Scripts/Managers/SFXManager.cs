using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] List<AudioClip> pressClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> collideClips = new List<AudioClip>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPressSFX()
    {
        audioSource.PlayOneShot(pressClips[Random.Range(0, pressClips.Count)]);
    }

    public void PlayCollideSFX()
    {
        audioSource.PlayOneShot(collideClips[Random.Range(0, collideClips.Count)]);
    }
}
