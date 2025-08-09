using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasHealthScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthScript.instance.RefillHealth();
            GameController.instance.UpdateLastCheckpoint();
        }
    }
   
   void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthScript.instance.RestartRoutineDrainHealth();
        }
    }
}
