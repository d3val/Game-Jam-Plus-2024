using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayFromList : MonoBehaviour
{
    public List<AudioClip> clips;
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        int index;
        index = Random.Range(0, clips.Count);
        source.PlayOneShot(clips[index]);
    }
}
