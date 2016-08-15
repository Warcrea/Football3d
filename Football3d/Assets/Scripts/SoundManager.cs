using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

    public AudioClip kick;
    public AudioClip pass;
    private new AudioSource audio;

    public void Start() {
        audio = GetComponent<AudioSource>();
    }

    public void KickSound() {
        audio.PlayOneShot(kick);
    }

    public void PassSound() {
        audio.PlayOneShot(pass);
    }
}