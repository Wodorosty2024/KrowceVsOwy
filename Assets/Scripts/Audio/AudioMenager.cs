using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    // [FMODUnity.EventRef]
    // public string musicEvent; // Zmienna przechowująca ścieżkę dźwiękową FMOD
    public EventReference EventName;

    FMOD.Studio.EventInstance musicEventInstance;

    void Start()
    {
        PlayMusic();
    }

    void PlayMusic()
    {
        // Inicjalizacja i odtwarzanie instancji zdarzenia dźwiękowego FMOD
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(EventName);

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
}