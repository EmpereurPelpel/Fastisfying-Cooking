using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static RythmScript;

public class RythmScript : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Intervals[] intervals;
    [SerializeField] private bool playMusic = false;

    private float tmpTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playMusic && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        if (musicSource.isPlaying)
        {
            foreach (Intervals interval in intervals)
            {
                float sampledTime = musicSource.timeSamples / (musicSource.clip.frequency * interval.GetIntervalLength(bpm));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }

    public void playMusicNow()
    {
        playMusic = true;
    }

    public float getKickTime()
    {
        return intervals[1].kickTime;
    }

    [System.Serializable]
    public class Intervals
    {
        [SerializeField] private float steps;
        [SerializeField] private UnityEvent trigger;
        [SerializeField] private float delay = 0;
        [SerializeField] private bool isPattern;
        [SerializeField] private bool[] sequence;
        [SerializeField] private bool isListening = false;

        private int kick = 0;
        private int lastInterval = -1;

        private static bool[,] sequences = new bool[10,8] {
            {true,false,false,false,false,false,false,false},
            {true,false,false,false,true,false,false,false},
            {true,false,true,false,true,false,false,false},
            {true,false,false,false,true,false,true,false},
            {true,false,true,false,true,false,true,false},
            {true,false,false,true,false,false,true,false},
            {true,true,false,true,false,false,true,false},
            {true,false,true,false,false,true,false,false},
            {true,false,false,true,false,true,false,false},
            {true,false,false,false,true,false,false,true},
        };

        public float kickTime = 0f;

        private void Start()
        {
        }

        public float GetIntervalLength(float bpm)
        {
            return 60f / (bpm * steps);
        }


        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval + delay) != lastInterval)
            {
                lastInterval = Mathf.FloorToInt(interval);
                if ((sequence[kick] && isPattern) || !isPattern)
                {
                    if (!isListening)
                    {
                        trigger.Invoke();
                    }
                    else
                    {
                        kickTime = Time.time;
                        GameObject.Find("ClickManager").GetComponent<ClickScript>().updateKickTime(kickTime);
                        
                    }
                } 
                if (kick < sequence.Length-1)
                {
                    kick++;
                }else
                {
                    kick = 0;
                    if (isPattern)
                    {
                        if (isListening)
                        {

                            int randomRow = Random.Range(0, sequences.GetLength(0));

                            for (int i = 0; i < sequence.Length; i++)
                            {
                                sequence[i] = sequences[randomRow, i]; // Copier chaque élément de la ligne
                            }
                        }
                        isListening = !isListening;
                    }
                }
                
            }

        }
        
    }

}
