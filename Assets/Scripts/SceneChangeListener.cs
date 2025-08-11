using System.Collections.Generic;
using UnityEngine;

public class SceneChangeListener : MonoBehaviour
{

    void Start()
    {
        GameController.instance.ChangeScene();
        GameController.instance.DestroyDeadEnemies();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Codex").Length; i++)
        {
            GameController.instance.DestroyFoundCodexPiece(GameObject.FindGameObjectsWithTag("Codex")[i]);
        }
        
    }

}
