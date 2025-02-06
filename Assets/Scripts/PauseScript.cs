using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Classe gérant le menu pause
public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseGeneral;
    [SerializeField] private GameObject pauseSound;

    [SerializeField] private AudioSource music;

    [SerializeField] AudioMixer audioMixerGroup;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;

    public bool isPaused = false;
    private bool musicWasPlaying = false;
    // Start is called before the first frame update
    private void Start()
    {
        LoadVolume();
    }

    //Active ou désactive le menu pause quand elle est appelée
    public void PauseClicked()
    {
        pauseGeneral.SetActive(true);
        pauseSound.SetActive(false);
        pausePanel.SetActive(!pausePanel.activeSelf);
        isPaused = !isPaused;
        if (isPaused && music.isPlaying)
        {
            music.Pause();
            musicWasPlaying=true;
        }
        else if (musicWasPlaying)
        {
            music.Play();
            musicWasPlaying=false;
        }
    }

    public void OpenSoundMenu()
    {
        pauseGeneral.SetActive(false);
        pauseSound.SetActive(true);
    }

    public void GoBack()
    {
        pauseGeneral.SetActive(true);
        pauseSound.SetActive(false);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixerGroup.SetFloat("Music",Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MusicVolume", volume);  
    }

    public void SetFXVolume()
    {
        float volume = fxSlider.value;
        audioMixerGroup.SetFloat("FX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("FXVolume", volume);   
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
        fxSlider.value = PlayerPrefs.GetFloat("FXVolume");
        SetFXVolume();
    }

}
