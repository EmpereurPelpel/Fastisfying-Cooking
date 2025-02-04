using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI mult;
    [SerializeField] private TextMeshProUGUI textFeedback;
    [SerializeField] private LifeBarBehaviour lifeBar;

    [SerializeField] private GameObject deathPanel;

    [SerializeField] private AudioSource music;
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
        lifeBar.HealDamage();
    }

    public void BadClick()
    {
        textFeedback.text = "BAD";
        goodStreak = 0;
        multCount = 1;
        mult.text = "X" + multCount.ToString();
        lifeBar.TakeDamage();
    }

    public void MissClick() 
    {
        textFeedback.text = "MISS";
        goodStreak = 0;
        multCount = 1;
        mult.text = "X" + multCount.ToString();
        lifeBar.TakeDamage();

    }

    public void Death()
    {
        Debug.Log("mort");
        music.Stop();
        deathPanel.SetActive(true);

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }




}
