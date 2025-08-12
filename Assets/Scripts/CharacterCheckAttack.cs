using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCheckAttack : MonoBehaviour
{
    public static CharacterCheckAttack instance;

    public enum BeingAttacked
    {
        BeingAttacked,
        NotBeingAttacked,
        Captured
    }

    public BeingAttacked beingAttacked = BeingAttacked.NotBeingAttacked;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
    {

        if (CharacterController.instance.movement != CharacterController.Movement.Fainted && CharacterController.instance.gameState == CharacterController.GameState.Playing)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                beingAttacked = BeingAttacked.BeingAttacked;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            beingAttacked = BeingAttacked.NotBeingAttacked;
        }
    }
}
