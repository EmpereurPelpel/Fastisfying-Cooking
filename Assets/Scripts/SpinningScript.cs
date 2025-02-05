using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0,spinSpeed * Time.deltaTime,0);
    }
}
