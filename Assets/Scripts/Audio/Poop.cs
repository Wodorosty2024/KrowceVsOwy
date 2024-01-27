using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Poop : MonoBehaviour
{
    [SerializeField] private EventReference poopSound;
    private AudioManager audioManager;
    private bool hasPlayedSound = false;

    public bool HasPlayedSound
    {
        get { return hasPlayedSound; }
        set { hasPlayedSound = value; }
    }

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        // if (audioManager != null && !hasPlayedSound)
        // {
        //     audioManager.PlayPoopSound(transform.position);
        //     Debug.LogError("POOP");
        //     hasPlayedSound = true;
        // }
        // else
        // {
        //     Debug.LogError("AudioManager not found in the scene or sound already played.");
        // }
    }
}
