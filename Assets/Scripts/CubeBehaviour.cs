using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour
{
    private Vector3 startSize;
    private AudioSource audioSource;

    [SerializeField] float pulseSize;
    [SerializeField] float pulseRelease;


    // Start is called before the first frame update
    void Start()
    {
        startSize = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,startSize,Time.deltaTime * pulseRelease);
    }


    public void Pulse()
    {
        transform.localScale = startSize * pulseSize;
        audioSource.Play();
    }
}

