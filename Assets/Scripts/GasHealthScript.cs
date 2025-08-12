using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GasHealthScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthScript.instance.RefillHealth();
            GameController.instance.UpdateLastCheckpoint();
            

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    GlitchController.instance.SetNoise(0.3f);
                    GlitchController.instance.SetGlitchStrength(0.3f);
                    GlitchController.instance.SetScanLines(0.8f);
                    break;
                case 1:
                    GlitchController.instance.SetNoise(1f);
                    GlitchController.instance.SetGlitchStrength(3f);
                    GlitchController.instance.SetScanLines(0.5f);
                    break;
                case 2:
                    GlitchController.instance.SetNoise(4f);
                    GlitchController.instance.SetGlitchStrength(11f);
                    GlitchController.instance.SetScanLines(0.4f);
                    break;
                // case 3:
                //     CharacterController.instance.ResetStates();
                //     break;
            }
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
