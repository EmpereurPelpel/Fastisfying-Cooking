using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI mult;
    [SerializeField] private TextMeshProUGUI textFeedback;
    private int scoreCount = 0;
    private int multCount = 1;
    private int goodStreak = 0;
    private int multStep = 4;
    private int scoreStep = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GoodClick()
    {
        textFeedback.text = "GOOD";
        goodStreak++;
        if (goodStreak == multStep*multCount)
        {
            multCount *=2;
            mult.text = "X" + multCount.ToString();

        }
        scoreCount += scoreStep * multCount;
        score.text = "SCORE : " + scoreCount.ToString();
    }

    public void BadClick()
    {
        textFeedback.text = "BAD";
        goodStreak = 0;
        multCount = 1;
        mult.text = "X" + multCount.ToString();
    }

    public void MissClick() 
    {
        textFeedback.text = "MISS";
        goodStreak = 0;
        multCount = 1;
        mult.text = "X" + multCount.ToString();

    }




}
