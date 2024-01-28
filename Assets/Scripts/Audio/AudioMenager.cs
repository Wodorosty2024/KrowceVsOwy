using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public EventReference backgroundMusicEvent; // Dźwięk muzyki w tle
    public EventReference poopSound; // Dźwięk poop

    private static AudioManager _instance;
    private FMOD.Studio.EventInstance musicEventInstance;

   public EventReference slowEvent;
    public EventReference midEvent;
    public EventReference fastEvent;

    private PlayerController playerController;
    private StudioEventEmitter slowEmitter;
    private StudioEventEmitter midEmitter;
    private StudioEventEmitter fastEmitter;

    private static bool isAlreadyOn = false;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayMusic();
    }

    void PlayMusic()
    {
        if(isAlreadyOn == false)
        {
            isAlreadyOn = true;
            // Inicjalizacja i odtwarzanie instancji zdarzenia dźwiękowego FMOD dla muzyki w tle
            musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(backgroundMusicEvent);
            // Ustawienie instancji zdarzenia na pętlę (odtwarzanie w nieskończoność)
            musicEventInstance.start();
            Debug.Log("PLAY MUSIC");
        }
    }

    // Funkcja do zatrzymywania muzyki (jeśli to konieczne)
    void StopMusic()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicEventInstance.release(); // Zwolnienie zasobów instancji
        }
    }

  void Start()
{
    playerController = PlayerController.instance;
    slowEmitter = gameObject.AddComponent<StudioEventEmitter>();
    midEmitter = gameObject.AddComponent<StudioEventEmitter>();
    fastEmitter = gameObject.AddComponent<StudioEventEmitter>();

    slowEmitter.EventReference = slowEvent;
    midEmitter.EventReference = midEvent;
    fastEmitter.EventReference = fastEvent;

    // Odtwarzaj eventy na start.
    // slowEmitter.Play();
    // midEmitter.Play();
    // fastEmitter.Play();

    // // Zatrzymaj eventy na początku, będziemy je uruchamiać w zależności od prędkości.
    // slowEmitter.Stop();
    // midEmitter.Stop();
    // fastEmitter.Stop();
}


void Update()
{
    if (playerController == null)
    {
        // Sprawdź ponownie, czy PlayerController został zainicjowany
        playerController = PlayerController.instance;
        return;
    }

    float speed = playerController.currentHorizontalSpeed;

    // if (speed == 0f)
    // {
    //     // Zatrzymaj odtwarzanie wszystkich dźwięków poruszania
    //     slowEmitter.Stop();
    //     midEmitter.Stop();
    //     fastEmitter.Stop();
    // }
    // else if (speed <= playerController.movementSpeedRange.x)
    // {
    //     // Play Slow Event
    //     if (!slowEmitter.IsPlaying())
    //     {
    //         slowEmitter.Play();
    //         midEmitter.Stop();
    //         fastEmitter.Stop();
    //     }
    // }
    // else if (speed >= playerController.movementSpeedRange.x && speed <= playerController.movementSpeedRange.y)
    // {
    //     // Play Mid Event
    //     if (!midEmitter.IsPlaying())
    //     {
    //         midEmitter.Play();
    //         slowEmitter.Stop();
    //         fastEmitter.Stop();
    //     }
    // }
    // else
    // {
    //     // Play Fast Event
    //     if (!fastEmitter.IsPlaying())
    //     {
    //         fastEmitter.Play();
    //         slowEmitter.Stop();
    //         midEmitter.Stop();
    //     }
    // }
}



    // Funkcja do odtwarzania dźwięku poop
    public void PlayPoopSound(Vector3 worldPos)
    {
        // // Odtwarzanie dźwięku poop na nowej instancji
        //  Debug.LogError("POOP in");
        // FMOD.Studio.EventInstance poopSoundInstance = FMODUnity.RuntimeManager.CreateInstance(poopSound);
        // poopSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(new Vector3(worldPos.x, worldPos.y, 0)));
        // poopSoundInstance.start();
        // poopSoundInstance.release(); // Zwolnienie zasobów instancji po odtworzeniu
        // Debug.LogError("POOP out");
    }
}
