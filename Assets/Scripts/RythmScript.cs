using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static RythmScript;

public class RythmScript : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Intervals[] intervals;
    [SerializeField] private GameObject playButton;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        playButton.SetActive(false);
        musicSource.Play();
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

        private int firstSequenceIndex = 0;
        private int lastSequenceIndex = 3;

        private int kick = 0;
        private int lastInterval = -1;
        private int bar = 1;

        [SerializeField]
        private bool[,] sequences = new bool[30,8] {
            //1
            {false,false,false,false,true,false,false,false},
            {false,false,false,false,false,false,true,false},
            {true,false,false,false,false,false,false,false},
            {false,false,true,false,false,false,false,false},

            //2
            {false,false,true,false,true,false,false,false},
            {false,false,false,false,true,false,true,false},
            {false,false,false,true,false,false,true,false},
            {false,false,true,false,false,true,false,false},
            {false,false,false,true,false,true,false,false},
            {false,false,false,false,true,false,false,true},
            {true,false,false,false,true,false,false,false},
            {true,false,true,false,false,false,false,false},
            {true,true,false,false,false,false,false,false},
            {true,false,false,false,false,false,true,false},

            //3
            {false,false,true,false,true,false,true,false},
            {false,true,false,true,false,false,true,false},
            {false,false,true,false,false,true,true,false},
            {false,false,true,false,false,false,true,true},
            {true,false,true,false,true,false,false,false},
            {true,false,false,false,true,false,true,false},
            {true,false,false,true,false,false,true,false},
            {true,false,true,false,false,true,false,false},
            {true,false,false,true,false,true,false,false},
            {true,false,false,false,true,false,false,true},

            //4
            {true,false,true,false,true,false,true,false},
            {true,true,false,true,false,false,true,false},
            {true,false,true,false,false,true,true,false},
            {true,false,false,false,true,true,false,true},
            {false,false,false,false,true,true,true,true},
            {false,true,true,false,false,true,true,false},
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
                //Routine rythmique (Pour un pattern, joue puis écoute la mesure)
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
                    //Changement de mesure
                    kick = 0;
                    if (isPattern)
                    {
                        if (isListening)
                        {

                            //Tirage de la séquence à jouer
                            int randomRow = Random.Range(firstSequenceIndex, lastSequenceIndex);

                            for (int i = 0; i < sequence.Length; i++)
                            {
                                Debug.Log("Row : " + randomRow);
                                sequence[i] = sequences[randomRow, i]; // Copier chaque élément de la ligne
                            }

                            //Augmentation de la difficulté en fonction du nombre de mesures passées
                            bar++;
                            if (bar == 8)
                            {
                                lastSequenceIndex = 13;
                            }else if (bar == 24)
                            {
                                firstSequenceIndex = 4;
                                lastSequenceIndex = 23;
                            }else if (bar == 48)
                            {
                                firstSequenceIndex = 14;
                                lastSequenceIndex = sequences.GetLength(0);
                                Debug.Log("last index : " + lastSequenceIndex);
                            }else if (bar == 64)
                            {
                                firstSequenceIndex = 21;
                            } else if (bar == 88)
                            {
                                firstSequenceIndex = 0;
                                lastSequenceIndex = 1;

                            } else if (bar == 90)
                            {
                                //WIN
                                GameObject.Find("ScoreManager").GetComponent<ScoreScript>().Win();
                            }
                        }
                        isListening = !isListening;
                    }
                }
                
            }

        }
        
    }

}
