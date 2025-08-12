using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeListener : MonoBehaviour
{

    void Start()
    {
        GameController.instance.ChangeScene();
        //GameController.instance.DestroyDeadEnemies();
        // GameObject[] foundCodexPieces = GameObject.FindGameObjectsWithTag("Codex");
        // if (foundCodexPieces.Length == 0) return;

        // for (int i = 0; i < foundCodexPieces.Length; i++)
        // {
        //     GameController.instance.DestroyFoundCodexPiece();
        // }
        
    }

}
