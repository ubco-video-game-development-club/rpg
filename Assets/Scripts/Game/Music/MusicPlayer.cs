using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    [SerializeField] private AudioClip zoneTrack;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayZoneTrack() => instance.InstancePlayZoneTrack();

    private void InstancePlayZoneTrack()
    {
        if (zoneTrack != null)
        {
            audioSource.clip = zoneTrack;
            audioSource.Play();
        }
    }
}
