using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{

    [SerializeField] private CubeBehaviour cube;

    [SerializeField] private float scoreTimeMargin = 1f;
    [SerializeField] private float detectionTimeMargin = 2f;
    [SerializeField] private PauseScript pauseScript;

    private float lastClick = 0f;
    private float kickTime = 0f;
    private float kickClickDif = 0f;
    private bool newKickToCheck = false;

    [SerializeField] private ScoreScript scoreScript;

    // Update is called once per frame
    private void Update()
    {
        //active le menu pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScript.PauseClicked();
        }

        //coup de couteau et vérification de précision
        if (!pauseScript.isPaused)
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
                        scoreScript.GoodClick();
                    }
                    else
                    {
                        scoreScript.BadClick();
                    }
                    lastClick = 0;
                    newKickToCheck = false;
                }
                else if (kickTime + detectionTimeMargin < Time.time)
                {
                    scoreScript.MissClick();
                    newKickToCheck = false;
                }
            }
        }
    }



    public void updateKickTime(float newKickTime)
    {
        kickTime = newKickTime;
        newKickToCheck = true;
    }
}
