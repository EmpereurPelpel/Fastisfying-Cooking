using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackgroundScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1;


    private Vector3 startPos = new Vector3(0, 10, -5.5f);

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(new Vector3(0, - scrollSpeed * Time.deltaTime, 0));
        if (transform.position.y <=-10 ) 
        {
            ResetPos();
        }
    }

    private void ResetPos()
    {
        transform.position = startPos;
    }
}
