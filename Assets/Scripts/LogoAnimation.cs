using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float animSpeed = 1;

    private int animSign = 1;

    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (rectTransform.localScale.y >= 1.07f && animSign == 1)
        {
            animSign = -1;
        }else if (rectTransform.localScale.y <= 0.93f && animSign == -1)
        {
            animSign = 1;
        }

        rectTransform.localScale += new Vector3(1,1,1)*Time.deltaTime*animSpeed*animSign;


    }
}
