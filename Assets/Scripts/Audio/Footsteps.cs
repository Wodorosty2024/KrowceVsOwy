using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Footsteps : MonoBehaviour
{
    public EventReference Event;
     private PlayerController playerController;
    private StudioEventEmitter Emitter;
    public void TriggerEvent()
    {
        playerController = PlayerController.instance;

    Emitter = gameObject.AddComponent<StudioEventEmitter>();


    Emitter.EventReference = Event;

    Emitter.Play();

    }
}
