using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private IngameMenuController iMC;
    string currentScene;

    [Header("Ajustes de Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Text textoMaster;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Toggle toggleMaster;
    [SerializeField] private TMP_Text textoMusic;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Toggle toggleMusic;
    [SerializeField] private TMP_Text textoSFX;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Toggle toggleSFX;

    [SerializeField] private GameObject confirmationPopUp;

    private float masterVolume;
    private float originalMasterVolume;
    private float musicVolume;
    private float originalMusicVolume;
    private float SFXVolume;
    private float originalSFXVolume;

    public bool masterAudio;
    public bool musicAudio;
    public bool sfxAudio;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Estableciendo audio
        audioMixer = Resources.FindObjectsOfTypeAll<AudioMixer>()[0];

        masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        sliderMaster.value = masterVolume;
        sliderMusic.value = musicVolume;
        sliderSFX.value = SFXVolume;

        masterAudio = true;
        musicAudio = true;
        sfxAudio = true;

        SetMaster(masterVolume);
        SetMusic(musicVolume);
        SetSFX(SFXVolume);
    }
    public void RecopilarValores() // Recopila valores al entrar en el menú de audio.
    {
        originalMasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        originalMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        originalSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
    }
    public float AjusteVolumen(float volume) // Transforma el audio para que sea más realista.
    {
        float finalVolume;
        if (volume == 0)
        {
            finalVolume = -80;
        }
        else
        {
            finalVolume = 20 * Mathf.Log10(volume);
        }
        return finalVolume;
    }
    public void Toggle(string audio) // Silencia/Desilencia los distintos  sonidos.
    {
        if (audio == "master")
        {
            masterAudio = !masterAudio;
            if (masterAudio)
            {
                SetMaster(sliderMaster.value);
            }
            else
            {
                SetMaster(0);
                float textVolume = sliderMaster.value * 100;
                textoMaster.text = textVolume.ToString("  0");
            }
        }
        else if (audio == "music")
        {
            musicAudio = !musicAudio;
            if (musicAudio)
            {
                SetMusic(sliderMusic.value);
            }
            else
            {
                SetMusic(0);
                float textVolume = sliderMusic.value * 100;
                textoMusic.text = textVolume.ToString("  0");
            }
        }
        else if (audio == "sfx")
        {
            sfxAudio = !sfxAudio;
            if (sfxAudio)
            {
                SetSFX(sliderSFX.value);
            }
            else
            {
                SetSFX(0);
                float textVolume = sliderSFX.value * 100;
                textoSFX.text = textVolume.ToString("  0");
            }
        }
    }
    public void SetMaster(float volume) // Audio master.
    {
        if (masterAudio)
        {
            masterVolume = volume;
            audioMixer.SetFloat("MasterVolume", AjusteVolumen(volume));
            float textVolume = volume * 100;
            textoMaster.text = textVolume.ToString("  0");
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", -80);
            float textVolume = volume * 100;
            textoMaster.text = textVolume.ToString("  0");
        }
    }
    public void SetMusic(float volume) // Audio música.
    {
        if (musicAudio)
        {
            musicVolume = volume;
            audioMixer.SetFloat("MusicVolume", AjusteVolumen(volume));
            float textVolume = volume * 100;
            textoMusic.text = textVolume.ToString("  0");
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
            float textVolume = volume * 100;
            textoMusic.text = textVolume.ToString("  0");
        }
    }
    public void SetSFX(float volume) // Audio SFX.
    {
        if (sfxAudio)
        {
            SFXVolume = volume;
            audioMixer.SetFloat("SFXVolume", AjusteVolumen(volume));
            float textVolume = volume * 100;
            textoSFX.text = textVolume.ToString("  0");
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", -80);
            float textVolume = volume * 100;
            textoSFX.text = textVolume.ToString("  0");
        }
    }
    public void ConfirmacionAudio() // Saca el PopUp de confirmación de cambio de audio. Botón Volver.
    {
        float currentMasterVolume = sliderMaster.value;
        float currentMusicVolume = sliderMusic.value;
        float currentSFXVolume = sliderSFX.value;
        if (currentMasterVolume != originalMasterVolume || currentMusicVolume != originalMusicVolume || currentSFXVolume != originalSFXVolume)
        {
            confirmationPopUp.SetActive(true);
        }
        else
        {
            audioMenu.SetActive(false);
            mainMenu.SetActive(true);
            if (currentScene == "Game")
            {
                iMC.OnMenuBool(false);
            }
        }
    }
    public void Revertir() // Revierte todo cambio hecho al denegar la confirmación.
    {
        sliderMaster.value = originalMasterVolume;
        sliderMusic.value = originalMusicVolume;
        sliderSFX.value = originalSFXVolume;
    }
    public void Restablecer() // Pone los sliders en el valor base. Botón Reset.
    {
        sliderMaster.value = 0.5f;
        sliderMusic.value = 0.5f;
        sliderSFX.value = 0.5f;
    }
    public void Apply() // Aplica los cambios hechos. Botón Aplicar y aceptar la confirmación.
    {
        SetMaster(sliderMaster.value);
        originalMasterVolume = masterVolume;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);

        SetMusic(sliderMusic.value);
        originalMusicVolume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        SetSFX(sliderSFX.value);
        originalSFXVolume = SFXVolume;
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }
}
