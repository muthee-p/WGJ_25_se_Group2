using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClose : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (CharacterCheckAttack.instance.beingAttacked == CharacterCheckAttack.BeingAttacked.Captured && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            audioSource.Stop();
        }
    }

}
