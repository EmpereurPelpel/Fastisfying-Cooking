using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarBehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 100f)]
    private float fillAmount = 100;

    private float maxFillAmount = 100;

    private float lifePassiveDmg = 1.5f;
    private float lifeActiveDmg = 20;
    private float lifeHealAmount = 3f;

    private float barWidth;

    [SerializeField] private AudioSource music;

    [SerializeField] private ScoreScript scoreScript;

    [SerializeField] private PauseScript pauseScript;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        barWidth = rectTransform.rect.width;

    }

    // Update is called once per frame
    private void Update()
    {
        if (music.isPlaying && !pauseScript.isPaused)
        {
            if (fillAmount > 0f)
            {
                fillAmount -= lifePassiveDmg * Time.deltaTime;
            }
            else
            {
                scoreScript.Death();
            }
        }

        rectTransform.sizeDelta = new Vector2(barWidth * fillAmount/maxFillAmount , rectTransform.rect.height);
    }


    public void TakeDamage()
    {
        fillAmount -= lifeActiveDmg;
    }

    public void HealDamage()
    {
        if(maxFillAmount - fillAmount < lifeHealAmount)
        {
            fillAmount = maxFillAmount;
        }else
        {
            fillAmount += lifeHealAmount;
        }
    }
}
