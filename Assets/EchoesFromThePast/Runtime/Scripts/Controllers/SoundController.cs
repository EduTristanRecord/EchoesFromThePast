using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public AudioSource sfx;

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
        
    }

    //Sfx 8 Sound Effect - Noah Smith
    public void SoundEffect(AudioClip clip)
    {
        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(clip);
    }
}
