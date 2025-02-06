using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseGeneral;
    [SerializeField] private GameObject pauseSound;

    [SerializeField] private AudioSource music;

    public bool isPaused = false;
    private bool musicWasPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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

    public void ApplyChanges()
    {
        pauseGeneral.SetActive(true);
        pauseSound.SetActive(false);
    }

    public void CancelChanges()
    {
        pauseGeneral.SetActive(true);
        pauseSound.SetActive(false);
    }

}
