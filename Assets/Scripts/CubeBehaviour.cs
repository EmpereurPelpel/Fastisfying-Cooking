using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe te test conserv�e pour la gestion des sfx
public class CubeBehaviour : MonoBehaviour
{
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Pulse()
    {
        audioSource.Play();
    }
}

