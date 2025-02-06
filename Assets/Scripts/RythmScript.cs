using System;
using Unity.VisualScripting;
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

    [SerializeField] private PauseScript pauseScript;
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (musicSource.isPlaying && !pauseScript.isPaused)
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

    public float GetIntervalLength()
    {
        return intervals[1].GetIntervalLength(bpm);
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
        [SerializeField] private ObjectSpawner spawner;

        private int firstSequenceIndex = 0;
        private int lastSequenceIndex = 3;

        private int kick = 0;
        private int lastInterval = -1;
        private int bar = 1;

        private bool kickMarked = false;
        private bool hasWaited = false;

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


        public float GetIntervalLength(float bpm)
        {
            return 60f / (bpm * steps);
        }


        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval + delay) != lastInterval)
            {
                //Routine rythmique (Pour un pattern, joue puis �coute la mesure)
                lastInterval = Mathf.FloorToInt(interval);

                //si c'est un pattern et que soit l'intervalle est plus grande que la pr�c�dente 
                if (isPattern && kick!= Mathf.FloorToInt(lastInterval / 2)%(int)steps)
                {
                    kick = (int)Mathf.Floor(lastInterval/2) % (int)steps;
                    kickMarked = false;
                }
                if ((sequence[kick] && isPattern && !kickMarked) || !isPattern)
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
                    kickMarked = true;
                } 
           
                if (kick == sequence.Length-1)
                {
                    //Changement de mesure
                    //kick = 0;
                    if (isPattern && hasWaited)
                    {
                        hasWaited = false;
                        if (isListening)
                        {
                            InitiateSequence();

                            DifficultyManager();
                        }
                        isListening = !isListening;
                    }
                    else
                    {
                        hasWaited = true;
                    }
                }
                
            }

        }
        private void InitiateSequence()
        {
            //Tirage de la s�quence � jouer
            int randomRow = UnityEngine.Random.Range(firstSequenceIndex, lastSequenceIndex);

            for (int i = 0; i < sequence.Length; i++)
            {
                Debug.Log("Row : " + randomRow);
                sequence[i] = sequences[randomRow, i]; // Copier chaque �l�ment de la ligne
            }
            // Deleting old food and creating a new one for the next sequence
            
            int numberOfCuts = 0;
            foreach (bool b in sequence)
            {
                if (b) numberOfCuts++;
            }
            spawner.SpawnNewObject(numberOfCuts);
            spawner.ResetObject();
        }

        private void DifficultyManager()
        {
            //Augmentation de la difficult� en fonction du nombre de mesures pass�es
            bar++;
            if (bar == 8)
            {
                lastSequenceIndex = 13;
            }
            else if (bar == 24)
            {
                firstSequenceIndex = 4;
                lastSequenceIndex = 23;
            }
            else if (bar == 48)
            {
                firstSequenceIndex = 14;
                lastSequenceIndex = sequences.GetLength(0);
                //Debug.Log("last index : " + lastSequenceIndex);
            }
            else if (bar == 64)
            {
                firstSequenceIndex = 21;
            }
            else if (bar == 88)
            {
                firstSequenceIndex = 0;
                lastSequenceIndex = 1;

            }
            else if (bar == 90)
            {
                //WIN
                GameObject.Find("ScoreManager").GetComponent<ScoreScript>().Win();
            }
        }
        
    }

}
