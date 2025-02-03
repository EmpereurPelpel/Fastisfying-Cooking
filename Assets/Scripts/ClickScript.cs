using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{

    [SerializeField] private CubeBehaviour cube;

    [SerializeField] private float scoreTimeMargin = 1f;
    [SerializeField] private float detectionTimeMargin = 2f;

    private float lastClick = 0f;
    private float kickTime = 0f;
    private float kickClickDif = 0f;
    private bool newKickToCheck = false;

    [SerializeField] private ScoreScript scoreScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cube.Pulse();
            lastClick = Time.time;
        }

        if (newKickToCheck)
        {
            kickClickDif = Mathf.Abs(kickTime - lastClick);
            if (kickClickDif < detectionTimeMargin)
            {
                if (kickClickDif < scoreTimeMargin)
                {
                    Debug.Log("OK");
                    scoreScript.GoodClick();
                }
                else
                {
                    Debug.Log("NUL");
                    scoreScript.BadClick();
                }
                lastClick = 0;
                newKickToCheck = false;
            } else if (kickTime + detectionTimeMargin < Time.time)
            {
                Debug.Log("MISS");
                scoreScript.MissClick();
                newKickToCheck = false;
            }
        }
    }



    public void updateKickTime(float newKickTime)
    {
        kickTime = newKickTime;
        newKickToCheck = true;
    }
}
