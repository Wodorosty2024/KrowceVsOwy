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
        // Inicjalizacja i odtwarzanie instancji zdarzenia dźwiękowego FMOD dla muzyki w tle
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(backgroundMusicEvent);

        // Ustawienie instancji zdarzenia na pętlę (odtwarzanie w nieskończoność)
        musicEventInstance.start();
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

    // Funkcja do odtwarzania dźwięku poop
    public void PlayPoopSound(Vector3 worldPos)
    {
        // Odtwarzanie dźwięku poop na nowej instancji
         Debug.LogError("POOP in");
        FMOD.Studio.EventInstance poopSoundInstance = FMODUnity.RuntimeManager.CreateInstance(poopSound);
        poopSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(new Vector3(worldPos.x, worldPos.y, 0)));
        poopSoundInstance.start();
        poopSoundInstance.release(); // Zwolnienie zasobów instancji po odtworzeniu
        Debug.LogError("POOP out");
    }
}
