using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static RythmScript;

//Classe permettant de gérer la barre rythmique
public class RythmBarBehaviour : MonoBehaviour
{
    #region External References
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject beatWriter;
    [SerializeField] private GameObject beats;
    [SerializeField] private PauseScript pauseScript;
    #endregion
    #region Variables
    private bool isListening = false;
    private bool timeToReset = false;

    private float oldTimeSample;

    private Vector3 beatWriterInitPosition;

    [SerializeField]
    private float beatSpeed = 50;
    #endregion
    // Start is called before the first frame update
    private void Start()
    {
        beatWriterInitPosition = beatWriter.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!pauseScript.isPaused)
        {
            beatWriter.transform.position = new Vector3(beatWriter.transform.position.x + (musicSource.timeSamples - oldTimeSample) * beatSpeed * Screen.width/1000, beatWriterInitPosition.y, beatWriterInitPosition.z);
            oldTimeSample = musicSource.timeSamples;
        }
    }

    /// <summary>
    /// Repositionne le beatWriter en début de ligne
    /// </summary>
    public void ResetBeatWriterPos()
    {

        beatWriter.transform.position = beatWriterInitPosition;
        if (timeToReset)
        {
            ResetBeats();
        }

        if (isListening)
        {
            timeToReset = true;
        }
        isListening = true;
    }

    /// <summary>
    /// Retire les beats
    /// </summary>
    private void ResetBeats()
    {
        for (int i=0; i< beats.transform.childCount; i++)
        {
            beats.transform.GetChild(i).gameObject.SetActive(false);
        }
        isListening= false;
        timeToReset= false;
    }

    /// <summary>
    /// Ecrit les beats
    /// </summary>
    public void WriteBeat()
    {
        int i = 0;
        bool beatWritten = false;
        while (i < beats.transform.childCount && !beatWritten)
        {
            if (!beats.transform.GetChild(i).gameObject.activeSelf)
            {
                beats.transform.GetChild(i).position = beatWriter.transform.position;
                beats.transform.GetChild(i).gameObject.SetActive(true);
                beatWritten = true;
            }
            i++;
        }
    }
}
